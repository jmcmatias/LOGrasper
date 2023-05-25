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
        public static OutputWindowViewModel? OutputWindowViewModel { get; set; }

        public static SearchViewViewModel? ViewModel { get; set; }

        public SearchEngine(SearchObject searchObject, OutputWindowViewModel outputWindowViewModel) 
        {
            SearchObject = searchObject;
            OutputWindowViewModel = outputWindowViewModel;
        }


        public void SearchAC()
        {
            Stopwatch stopwatch = new Stopwatch();
            AhoCorasick ac = new();
            stopwatch.Start();
            foreach (string kw in SearchObject._keywordList)
            {
                ac.AddKeyword(kw);
            }

            ac.BuildAutomaton();
            stopwatch.Stop();


            SearchFilesInFolder(SearchObject._rootFolderPath, OutputObject, SearchObject._keywordList ,ac, OutputWindowViewModel);

           
        }


        static void SearchFilesInFolder(string folder, OutputObject outputObject, List<string> kwList, AhoCorasick ac, OutputWindowViewModel outputWindowViewModel)
        {
            
            try
            {
         
                if (Directory.GetFiles(folder).Length != 0)
                {
                    foreach (string file in Directory.GetFiles(folder))
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


                                List<Tuple<int, string>> matches = ac.Search(line);

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

                                outputObject._ouputObject.Add(foundInFile);
                                outputWindowViewModel.UpdateOutput(outputObject);


                                //ARRANJAR MANEIRA DE SABER QUANDO É ADICIONADO ALGO NO OUTPUTOBJECT
                            }

                        }
                    }
                }

                foreach (string subFolder in Directory.GetDirectories(folder))
                {
                    SearchFilesInFolder(subFolder, outputObject,kwList, ac, outputWindowViewModel);
                }
                


            }
            catch (Exception e)
            {
                // Display any errors that occur during the process
                Console.WriteLine($"Error: {e.Message}");
            }

        }

    }
}

    