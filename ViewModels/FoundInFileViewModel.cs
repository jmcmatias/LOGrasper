using LOGrasper.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using static LOGrasper.Models.OutputObject;


namespace LOGrasper.ViewModels
{
    public class FoundInFileViewModel : ViewModelBase
    {
        private readonly FoundInFile _foundInFile;

        public string Path => _foundInFile.Path;
        public string FileName => _foundInFile.FileName;

        private readonly ObservableCollection<LineInfoViewModel> _linesFound = new();
        public IEnumerable<LineInfoViewModel> LinesFound
        {
            get 
            { 
                return _linesFound; 
            }
        }
            





        public FoundInFileViewModel(OutputObject.FoundInFile foundInFile)
        {
            _foundInFile = foundInFile;
            foreach (var line in foundInFile.LinesFound)
            {
                _linesFound.Add(new LineInfoViewModel(new FoundInFile.LineInfo(line.Number, line.Content)));
            }
        }


        public ObservableCollection<LineInfoViewModel> ConvertLinesFoundToViewmodel(FoundInFile linesFound)
        {
            ObservableCollection<LineInfoViewModel> lines = new();

            foreach (var line in linesFound.LinesFound)
            {
                lines.Add(new LineInfoViewModel(line));
            }

            return lines;
        }


        public string DisplayFile
        {
            get
            {
                return string.Format("{0} at {1}", FileName, Path);
            }
        }

    }

}
