using LOGrasper.Commands;
using LOGrasper.Models;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace LOGrasper.ViewModels;

public class KeywordListViewModel : ViewModelBase
{

    private string _newKeyword;

    private KeywordViewModel _selectedKeyword;

    private bool _canEdit = true;

    public ObservableCollection<KeywordViewModel> _keywordList;

    public IEnumerable<KeywordViewModel> KeywordList => _keywordList;

    public ICommand AddKeywordCommand { get; }
    public ICommand DeleteKeywordCommand { get; }
    public ICommand EditKeywordCommand { get; }

    public string NewKeyword
    {
        get { return _newKeyword; }
        set
        {
            _newKeyword = value;
            OnPropertyChanged(nameof(NewKeyword));
        }
    }

    public KeywordViewModel SelectedKeyword
    {
        get { return _selectedKeyword; }
        set
        {
            _selectedKeyword = value;
            OnPropertyChanged(nameof(SelectedKeyword));
        }
    }

    public bool HasList() => _keywordList.Count > 0 ;

    public bool CanEdit
    {
        get { return _canEdit; }
        set
        {
            _canEdit = value;
            OnPropertyChanged(nameof(SelectedKeyword));
        }
    }


    //KeywordList display

    public KeywordListViewModel()
    {
        _keywordList = new ObservableCollection<KeywordViewModel>();

        AddKeywordCommand = new AddKeywordCommand(this);
        DeleteKeywordCommand = new DeleteKeywordCommand(this);

    }

    public KeywordListViewModel(ICommand addKeywordCommand, ICommand deleteKeywordCommand, ICommand editKeywordCommand)
    {
        AddKeywordCommand = addKeywordCommand;
        DeleteKeywordCommand = deleteKeywordCommand;
        EditKeywordCommand = editKeywordCommand;
    }
}