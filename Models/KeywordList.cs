using LOGrasper.Exceptions;
using LOGrasper.ViewModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LOGrasper.Models
{
    public class KeywordList : List<string>
    {
        private readonly ObservableCollection<string> _keywordList;

        public KeywordList()
        {
            _keywordList = new ObservableCollection<string>();
        }

    }
}
