namespace Labb3_QuizApp.ViewModels;

class AnswerOptionViewModel : ViewModelBase
{
    private string _text;
    public string Text
    {
        get => _text;
        set { _text = value; RaisePropertyChanged(); }
    }

    public bool IsCorrect { get; set; }

    private AnswerState _state = AnswerState.Neutral;
    public AnswerState State
    {
        get => _state;
        set { _state = value; RaisePropertyChanged(nameof(State)); }
    }
}
public enum AnswerState
{
    Neutral,
    Correct,
    Incorrect
}
