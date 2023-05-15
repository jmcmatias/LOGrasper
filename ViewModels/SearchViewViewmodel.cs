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
    public RootFolderBrowseViewModel RootFolderBrowseViewModel { get; set; }
    public KeywordListViewModel KeywordListViewModel { get; set; }
    public OutputWindowViewModel OutputWindowViewModel { get; set; }

    public String RootFolder = "teste"; //será ""
    
    private string? _messageDispenser;

    public string MessageDispenser { set => _messageDispenser = value; }

    public SearchViewViewmodel()
    {
        RootFolderBrowseViewModel = new RootFolderBrowseViewModel();
        KeywordListViewModel = new KeywordListViewModel();
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