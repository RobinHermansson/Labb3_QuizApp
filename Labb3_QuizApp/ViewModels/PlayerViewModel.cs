using Labb3_QuizApp.Command;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
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

    public int CorrectlyAnsweredCount
    {
        get { return _correctlyAnsweredCount; }
        set { _correctlyAnsweredCount = value; }
    }


    public QuestionViewModel CurrentQuestionViewModel;

    private int _questionSet = 0;

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

    private List<QuestionViewModel> _questionViewModels;

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
            CurrentQuestionOutOfTotal = $"Question {QuestionSet+1} out of {ActivePack.Questions.Count}";
        }
    }
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
        
        TimerText = _initialTimerValue;
        
        _timer.Start();
    }
    private void StartDelayedAdvance()
    {
        var advanceTimer = new DispatcherTimer();
        advanceTimer.Interval = TimeSpan.FromSeconds(2);
        advanceTimer.Tick += (s, e) => {
            advanceTimer.Stop();
            NextQuestion();
        };
        advanceTimer.Start();
    }

    public void PlayGame(object? arg)
    {

        LoadCurrentQuestion();

    }

    private bool CanPlayGame(object? arg)
    {
        return true;
    }


    private void LoadCurrentQuestion()
    {
        if (ActivePack?.Questions != null && 
            QuestionSet >= 0 && 
            QuestionSet < ActivePack.Questions.Count)
        {
            CurrentQuestion = new QuestionViewModel(ActivePack.Questions[QuestionSet]);
            CurrentQuestionOutOfTotal = $"Question {QuestionSet+1} out of {ActivePack.Questions.Count}";
        }
        ResetTimer();
    }
    
    private void SelectAnswer(object? answer)
    {
        if (answer == null) return;
        _timer.Stop();

        if (answer is AnswerOptionViewModel asModel)
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
    }

    private void NextQuestion()
    {
        if (QuestionSet < ActivePack.Questions.Count - 1)
        {
            QuestionSet++;
            LoadCurrentQuestion();
        }
        else
        {
            _timer.Stop();
            MessageBox.Show($"Quiz completed! You got {CorrectlyAnsweredCount} correct!");
        }
    }
}
