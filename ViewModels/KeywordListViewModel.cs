using LOGrasper.Commands;
using LOGrasper.Models;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace LOGrasper.ViewModels;

public class KeywordListViewModel : ViewModelBase
{

    private string _newKeyword;

    private KeywordViewModel _selectedKeyword;

    private bool _canEdit = false;

    private bool _notEmpty = false;



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

    public bool NotEmpty
    {
        get { return _notEmpty; }
        set
        {
            _notEmpty = value;
            OnPropertyChanged(nameof(NotEmpty));
        }


    }

    //KeywordList display

    public KeywordListViewModel(SearchViewViewModel searchViewViewmodel)
    {

        _keywordList = new ObservableCollection<KeywordViewModel>();
        OnPropertyChanged(nameof(_keywordList));
        AddKeywordCommand = new AddKeywordCommand(this, searchViewViewmodel);
        DeleteKeywordCommand = new DeleteKeywordCommand(this);

    }

    public KeywordListViewModel(ICommand addKeywordCommand, ICommand deleteKeywordCommand, ICommand editKeywordCommand)
    {
        AddKeywordCommand = addKeywordCommand;
        DeleteKeywordCommand = deleteKeywordCommand;
        EditKeywordCommand = editKeywordCommand;
    }
}