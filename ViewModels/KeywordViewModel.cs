using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using LOGrasper.Models;

namespace LOGrasper.ViewModels
{
    /// <summary>
    /// Needed for Text Binding in View (show List Items), because it implements INotify
    /// </summary>
    public class KeywordViewModel : ViewModelBase
    {
        private string? _keyword;
        private bool _hasNotClause;
        private SolidColorBrush _keywordColor;
        public string Keyword
        {
            get { return _keyword; }
            set
            {
                _keyword = value;
                _keywordColor = new SolidColorBrush(Colors.DarkOliveGreen); // Initialize with default color
            }
        }

        public bool IsNot
        {
            get { return _hasNotClause; }
            set
            {
                _hasNotClause = value;
            }
        }

        public KeywordViewModel(string Keyword)
        {
            _keyword = Keyword;
            _keywordColor = new SolidColorBrush(Colors.DarkOliveGreen); // Initialize with default color
        }

        public SolidColorBrush KeywordColor
        {
            get { return _keywordColor; }
            set
            {
                _keywordColor = value;
                OnPropertyChanged(nameof(KeywordColor));
            }
        }
    }
}
