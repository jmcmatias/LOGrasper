using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LOGrasper.Models;

namespace LOGrasper.ViewModels
{
    /// <summary>
    /// Needed for Text Binding in View (show List Items), because it implements INotify
    /// </summary>
    public class KeywordViewModel : ViewModelBase
    {
        private readonly string _keyword;

        public string Keyword => _keyword;
    public KeywordViewModel(string Keyword)
        {
            _keyword = Keyword;
        }
    }
}
