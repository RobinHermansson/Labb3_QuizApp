using Labb3_QuizApp.Command;
using Labb3_QuizApp.Views;
using System.Windows;
using System.Windows.Threading;

namespace Labb3_QuizApp.ViewModels;

class PlayerViewModel : ViewModelBase
{
    private readonly MainWindowViewModel? _mainWindowViewModel;

    public DelegateCommand SetPackNameCommand { get; }

    public string ButtonColor { get; set; } = "LightGray";
    public DelegateCommand CheckAnswerAndProceedCommand { get; }
    public DelegateCommand PlayGameCommand { get; }
    public QuestionPackViewModel ActivePack { get => _mainWindowViewModel.ActivePack; }
    public DelegateCommand SelectAnswerCommand { get; }

    private int _correctlyAnsweredCount = 0;
    private List<QuestionViewModel> _shuffledQuestions;

    public int CorrectlyAnsweredCount
    {
        get { return _correctlyAnsweredCount; }
        set { _correctlyAnsweredCount = value; }
    }

    private bool _hasClickedAnswer = false;
    public bool HasClickedAnswer
    {
        get { return _hasClickedAnswer; }
        set { _hasClickedAnswer = value; }
    }


    private DispatcherTimer _timer;
    private int _initialTimerValue = 30;
    private int _timerText = 30;

    public int TimerText
    {
        get => _timerText;
        set
        {
            _timerText = value;
            RaisePropertyChanged();
        }
    }


    private string _currentQuestionOutOfTotal = "";
    public string CurrentQuestionOutOfTotal
    {
        get => _currentQuestionOutOfTotal;
        set
        {
            _currentQuestionOutOfTotal = value;
            RaisePropertyChanged();
        }

    }

    public QuestionViewModel _currentQuestion;
    public QuestionViewModel CurrentQuestion
    {
        get => _currentQuestion;
        set
        {
            _currentQuestion = value;
            RaisePropertyChanged();
            CurrentQuestionOutOfTotal = $"Question {QuestionSet + 1} out of {_shuffledQuestions.Count}";
        }
    }
    private int _questionSet = 0;
    public int QuestionSet
    {
        get => _questionSet;
        set
        {
            _questionSet = value;
            RaisePropertyChanged();
        }
    }

    public PlayerViewModel(MainWindowViewModel? mainWindowViewModel)
    {
        _mainWindowViewModel = mainWindowViewModel;
        PlayGameCommand = new DelegateCommand(PlayGame, CanPlayGame);

        SelectAnswerCommand = new DelegateCommand(SelectAnswer);

        _timer = new DispatcherTimer();
        _timer.Interval = TimeSpan.FromSeconds(1.0);
        _timer.Tick += Timer_Tick;


    }

    private void Timer_Tick(object? sender, EventArgs e)
    {
        TimerText -= 1;

        if (TimerText <= 0)
        {
            _timer.Stop();

            foreach (var option in CurrentQuestion.AnswerOptions)
            {
                option.State = option.IsCorrect ? AnswerState.Correct : AnswerState.Incorrect;
            }

            StartDelayedAdvance();
        }
    }
    private void ResetTimer()
    {
        _timer.Stop();

        TimerText = ActivePack.TimeLimitInSeconds;

        _timer.Start();
    }
    private void StartDelayedAdvance()
    {
        var advanceTimer = new DispatcherTimer();
        advanceTimer.Interval = TimeSpan.FromSeconds(2);
        advanceTimer.Tick += (s, e) =>
        {
            advanceTimer.Stop();
            NextQuestion();
        };
        if (!HasClickedAnswer)
        {
            advanceTimer.Start();
        }
    }

    public void PlayGame(object? arg)
    {
        _shuffledQuestions = ActivePack.Questions
            .Select(q => new QuestionViewModel(q))
            .OrderBy(q => Guid.NewGuid())
            .ToList();
        LoadCurrentQuestion();

    }

    private bool CanPlayGame(object? arg)
    {
        return true;
    }


    private void LoadCurrentQuestion()
    {
        if (_shuffledQuestions != null &&
            QuestionSet >= 0 &&
            QuestionSet < _shuffledQuestions.Count)
        {
            CurrentQuestion = _shuffledQuestions[QuestionSet];
            CurrentQuestionOutOfTotal = $"Question {QuestionSet + 1} out of {_shuffledQuestions.Count}";
        }
        ResetTimer();
    }

    private void SelectAnswer(object? answer)
    {
        if (answer == null) return;
        _timer.Stop();

        if (answer is AnswerOptionViewModel asModel && !HasClickedAnswer)
        {
            bool isCorrectAnswer = asModel.IsCorrect;
            if (isCorrectAnswer)
            {
                CorrectlyAnsweredCount += 1;
            }

            foreach (var option in CurrentQuestion.AnswerOptions)
            {
                if (isCorrectAnswer)
                {
                    option.State = option == asModel ? AnswerState.Correct : AnswerState.Neutral;
                }
                else
                {
                    option.State = option.IsCorrect ? AnswerState.Correct : AnswerState.Incorrect;
                }
            }
        }
        StartDelayedAdvance();
        HasClickedAnswer = true;
    }

    private void NextQuestion()
    {
        if (QuestionSet < _shuffledQuestions.Count - 1)
        {
            QuestionSet++;
            HasClickedAnswer = false;
            LoadCurrentQuestion();
        }
        else
        {
            _timer.Stop();
            if (_mainWindowViewModel.CurrentView is PlayerView)
            {
                MessageBox.Show($"Quiz completed! You got {CorrectlyAnsweredCount} correct! out of {_shuffledQuestions.Count}");
            }
            _mainWindowViewModel.SwitchToConfigurationView(this);
            CorrectlyAnsweredCount = 0;
            HasClickedAnswer = false;
            QuestionSet = 0;
        }
    }
}
