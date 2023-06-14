using LOGrasper.Models.SearchAlgorithms;
using LOGrasper.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
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


        public SearchObject SearchObject { get => _searchObject; set => _searchObject = value; }
        public OutputObject OutputObject { get => _outputObject; set => _outputObject = value; }
        private static OutputWindowViewModel? _outputWindowViewModel { get; set; }

        private static SearchViewViewModel? _searchViewViewModel { get; set; }

        public SearchEngine(SearchObject searchObject, OutputWindowViewModel outputWindowViewModel, SearchViewViewModel searchViewViewModel) 
        {
            SearchObject = searchObject;
            _outputWindowViewModel = outputWindowViewModel;
            _searchViewViewModel = searchViewViewModel;
        }


        public async Task SearchAC()
        {
            _searchViewViewModel.stopwatch = new();

            _searchViewViewModel.stopwatch.Start();


            AhoCorasick ac = new();

            foreach (string kw in SearchObject._keywordList)
            {
                ac.AddKeyword(kw);
            }

            ac.BuildAutomaton();

            await SearchFilesInFolder(SearchObject._rootFolderPath, OutputObject, SearchObject._keywordList ,ac, _outputWindowViewModel, _searchViewViewModel);

            _searchViewViewModel.stopwatch.Stop();
            _searchViewViewModel.StopwatchString = _searchViewViewModel.stopwatch.Elapsed.ToString();

            await Application.Current.Dispatcher.InvokeAsync(() =>
            {
                _searchViewViewModel.MessageDispenser = "Search Finished in " + _searchViewViewModel.StopwatchString;
            });

        }


        static async Task<Task> SearchFilesInFolder(string folder, OutputObject outputObject, List<string> kwList, AhoCorasick ac, OutputWindowViewModel outputWindowViewModel, SearchViewViewModel searchViewViewModel)
        {
            List<Task> searchTasks = new List<Task>();

            try
            {
                // If the folder has no files skip to the next subfolder
                if (Directory.GetFiles(folder).Length != 0)
                {
                    foreach (string file in Directory.GetFiles(folder))
                    {
                       Task searchFile = SearchFileAsync(file);
                       searchTasks.Add(searchFile);
                        
                        await Application.Current.Dispatcher.InvokeAsync(() =>
                        {
                           searchViewViewModel.MessageDispenser = "Searching "+ searchTasks.Count +" Tasks @>>" + file;
                        });
                        
                    }
                }

                foreach (string subFolder in Directory.GetDirectories(folder))
                {
                    await SearchFilesInFolder(subFolder, outputObject,kwList, ac, outputWindowViewModel, searchViewViewModel);
                }
                
                async Task<Task> SearchFileAsync(string file)
                {
                    //Console.WriteLine($"Reading file: {file}");
                    int n = 1;
                    bool lineFound = false;
                    FoundInFile foundInFile = new();
                    ObservableCollection<FoundInFile.LineInfo> lines = new();
                    using (StreamReader reader = new StreamReader(file))
                    {
                        string? line;


                        while ((line = reader.ReadLine()) != null)
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

    }
}

    