using LOGrasper.Models;
using LOGrasper.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOGrasper.Commands
{
    internal class AddKeywordCommand : CommandBase
    {
       // private readonly KeywordListViewModel _keywordListViewModel; // Viewmodel instance

        private readonly KeywordList _keywordList;
        private readonly Keyword _newKeyword;

        public AddKeywordCommand(KeywordListViewModel KeywordListViewModel)
        {
               _keywordList = KeywordListViewModel._keywordList;
               _newKeyword = KeywordListViewModel.NewKeyword;
        }

        public override void Execute(object parameter)
        {
            _keywordList.AddKeyword(_newKeyword);
        }
    }
}
