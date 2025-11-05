using Labb3_QuizApp.Models;
using Labb3_QuizApp.Services;
using System.Collections.ObjectModel;
using System.Windows;

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

	public QuestionPackGeneratorAPIService QuestionPackGeneratorAPIService { get; }
    public MainWindowViewModel()
    {
		PlayerViewModel = new PlayerViewModel(this);
		ConfigurationViewModel = new ConfigurationViewModel(this);
		QuestionPackGeneratorAPIService = new QuestionPackGeneratorAPIService();

		var pack = new QuestionPack("MyQuestionPack");
		ActivePack = new QuestionPackViewModel(pack);
		ActivePack.Questions.Add(new Question("What is 1 + 1?", "2", "3", "4", "5"));
		ActivePack.Questions.Add(new Question("How though?", "Just because", "Unknown", "FIve.", "Yeah."));
	
    }

}
