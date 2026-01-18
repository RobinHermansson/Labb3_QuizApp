using Labb3_QuizApp.Command;
using Labb3_QuizApp.Models;
using Labb3_QuizApp.Services;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;

namespace Labb3_QuizApp.ViewModels;

class ExternalImportOptionsViewModel : ViewModelBase
{
    private readonly QuestionPackGeneratorAPIService _apiService;
    private List<TriviaCategory> _categories;

    public event EventHandler? CloseRequested;
    public List<TriviaCategory> Categories
    {
        get => _categories;
        set
        {
            _categories = value;
            RaisePropertyChanged();
        }
    }
    
    private TriviaCategory _selectedCategory;
    public TriviaCategory SelectedCategory
    {
        get => _selectedCategory;
        set
        {
            _selectedCategory = value;
            RaisePropertyChanged();
        }
    }
    private bool _isLoading = true;
    public bool IsLoading
    {
        get => _isLoading;
        set
        {
            _isLoading = value;
            RaisePropertyChanged();
            ImportCommand.RaiseCanExecuteChanged();
        }
    }
    public bool DialogResult { get; private set; }
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
    public ExternalImportOptionsViewModel(QuestionPackGeneratorAPIService apiService)
    {
        _apiService = apiService;
        _categories = new List<TriviaCategory>();
        CancelCommand = new DelegateCommand(Cancel);
        ImportCommand = new DelegateCommand(Import, CanImport);
    }
    private void Cancel(object? arg)
    {
        DialogResult = false;
        CloseRequested?.Invoke(this, EventArgs.Empty);
    }

    private void Import(object? arg)
    {
        DialogResult = true;
        CloseRequested?.Invoke(this, EventArgs.Empty);
    }
    private bool CanImport(object? arg)
    {
        return SelectedCategory != null;
    }
    public async Task InitializeAsync()
    {
        IsLoading = true;
        try
        {
            var categories = await _apiService.GetCategoriesAsync();
            if (!categories.Any(c => c.id == 0))
            {
                categories.Insert(0, new TriviaCategory { id = 0, name = "Any Category" });
            }

            Categories = categories;
            SelectedCategory = Categories[0];
        }

        catch (Exception ex)
        {
            Debug.WriteLine($"Error loading categories: {ex.Message}");
        }
        finally
        {
            IsLoading = false;
        }

        }
}
