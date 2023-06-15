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
        private readonly SearchViewViewModel _searchViewViewmodel;

        public AddKeywordCommand(KeywordListViewModel KeywordListViewModel, SearchViewViewModel searchViewViewmodel)
        {
            _keywordListViewModel = KeywordListViewModel;
            _searchViewViewmodel = searchViewViewmodel;
            _keywordListViewModel.PropertyChanged += keywordListViewModel_PropertyChanged;  

        }



        public override void Execute(object parameter)
        {
            _keywordListViewModel._keywordList.Add(new KeywordViewModel(_keywordListViewModel.NewKeyword));
            if (_keywordListViewModel.KeywordList.Any())
            {
                _searchViewViewmodel.HasKeywordList=true;
            }
        }

        public override bool CanExecute(object? parameter)
        {
            return !string.IsNullOrEmpty(_keywordListViewModel.NewKeyword) && base.CanExecute(parameter);
        }

        private void keywordListViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if(e.PropertyName == nameof(_keywordListViewModel.NewKeyword)) 
            {
                OnCanExecuteChanged();
            }
        }

    }
}
