using Labb3_QuizApp.Command;
using Labb3_QuizApp.Models;
using Labb3_QuizApp.ViewModels;
using System.Windows;

class OptionsWindowViewModel : ViewModelBase
{
    private Window _dialogWindow;
    public bool DialogResult { get; private set; }

    public bool IsEditMode { get; private set; }

    private QuestionPackViewModel _originalPack;

    public string Name { get; set; }
    public Array DifficultyValues => Enum.GetValues(typeof(Difficulty));
    public Difficulty Difficulty { get; set; }
    public int TimeLimitInSeconds { get; set; }

    public string ConfirmButtonText => IsEditMode ? "Save" : "Create";

    public DelegateCommand CancelCommand { get; }
    public DelegateCommand ConfirmCommand { get; }

    public OptionsWindowViewModel(QuestionPackViewModel packToEdit = null)
    {
        IsEditMode = packToEdit != null;
        _originalPack = packToEdit;

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

        if (IsEditMode && _originalPack != null)
        {
            _originalPack.Name = this.Name;
            _originalPack.Difficulty = this.Difficulty;
            _originalPack.TimeLimitInSeconds = this.TimeLimitInSeconds;
        }

        _dialogWindow?.Close();
    }

    public QuestionPack CreateQuestionPack()
    {
        return new QuestionPack(Name)
        {
            Difficulty = this.Difficulty,
            TimeLimitInSeconds = this.TimeLimitInSeconds
        };
    }
}