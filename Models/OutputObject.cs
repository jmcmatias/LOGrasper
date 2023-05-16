using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace LOGrasper.Models
{
    internal class OutputObject : INotifyCollectionChanged
    {
        public ObservableCollection<Found> _ouputObject;

        public OutputObject(ObservableCollection<Found> ouputObject)
        {
            _ouputObject = ouputObject;
        }

        public event NotifyCollectionChangedEventHandler? CollectionChanged;

        public class Found
        {
            public Found(string path, string fileName, List<Line> linesFound)
            {
                Path = path;
                FileName = fileName;
                LinesFound = linesFound;
            }

            public string Path { get; set; }
            public string FileName { get; set; }
            public List<Line> LinesFound { get; set; }
        }
        public class Line
        {
            public Line(BigInteger number, List<int>? ocurrences)
            {
                Number = number;
                Ocurrences = ocurrences;
            }

            public BigInteger Number { get; set; }
            public List<int>? Ocurrences { get; set;}
        }
    }
}
