using Labb3_QuizApp.Command;
using Labb3_QuizApp.Models;
using Labb3_QuizApp.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Labb3_QuizApp.ViewModels;

class ExternalImportOptionsViewModel: ViewModelBase
{
    private Window _dialogWindow;
    private ObservableCollection<QuestionPackViewModel> _originalPackList;
    private TriviaApiRequestGenerator _apiRequestGenerator = new TriviaApiRequestGenerator();
    public List<string> Categories => _apiRequestGenerator.CategoryDict.Keys.ToList();
    public bool DialogResult { get; private set; }
    public string SelectedCategory { get; set; } = "Any Category";
    public Array DifficultyValues => Enum.GetValues(typeof(Difficulty));
    public Difficulty SelectedDifficulty { get; set; } = Difficulty.Medium;
    
    private int _numberOfQuestions = 10;
    public int NumberOfQuestions
    {
        get => _numberOfQuestions;
        set
        {
            _numberOfQuestions = value;
            RaisePropertyChanged();
        }
    }
    
    public DelegateCommand CancelCommand { get; }
    public DelegateCommand ImportCommand { get; }
    public ExternalImportOptionsViewModel(ObservableCollection<QuestionPackViewModel> originalPackList)
    {
        _originalPackList = originalPackList;
        CancelCommand = new DelegateCommand(Cancel);
        ImportCommand = new DelegateCommand(Import);
    }
    public void SetDialogWindow(Window window)
    {
        _dialogWindow = window;
    }

     private void Cancel(object? arg)
    {
        DialogResult = false;
        _dialogWindow?.Close();
    }
    
    private void Import(object? arg)
    {
        DialogResult = true;
        _dialogWindow?.Close();
    }
}
