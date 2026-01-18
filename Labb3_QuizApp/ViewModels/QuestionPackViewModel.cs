using Labb3_QuizApp.Models;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
namespace Labb3_QuizApp.ViewModels;

public class QuestionPackViewModel : ViewModelBase
{
    private readonly QuestionPack _model;

    public QuestionPackViewModel(QuestionPack model)
    {
        _model = model;
        Questions = new ObservableCollection<Question>(_model.Questions);
        Questions.CollectionChanged += Questions_CollectionChanged;
    }
    public QuestionPackViewModel()
    {

    }

    private void Questions_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.Action == NotifyCollectionChangedAction.Add && e.NewItems != null)
            foreach (Question q in e.NewItems) _model.Questions.Add(q);
        if (e.Action == NotifyCollectionChangedAction.Remove && e.OldItems != null)
            foreach (Question q in e.OldItems) _model.Questions.Remove(q);
        if (e.Action == NotifyCollectionChangedAction.Replace && e.OldItems != null && e.NewItems != null)
            _model.Questions[e.OldStartingIndex] = (Question)e.NewItems[0]!;
        if (e.Action == NotifyCollectionChangedAction.Reset)
            _model.Questions.Clear();
    }

    public Array DifficultyValues
    {
        get
        {
            return Enum.GetValues(typeof(Difficulty));
        }
    }

    private Difficulty _selectedDifficulty;
    public Difficulty SelectedDifficulty
    {
        get => _selectedDifficulty;
        set
        {
            _selectedDifficulty = value;
            _model.Difficulty = value;
            RaisePropertyChanged();
        }
    }


    public string Name
    {
        get => _model.Name;
        set
        {
            _model.Name = value;
            RaisePropertyChanged();
        }
    }
    public Difficulty Difficulty
    {
        get => _model.Difficulty;
        set
        {
            _model.Difficulty = value;
            RaisePropertyChanged();
        }
    }
    public int TimeLimitInSeconds
    {
        get => _model.TimeLimitInSeconds;
        set
        {
            _model.TimeLimitInSeconds = value;
            RaisePropertyChanged();
        }
    }

    public ObservableCollection<Question> Questions { get; set; }
    public QuestionPack GetModel()
    {
        var model = new QuestionPack(Name)
        {
            Difficulty = this.Difficulty,
            TimeLimitInSeconds = this.TimeLimitInSeconds
        };

        foreach (var question in Questions)
        {
            model.Questions.Add(question);
        }

        return model;
    }
}
