using Labb3_QuizApp.Command;
using Labb3_QuizApp.Models;
using Labb3_QuizApp.ViewModels;
using System.Windows;

class OptionsWindowViewModel : ViewModelBase
{
    private Window _dialogWindow;
    public bool DialogResult { get; private set; }
    
    // Track if we're editing or creating
    public bool IsEditMode { get; private set; }
    
    // Original pack reference (if in edit mode)
    private QuestionPackViewModel _originalPack;
    
    // Properties
    public string Name { get; set; }
    public Array DifficultyValues => Enum.GetValues(typeof(Difficulty));
    public Difficulty Difficulty { get; set; }
    public int TimeLimitInSeconds { get; set; }
    
    // Dynamic button text
    public string ConfirmButtonText => IsEditMode ? "Save" : "Create";
    
    // Commands
    public DelegateCommand CancelCommand { get; }
    public DelegateCommand ConfirmCommand { get; }
    
    public OptionsWindowViewModel(QuestionPackViewModel packToEdit = null)
    {
        IsEditMode = packToEdit != null;
        _originalPack = packToEdit;
        
        // Initialize with existing pack or defaults
        if (IsEditMode)
        {
            Name = packToEdit.Name;
            Difficulty = packToEdit.Difficulty;
            TimeLimitInSeconds = packToEdit.TimeLimitInSeconds;
        }
        else
        {
            Name = "<New Question Pack>";
            Difficulty = Difficulty.Medium;
            TimeLimitInSeconds = 30;
        }
        
        // Initialize commands
        CancelCommand = new DelegateCommand(_ => Cancel());
        ConfirmCommand = new DelegateCommand(_ => Confirm(), _ => CanConfirm());
    }
    
    public void SetDialogWindow(Window window)
    {
        _dialogWindow = window;
    }
    
    private bool CanConfirm()
    {
        return !string.IsNullOrWhiteSpace(Name);
    }
    
    private void Cancel()
    {
        DialogResult = false;
        _dialogWindow?.Close();
    }
    
    private void Confirm()
    {
        DialogResult = true;
        
        // If in edit mode, update the original pack
        if (IsEditMode && _originalPack != null)
        {
            _originalPack.Name = this.Name;
            _originalPack.Difficulty = this.Difficulty;
            _originalPack.TimeLimitInSeconds = this.TimeLimitInSeconds;
        }
        
        _dialogWindow?.Close();
    }
    
    // For creating new packs
    public QuestionPack CreateQuestionPack()
    {
        return new QuestionPack(Name)
        {
            Difficulty = this.Difficulty,
            TimeLimitInSeconds = this.TimeLimitInSeconds
        };
    }
}