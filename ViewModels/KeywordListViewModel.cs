using LOGrasper.Commands;
using LOGrasper.Models;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace LOGrasper.ViewModels;

public class KeywordListViewModel : ViewModelBase
{

    public KeywordList _keywordList;

    private Keyword _newKeyword;


    public Keyword NewKeyword
    {
        get
        {
            return _newKeyword;
        }
        set
        {
            _newKeyword = value;
            OnPropertyChanged(nameof(NewKeyword));
        }
    }


    
    public bool CanEdit
    {
        get
        {
            //if (KeywordList.Count>0) return true;
            //else return false;
            return true;
        }
    }

    public ICommand AddKeywordCommand { get; }
    public ICommand DeleteKeywordCommand { get; }
    public ICommand EditKeywordCommand { get; }



    //KeywordList display

    public KeywordListViewModel(KeywordList keywordList)
    {
        _keywordList = keywordList;
        Keyword test = new("teste");
        
        _keywordList.AddKeyword (test);
        _newKeyword = NewKeyword;
        AddKeywordCommand = new AddKeywordCommand(this);
        
    }

    public KeywordListViewModel(ICommand addKeywordCommand, ICommand deleteKeywordCommand, ICommand editKeywordCommand)
    {
        AddKeywordCommand = addKeywordCommand;
        DeleteKeywordCommand = deleteKeywordCommand;
        EditKeywordCommand = editKeywordCommand;
    }
}