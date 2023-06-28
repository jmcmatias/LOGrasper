using System.Collections.ObjectModel;


namespace LOGrasper.Models
{
    public class OutputObject 
    {
        public ObservableCollection<FoundInFile> _ouputObject;

        public OutputObject()
        {
            _ouputObject = new();
        }

        public class FoundInFile : ObservableCollection<FoundInFile>
        {
           

            public FoundInFile() { }

            public string Path { get; set; }
            public string FileName { get; set; }
            public ObservableCollection<LineInfo> LinesFound { get; set; }

            public FoundInFile(string path, string fileName, ObservableCollection<LineInfo> linesFound)
            {
                Path = path;
                FileName = fileName;
                LinesFound = linesFound;
            }

            public void Add(string folder, string file, ObservableCollection<LineInfo> linesFound)
            {
                Path = folder;
                FileName = file;
                LinesFound = linesFound;
            }

            /// <summary>
            /// Nested Lineinfo
            /// </summary>
            public class LineInfo
            {
                public LineInfo(int number, string content, string lightContent)
                {
                    Number = number;
                    Content = content;
                    LightContent = lightContent;
                }

                public int Number { get; set; }
                public string Content { get; set; }
                public string LightContent { get; set; }
            }
        }

     
    }
}
