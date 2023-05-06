using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;

namespace LOGrasper.ViewModels;

public class KeywordListViewModel : ViewModelBase
{

    
    public List<string> KeywordList = new List<string>();

    public KeywordListViewModel()
    {
        KeywordList.Add("teste");
    }

    private string _keyword;

    public string Keyword
    {
        get
        {
            return _keyword;
        }
        set
        {
            _keyword = value;
            OnPropertyChanged(nameof(Keyword));
        }
    }

    public bool CanEdit
    {
        get
        {
            if (KeywordList.Count>0) return true;
            else return false;
        }
    }


    public ICommand AddKeywordCommand { get; }
    public ICommand EditKeywordCommand { get; }
    public ICommand DeleteKeywordCommand { get; }

}