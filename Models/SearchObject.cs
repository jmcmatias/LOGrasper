using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOGrasper.Models
{
    public class SearchObject
    {
        public SearchObject(string rootFolderPath, KeywordList keywordList)
        {
            RootFolderPath = rootFolderPath;
            KeywordList = keywordList;
        }

        public string RootFolderPath { get; set; }
        public KeywordList KeywordList { get; set; }
        

    }
}
