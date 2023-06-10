using LOGrasper.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

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
                public LineInfo(int number, string content)
                {
                    Number = number;
                    Content = content;
                }

                public int Number { get; set; }
                public string Content { get; set; }
            }
        }

     
    }
}
