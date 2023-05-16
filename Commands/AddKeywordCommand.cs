using LOGrasper.Exceptions;
using LOGrasper.Models;
using LOGrasper.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOGrasper.Commands
{
    internal class AddKeywordCommand : CommandBase
    {
       // private readonly KeywordListViewModel _keywordListViewModel; // Viewmodel instance

        private readonly KeywordListViewModel _keywordListViewModel;

        public AddKeywordCommand(KeywordListViewModel KeywordListViewModel)
        {
               _keywordListViewModel = KeywordListViewModel;

        }

        public override void Execute(object parameter)
        {
            _keywordListViewModel._keywordList.Add(new KeywordViewModel(_keywordListViewModel.NewKeyword));
            if (_keywordListViewModel.HasList())
            {
                _keywordListViewModel.CanEdit = true;
            }
        }
    }
}
