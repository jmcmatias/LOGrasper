using LOGrasper.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOGrasper.Commands
{
    internal class DeleteKeywordCommand : CommandBase
    {
        // private readonly KeywordListViewModel _keywordListViewModel; // Viewmodel instance

        private readonly KeywordListViewModel _keywordListViewModel;

        public DeleteKeywordCommand(KeywordListViewModel KeywordListViewModel)
        {
            _keywordListViewModel = KeywordListViewModel;

        }

        public override void Execute(object parameter)
        {
            _keywordListViewModel._keywordList.Remove((_keywordListViewModel.SelectedKeyword));
        }
    }
}
