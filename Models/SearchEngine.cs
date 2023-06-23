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
        private  SearchObject _searchObject;
        private  OutputObject _outputObject = new();

        public static int totalFiles, totalFilesSearched;
        public SearchObject SearchObject { get => _searchObject; set => _searchObject = value; }
        public OutputObject OutputObject { get => _outputObject; set => _outputObject = value; }

        private readonly OutputWindowViewModel _outputWindowViewModel;
        private readonly RootFolderBrowseViewModel _rootFolderBrowserViewModel;
        public static SearchViewViewModel _searchViewViewModel;

        private static ConcurrentBag<Task> searchTasks = new ConcurrentBag<Task>();
        private static SemaphoreSlim? maxTasks;   // testar várias opções
        public static int TotalSearchTasks;

        private const int maxLineSize = 9600; // restrict Lines to be presented in UI to 9600 chars, for performance issues, Used for Clipping line contents into Lightcontent
 
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

            totalFiles = _rootFolderBrowserViewModel.GetTotalFilesSelected();
            totalFilesSearched = 0;


            AhoCorasick? ac = new();

            foreach (string kw in SearchObject._keywordList)
            {
                ac.AddKeyword(kw);
            }
            // Build AC Automaton
            ac.BuildAutomaton();
            TotalSearchTasks = 0;
            await SearchFilesInFolder(SearchObject._rootFolderPath, OutputObject, SearchObject._keywordList ,ac, _outputWindowViewModel, _searchViewViewModel);

            await Task.WhenAll(searchTasks);

            _searchViewViewModel.stopwatch.Stop();
            _searchViewViewModel.StopwatchString = _searchViewViewModel.stopwatch.Elapsed.ToString();
            
            await Application.Current.Dispatcher.InvokeAsync(() =>
            {
                _searchViewViewModel.MessageDispenser = "Search Finished in " + _searchViewViewModel.StopwatchString;
            });

        }


        static async Task<Task> SearchFilesInFolder(string folder, OutputObject outputObject, List<string> kwList, AhoCorasick ac, OutputWindowViewModel outputWindowViewModel, SearchViewViewModel searchViewViewModel)
        {

            try
            {
                   // If the folder has no files skip to the next subfolder
                    if (Directory.GetFiles(folder).Length != 0)
                    {
                        foreach (string file in Directory.GetFiles(folder))
                        {
                        if (searchViewViewModel.CancellationFlag)
                        {
                            break;
                        }
                            maxTasks.Wait();
                            Task searchFile = Task.Factory.StartNew(() => SearchFileAsync(file), TaskCreationOptions.LongRunning).ContinueWith ((task) => maxTasks.Release()); // Parallel processing
                            searchTasks.Add(searchFile);
                            TotalSearchTasks++;
                            
                            await Application.Current.Dispatcher.InvokeAsync(() =>
                            {
                                _searchViewViewModel.MessageDispenser = totalFilesSearched*100/totalFiles + "% of files Completed, Searching @>>" + file;
                            });
                            
                        }
                    }

                    foreach (string subFolder in Directory.GetDirectories(folder))
                    {
                        await SearchFilesInFolder(subFolder, outputObject, kwList, ac, outputWindowViewModel, searchViewViewModel);
                    }

                    async Task<Task> SearchFileAsync(string file)
                    {
                        //Console.WriteLine($"Reading file: {file}");
                        int n = 1;
                        bool lineFound = false;
                        FoundInFile foundInFile = new();
                        ObservableCollection<FoundInFile.LineInfo> lines = new();

                    // using means that once the block is exited de StreamReader is automatically disposed
                        using (StreamReader reader = new StreamReader(file))
                        {
                            string? line;

                        while ((line = reader.ReadLine()) != null && !searchViewViewModel.CancellationFlag)
                        {
                            string lightContent = string.Empty;
                            List<Tuple<int, string>>? matches = ac.Search(line);
                                                        
                            // Check if all keywords where found
                            if (matches != null && kwList.All(item => matches.Any(tuple => tuple.Item2 == item)))
                            {
                                if (line.Length > maxLineSize)
                                {
                                    lightContent = line[..maxLineSize] + "\n !!! ATTENTION ... Line length was too big and was clipped for performance issues, if you save the output, you will get the full line, thank you ... ATTENTION !!!";
                                }
                                else lightContent = line;

                                FoundInFile.LineInfo lineinfo = new FoundInFile.LineInfo(n, line, lightContent);
                                lines.Add(lineinfo);
                                lineFound = true;
                            }

                            
                            n++;

                            _searchViewViewModel.SystemInfo = "Number of Free Search Tasks Slots: " + maxTasks.CurrentCount;                          
                            
                        }
                        // Output the line if all Keywords were found
                        if (lineFound)
                            {
                                foundInFile.Add(folder, file, lines);

                                await Application.Current.Dispatcher.InvokeAsync(() =>
                                {
                                    outputObject._ouputObject.Add(foundInFile);
                                    outputWindowViewModel.UpdateOutput(outputObject);
                                });
                            
                            }
                        }
                        totalFilesSearched++;
                        
                       return Task.CompletedTask;
                    }
                
            }
            catch (Exception ex)
            {
                _searchViewViewModel.MessageDispenser = "An error has Ocurred while trying to Search in Files: "+ ex.Message;
            }

            await Task.WhenAll(searchTasks);

           return Task.CompletedTask;
        }

        public static int PercentSearched()
        {
            return (totalFilesSearched/totalFiles)*100;
        }
        
        public int GetTotalSearchTasks()
        {
            return TotalSearchTasks;
        }
    }
}

    