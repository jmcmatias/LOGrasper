using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using System.Windows.Input;
using LOGrasper.Models;
using LOGrasper.ViewModels;

namespace LOGrasper.ViewModels;

public class SearchViewViewmodel : ViewModelBase
{
    public KeywordListViewModel KeywordListViewModel { get; set; }
    public OutputWindowViewModel OutputWindowViewModel { get; set; }

    public String RootFolder = "teste"; //será ""
    
    private string _messageDispenser;

    public string MessageDispenser { set => _messageDispenser = value; }

    public SearchViewViewmodel(KeywordList keywordList)
    {
        KeywordListViewModel = new KeywordListViewModel(keywordList);
        OutputWindowViewModel = new OutputWindowViewModel();
        MessageDispenser = "TESTE";
    }

  

    public bool CanSearch
    {
        get
        {
            if (KeywordListViewModel.CanEdit && RootFolder != "") return true;
            else return false;
        }
    }


}