using LOGrasper.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOGrasper.Commands
{
    internal class SearchCommand : CommandBase
    {

        public bool _canSearch;

        private readonly SearchViewViewModel _searchViewViewModel;
        private readonly RootFolderBrowseViewModel _rootFolderBrowseViewModel;
        private readonly KeywordListViewModel _keywordListViewModel;
        private readonly OutputWindowViewModel _outputWindowViewModel;

        public SearchCommand(SearchViewViewModel searchViewViewModel) 
        {
            _searchViewViewModel = searchViewViewModel;
            _rootFolderBrowseViewModel = _searchViewViewModel.RootFolderBrowseViewModel;
            _keywordListViewModel = _searchViewViewModel.KeywordListViewModel;
            _outputWindowViewModel = _searchViewViewModel.OutputWindowViewModel;

            _searchViewViewModel.PropertyChanged += _searchViewViewmodel_PropertyChanged;

        }



        private void _searchViewViewmodel_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_searchViewViewModel.HasKeywordList) || e.PropertyName == nameof(_searchViewViewModel.HasRootFolder))
            {
                OnCanExecuteChanged();
            }          
        }

        public override bool CanExecute(object? parameter)
        {
            return _searchViewViewModel.HasKeywordList && _searchViewViewModel.HasRootFolder;
        }

        public override void Execute(object parameter)
        {
            _searchViewViewModel.InitiateSearch(_rootFolderBrowseViewModel, _keywordListViewModel);
        }

       


    }
}
