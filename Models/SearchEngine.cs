using LOGrasper.Models.SearchAlgorithms;
using LOGrasper.ViewModels;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using static LOGrasper.Models.OutputObject;

namespace LOGrasper.Models
{
    public class SearchEngine
    {
        private SearchObject _searchObject;
        private OutputObject _outputObject = new();

        // Variables for tracking the total number of files and files searched
        public static int totalFiles, totalFilesSearched;

        public SearchObject SearchObject { get => _searchObject; set => _searchObject = value; }
        public OutputObject OutputObject { get => _outputObject; set => _outputObject = value; }

        private readonly OutputWindowViewModel _outputWindowViewModel;
        private readonly RootFolderBrowseViewModel _rootFolderBrowserViewModel;
        public static SearchViewViewModel _searchViewViewModel;

        // ConcurrentBag to store search tasks
        private static ConcurrentBag<Task> searchTasks = new ConcurrentBag<Task>();

        // SemaphoreSlim for limiting the maximum number of parallel tasks
        private static SemaphoreSlim? maxTasks; 

        // Variable for tracking the total number of search tasks
        public static int TotalSearchTasks;

        // Maximum line size for clipping line contents
        private const int maxLineSize = 9600;

        public SearchEngine(SearchObject searchObject, SearchViewViewModel searchViewViewModel)
        {
            _searchObject = searchObject;
            _searchViewViewModel = searchViewViewModel;
            _outputWindowViewModel = searchViewViewModel.OutputWindowViewModel;
            _rootFolderBrowserViewModel = searchViewViewModel.RootFolderBrowseViewModel;
            maxTasks = _searchViewViewModel.Semaphore;
            TotalSearchTasks = 0;
        }

        public async Task SearchAC()
        {
            _searchViewViewModel.stopwatch = new();
            _searchViewViewModel.stopwatch.Start();

            // Get the total number of files selected for search
            totalFiles = _rootFolderBrowserViewModel.GetTotalFilesSelected();
            totalFilesSearched = 0;

            // Create AhoCorasick object
            AhoCorasick? ac = new();

            // Add keywords to the AhoCorasick Automaton
            foreach (string kw in SearchObject._keywordList)
            {
                ac.AddKeyword(kw);
            }

            // Build AC Automaton
            ac.BuildAutomaton();
            TotalSearchTasks = 0;

            // Start searching files in the root folder
            await SearchFilesInFolder(SearchObject._rootFolderPath, OutputObject, SearchObject._keywordList, ac, _outputWindowViewModel, _searchViewViewModel);

            await Task.WhenAll(searchTasks);

            _searchViewViewModel.stopwatch.Stop();
            _searchViewViewModel.StopwatchString = _searchViewViewModel.stopwatch.Elapsed.ToString();

            // Update the message dispenser with the search completion message
            await Application.Current.Dispatcher.InvokeAsync(() =>
            {
                _searchViewViewModel.MessageDispenser = "Search Finished in " + _searchViewViewModel.StopwatchString;
            });
        }

        // Recursive method to search for files in a given folder and its subfolders
        static async Task<Task> SearchFilesInFolder(string folder, OutputObject outputObject, List<string> kwList, AhoCorasick ac, OutputWindowViewModel outputWindowViewModel, SearchViewViewModel searchViewViewModel)
        {
            try
            {
                // If the folder has no files, skip to the next subfolder
                if (Directory.GetFiles(folder).Length != 0)
                {
                    foreach (string file in Directory.GetFiles(folder))
                    {
                        // Check if the search operation has been canceled
                        if (searchViewViewModel.CancellationFlag)
                        {
                            break;
                        }

                        // Wait for available task slot
                        maxTasks.Wait();

                        // Start a new task to search the file asynchronously
                        Task searchFile = Task.Factory.StartNew(() => SearchFileAsync(file), TaskCreationOptions.LongRunning).ContinueWith((task) => maxTasks.Release());

                        // Add the search task to the concurrent bag
                        searchTasks.Add(searchFile);
                        TotalSearchTasks++;

                        // Update the message dispenser with the current search progress
                        await Application.Current.Dispatcher.InvokeAsync(() =>
                        {
                            _searchViewViewModel.MessageDispenser = totalFilesSearched * 100 / totalFiles + "% of files Completed, Searching @>>" + file;
                        });
                    }
                }

                // Recursively search files in subfolders
                foreach (string subFolder in Directory.GetDirectories(folder))
                {
                    await SearchFilesInFolder(subFolder, outputObject, kwList, ac, outputWindowViewModel, searchViewViewModel);
                }

                // Asynchronously search a file
                async Task<Task> SearchFileAsync(string file)
                {
                    int n = 1;
                    bool lineFound = false;
                    FoundInFile foundInFile = new();
                    ObservableCollection<FoundInFile.LineInfo> lines = new();

                    // Read the file line by line
                    using (StreamReader reader = new StreamReader(file))
                    {
                        string? line;

                        while ((line = reader.ReadLine()) != null && !searchViewViewModel.CancellationFlag)
                        {
                            string lightContent = string.Empty;

                            // Search for matches using AhoCorasick
                            List<Tuple<int, string>>? matches = ac.Search(line, kwList);

                            // Check if all keywords were found in the line
                            if (matches != null && kwList.All(item => matches.Any(tuple => tuple.Item2 == item)))
                            {
                                if (line.Length > maxLineSize)
                                {
                                    lightContent = line[..maxLineSize] + "\n !!! ATTENTION ... Line length was too big and was clipped for performance issues, if you save the output, you will get the full line, thank you ... ATTENTION !!!";
                                }
                                else
                                {
                                    lightContent = line;
                                }

                                // Create a LineInfo object and add it to the collection
                                FoundInFile.LineInfo lineinfo = new FoundInFile.LineInfo(n, line, lightContent);
                                lines.Add(lineinfo);
                                lineFound = true;
                            }

                            n++;

                            // Update the system info with the number of free search task slots
                            _searchViewViewModel.SystemInfo = "Number of Free Search Tasks Slots: " + maxTasks.CurrentCount;
                        }

                        // Output the line if all keywords were found
                        if (lineFound)
                        {
                            foundInFile.Add(folder, file, lines);

                            // Update the output window with the found file information
                            await Application.Current.Dispatcher.InvokeAsync(() =>
                            {
                                outputObject._ouputObject.Add(foundInFile);
                                outputWindowViewModel.UpdateOutput(outputObject);
                            });
                        }
                    }

                    // Increment the total number of files searched
                    totalFilesSearched++;

                    return Task.CompletedTask;
                }
            }
            catch (Exception ex)
            {
                // Update the message dispenser with the error message
                _searchViewViewModel.MessageDispenser = "An error has occurred while trying to search in files: " + ex.Message;
            }

            await Task.WhenAll(searchTasks);

            return Task.CompletedTask;
        }

        public static int PercentSearched()
        {
            return (totalFilesSearched / totalFiles) * 100;
        }

        public int GetTotalSearchTasks()
        {
            return TotalSearchTasks;
        }
    }
}

