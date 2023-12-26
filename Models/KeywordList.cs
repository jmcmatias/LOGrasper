using LOGrasper.ViewModels;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace LOGrasper.Models
{
    public class KeywordList : List<KeywordViewModel>
    {
        private readonly ObservableCollection<KeywordViewModel> _keywordList;

        public KeywordList()
        {
            _keywordList = new ObservableCollection<KeywordViewModel>();
        }

    }
}
