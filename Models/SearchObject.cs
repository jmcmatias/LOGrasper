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
        public KeywordList _keywordList { get; set; }

        public string _rootFolderPath { get; set; }

        public SearchObject(string rootFolderPath, ObservableCollection<KeywordViewModel> keywordList)
        {
            _rootFolderPath = rootFolderPath;
            _keywordList = ConvertListFromViewModel(keywordList);
        }

        public static KeywordList ConvertListFromViewModel(ObservableCollection<KeywordViewModel> keywordList )
        {
            KeywordList kwList = new();
            foreach ( KeywordViewModel keywordViewModel in keywordList )
            {
                kwList.Add(keywordViewModel.Keyword);
            }
            return kwList;

        }
      
        


    }
}
