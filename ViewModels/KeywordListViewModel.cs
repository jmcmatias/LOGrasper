﻿using LOGrasper.Commands;
using LOGrasper.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Data;
using System.Windows;
using System.Windows.Input;
using System.Windows.Documents;
using System.Windows.Controls;
using System.Windows.Media;

namespace LOGrasper.ViewModels;

public class KeywordListViewModel : ViewModelBase
{

    private string _newKeyword = "Add Keywords";

    private KeywordViewModel _selectedKeyword;

    private static bool _isEditing = false;
    private static bool _selectKeywordUnlock = true;

    private bool _notEmpty = false;

    private string _Info = "PLEASE NOTICE: Keywords are CASE SENSITIVE.\n" +
                           "LOGrasper will only output Lines that contain ALL the Keywords in this List.\n" +
                           "LEGEND:\n" +
                           "Green - Keywords to search.\n" +
                           "Red - Not Clause, invalidate lines that contain keyword.";
    private string _AddButton = "ADD";
    private string _AddButtonColor = "#FEB1FE";
    private int _AddButtonSize = 40;

    private SolidColorBrush _KeywordColor = new SolidColorBrush(Colors.ForestGreen);

    public SolidColorBrush _StandardKeywordColor = new SolidColorBrush(Colors.ForestGreen);  // Dark Green
    public SolidColorBrush _HasNotClauseColor = new SolidColorBrush(Colors.DarkRed); // Dark Red
    
    public ObservableCollection<KeywordViewModel> _keywordList;

    public IEnumerable<KeywordViewModel> KeywordList => _keywordList;
   

    public ICommand AddKeywordCommand { get; }
    public ICommand DeleteKeywordCommand { get; }
    public ICommand EditKeywordCommand { get; }
    public ICommand NotKeywordCommand { get; }

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
        get => _selectedKeyword;
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

    public bool IsEditing
    {
        get { return _isEditing; }
        set
        {
            _isEditing = value;
            OnPropertyChanged(nameof(IsEditing));
        }
    }
    public bool SelectKeywordUnlock
    {
        get { return _selectKeywordUnlock; }
        set
        {
            _selectKeywordUnlock = value;
            OnPropertyChanged(nameof(SelectKeywordUnlock));
        }
    }

    public string AddButton
    {
        get => _AddButton;
        set
        {
            _AddButton = value;
            OnPropertyChanged(nameof(AddButton));
        }
    }

    public string AddButtonColor
    {
        get => _AddButtonColor;
        set
        {
            _AddButtonColor = value;
            OnPropertyChanged(nameof(AddButtonColor));
        }
    }

    public int AddButtonSize
    {
        get => _AddButtonSize;
        set
        {
            _AddButtonSize = value;
            OnPropertyChanged(nameof(AddButtonSize));
        }
    }

    public string Info
    {
        get => _Info;
    }

    public SolidColorBrush KeywordColor
    {
        get => _KeywordColor;
        set
        {
            _KeywordColor = value;
            OnPropertyChanged(nameof(_KeywordColor));
        }
    }

    //KeywordList display

    public KeywordListViewModel(SearchViewViewModel searchViewViewmodel)
    {
        _keywordList = new ObservableCollection<KeywordViewModel>();
        AddKeywordCommand = new AddKeywordCommand(this, searchViewViewmodel);
        DeleteKeywordCommand = new DeleteKeywordCommand(this, searchViewViewmodel);
        EditKeywordCommand = new EditKeywordCommand(this, searchViewViewmodel);
        NotKeywordCommand = new NotKeywordCommand(this, searchViewViewmodel);
    }

    public KeywordListViewModel(ICommand addKeywordCommand, ICommand deleteKeywordCommand, ICommand editKeywordCommand, ICommand notKeywordCommand)
    {
        AddKeywordCommand = addKeywordCommand;
        DeleteKeywordCommand = deleteKeywordCommand;
        EditKeywordCommand = editKeywordCommand;
        NotKeywordCommand = notKeywordCommand;
    }

    public void EditingButton()
    {
        AddButton = "EDITING";
        AddButtonColor = "#FFFF99";
        AddButtonSize = 60;
    }

    public void AddingButton()
    {
        AddButton = "ADD";
        AddButtonColor = "#FEB1FE";
        AddButtonSize = 40;
    }

    public void NotClauseColor()
    {
        KeywordColor = _HasNotClauseColor;
    }

    public void FontColor()
    {
        KeywordColor = _StandardKeywordColor;
    }


    public bool KeywordExists(string keyword)
    {
         if(_keywordList.Any(k => k.Keyword.ToString() == keyword))
         {
             return true;
         }
         return false;   
    }


}


