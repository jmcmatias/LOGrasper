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

        // Properties for binding the path and file name
        public string Path => _foundInFile.Path;
        public string FileName => _foundInFile.FileName;

        // Collection to store the lines found in the file
        private readonly ObservableCollection<LineInfoViewModel> _linesFound = new();
        public IEnumerable<LineInfoViewModel> LinesFound
        {
            get
            {
                return _linesFound;
            }
        }

        // Constructor
        public FoundInFileViewModel(OutputObject.FoundInFile foundInFile)
        {
            _foundInFile = foundInFile;

            // Convert the lines found to LineInfoViewModel and add them to the collection
            foreach (var line in foundInFile.LinesFound)
            {
                _linesFound.Add(new LineInfoViewModel(new FoundInFile.LineInfo(line.Number, line.Content, line.LightContent)));
            }
        }

        // Method for converting lines found to LineInfoViewModel
        public ObservableCollection<LineInfoViewModel> ConvertLinesFoundToViewmodel(FoundInFile linesFound)
        {
            ObservableCollection<LineInfoViewModel> lines = new();

            foreach (var line in linesFound.LinesFound)
            {
                lines.Add(new LineInfoViewModel(line));
            }

            return lines;
        }

        // Property for displaying the file name and path
        public string DisplayFile
        {
            get
            {
                return string.Format("{0} at {1}", FileName, Path);
            }
        }
    }

}
