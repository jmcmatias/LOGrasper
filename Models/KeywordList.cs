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
    public class KeywordList
    {
        private readonly ObservableCollection<string> _keywordList;

        public KeywordList()
        {
            _keywordList = new ObservableCollection<string>();
        }

        public int Count()
        {
            return _keywordList.Count;
        }

        public void Add(string keyword)
        {
            _keywordList.Add(keyword);
        }

        public void Remove(string keyword) 
        {
            _keywordList?.Remove(keyword);
        }

        public void Clear() 
        {
            _keywordList.Clear();
        }
    }
}
