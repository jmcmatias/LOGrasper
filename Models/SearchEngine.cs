using LOGrasper.Models.SearchAlgorithms;
using LOGrasper.ViewModels;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Shapes;
using static LOGrasper.Models.OutputObject;

namespace LOGrasper.Models
{
    public class SearchEngine : ViewModelBase
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


#pragma warning disable CS8618 // O campo não anulável precisa conter um valor não nulo ao sair do construtor. Considere declará-lo como anulável.
        public SearchEngine(SearchObject searchObject, SearchViewViewModel searchViewViewModel)
#pragma warning restore CS8618 // O campo não anulável precisa conter um valor não nulo ao sair do construtor. Considere declará-lo como anulável.
        {
            SearchObject = searchObject;
            _searchViewViewModel = searchViewViewModel;
            _outputWindowViewModel = searchViewViewModel.OutputWindowViewModel;
            _rootFolderBrowserViewModel = searchViewViewModel.RootFolderBrowseViewModel;
        }


        public async Task SearchAC(bool cancelSearch)
        {
            _searchViewViewModel.stopwatch = new();

            _searchViewViewModel.stopwatch.Start();

            totalFiles = _rootFolderBrowserViewModel.GetTotalFilesSelected();
            totalFilesSearched = 0;


            AhoCorasick ac = new();

            foreach (string kw in SearchObject._keywordList)
            {
                ac.AddKeyword(kw);
            }
            // Build AC Automaton
            ac.BuildAutomaton();

            await SearchFilesInFolder(SearchObject._rootFolderPath, OutputObject, SearchObject._keywordList ,ac, _outputWindowViewModel, _searchViewViewModel);

            await Task.WhenAll(searchTasks);


            _searchViewViewModel.stopwatch.Stop();
            _searchViewViewModel.StopwatchString = _searchViewViewModel.stopwatch.Elapsed.ToString();

            if (Task.WhenAll(searchTasks) == Task.CompletedTask)
            {
                _searchViewViewModel.SearchButton = "Acabou";
            }

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
                         
                            Task searchFile = Task.Factory.StartNew(() => SearchFileAsync(file), TaskCreationOptions.LongRunning); // Parallel processing

                            searchTasks.Add(searchFile);

                            await Application.Current.Dispatcher.InvokeAsync(() =>
                            {
                                searchViewViewModel.MessageDispenser = totalFilesSearched*100/totalFiles + "% of files - Searching @>>" + file;
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

                                List<Tuple<int, string>> matches = ac.Search(line); ;

                                // if 
                                if (kwList.All(item => matches.Any(tuple => tuple.Item2 == item)))
                                {
                                    FoundInFile.LineInfo lineinfo = new FoundInFile.LineInfo(n, line);
                                    lines.Add(lineinfo);
                                    lineFound = true;

                                }

                                // Output the line if all Keywords were found
                                n++;

                                //Console.WriteLine("Keywords Count" + keywords.Count);
                            }
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
            catch (Exception e)
            {
                // Display any errors that occur during the process
                Console.WriteLine($"Error: {e.Message}");
            }

            await Task.WhenAll(searchTasks);

           return Task.CompletedTask;
        }

        public static int PercentSearched()
        {
            return (totalFilesSearched/totalFiles)*100;
        }

    }
}

    