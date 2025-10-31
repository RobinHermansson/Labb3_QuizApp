using Labb3_QuizApp.Models;
using System.Collections.ObjectModel;

namespace Labb3_QuizApp.ViewModels;

internal class MainWindowViewModel: ViewModelBase
{
    public ObservableCollection<QuestionPackViewModel> Packs { get; } = new();
	private QuestionPackViewModel _activePack;

	public QuestionPackViewModel ActivePack
	{
		get => _activePack;
		set {
			_activePack = value;
			RaisePropertyChanged();
			PlayerViewModel.RaisePropertyChanged(nameof(PlayerViewModel.ActivePack));
		}
	}

    public PlayerViewModel PlayerViewModel { get; }
	public ConfigurationViewModel ConfigurationViewModel { get; }
    public MainWindowViewModel()
    {
		PlayerViewModel = new PlayerViewModel(this);
		ConfigurationViewModel = new ConfigurationViewModel(this);

		var pack = new QuestionPack("MyQuestionPack");
		ActivePack = new QuestionPackViewModel(pack);
		ActivePack.Questions.Add(new Question("What is 1 + 1?", "2", "3", "4", "5"));
		ActivePack.Questions.Add(new Question("How though?", "Just because", "Unknown", "FIve.", "Yeah."));

    }

}
