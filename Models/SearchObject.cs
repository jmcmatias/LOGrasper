using LOGrasper.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOGrasper.Models
{
    public class SearchObject
    {
        public ObservableCollection<KeywordViewModel> _keywordList { get; set; }  // Represents the keyword list used for the search
        public string _rootFolderPath { get; set; }  // Represents the root folder path to search within
        public SearchObject(string rootFolderPath, ObservableCollection<KeywordViewModel> keywordList)
        {
            _rootFolderPath = rootFolderPath;  // Assign the root folder path passed as an argument to the corresponding property

            _keywordList = keywordList;  // Convert the keyword list from the ViewModel to the internal KeywordList type
        }
    }
}
