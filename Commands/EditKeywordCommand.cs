using LOGrasper.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOGrasper.Commands
{
    internal class EditKeywordCommand : CommandBase
    {
        private readonly KeywordListViewModel _keywordListViewModel;
        private readonly SearchViewViewModel _searchViewViewModel;

        private bool _hasSelection;

        public bool HasSelection
        {
            get { return _hasSelection; }
            set { _hasSelection = value; }
        }

        public EditKeywordCommand(KeywordListViewModel keywordListViewModel, SearchViewViewModel searchViewViewModel) 
        {
            _keywordListViewModel = keywordListViewModel;
            _searchViewViewModel = searchViewViewModel;
            _keywordListViewModel.PropertyChanged += keywordListViewModel_PropertyChanged;

        }

        private void keywordListViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_keywordListViewModel.SelectedKeyword))
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
        public override void Execute(object? parameter)
        {
            if (_keywordListViewModel.SelectedKeyword != null)
            {
                _keywordListViewModel.NewKeyword = _keywordListViewModel.SelectedKeyword.Keyword.ToString();
                _keywordListViewModel.IsEditing = true;
                _keywordListViewModel.SelectKeywordUnlock = !_keywordListViewModel.IsEditing;
                _keywordListViewModel.EditingButton();
                _searchViewViewModel.MessageDispenser = "Entered Keyword Edit Mode, Click Editing Button to Finish Editing";
            }
        }
        public override bool CanExecute(object? parameter)
        {
            return HasSelection;
        }

    }
}
