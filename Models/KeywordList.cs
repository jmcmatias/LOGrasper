using System.Collections.Generic;
using System.Collections.ObjectModel;

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
