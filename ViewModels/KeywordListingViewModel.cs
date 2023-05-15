using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LOGrasper.Models;

namespace LOGrasper.ViewModels
{
    /// <summary>
    /// Needed for Text Binding in View (show List Items)
    /// </summary>
    public class KeywordListingViewModel : ViewModelBase
    {
        public readonly Keyword _keyword;

        public Keyword Keyword => _keyword;
    public KeywordListingViewModel(Keyword Keyword)
        {
            _keyword= Keyword;
        }
    }
}
