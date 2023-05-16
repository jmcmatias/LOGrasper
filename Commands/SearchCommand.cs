using LOGrasper.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOGrasper.Commands
{
    internal class SearchCommand : CommandBase
    {

        public bool _canSearch;

        private readonly SearchViewViewmodel _searchViewViewmodel;
        private readonly RootFolderBrowseViewModel _rootFolderBrowseViewModel;
        private readonly KeywordListViewModel _keywordListViewModel;

        public SearchCommand(SearchViewViewmodel searchViewViewmodel, RootFolderBrowseViewModel rootFolderBrowseViewModel, KeywordListViewModel keywordListViewModel) 
        {
            _searchViewViewmodel = searchViewViewmodel;
            _rootFolderBrowseViewModel = rootFolderBrowseViewModel;
            _keywordListViewModel = keywordListViewModel;
        }

        public override void Execute(object parameter)
        {
            _searchViewViewmodel.InitiateSearch(_rootFolderBrowseViewModel, _keywordListViewModel);
        }
    }
}
