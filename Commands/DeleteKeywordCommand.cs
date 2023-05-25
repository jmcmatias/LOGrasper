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

        private bool _hasSelection;

        public bool HasSelection
        {
            get { return _hasSelection; }
            set { _hasSelection = value; }
        }

        public DeleteKeywordCommand(KeywordListViewModel KeywordListViewModel)
        {
            _keywordListViewModel = KeywordListViewModel;

            _keywordListViewModel.PropertyChanged += keywordListViewModel_PropertyChanged;

        }

        private void keywordListViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if(e.PropertyName == nameof(_keywordListViewModel.SelectedKeyword))
            {
                if (_keywordListViewModel.SelectedKeyword != null)
                {
                    HasSelection = true;
                    OnCanExecuteChanged();
                } 
                else
                {
                    HasSelection = false; 
                    OnCanExecuteChanged();
                }
            }
        }

        public override bool CanExecute(object? parameter)
        {
            return HasSelection;
        }

        public override void Execute(object parameter)
        {
            _keywordListViewModel._keywordList.Remove((_keywordListViewModel.SelectedKeyword));
        }
    }
}
