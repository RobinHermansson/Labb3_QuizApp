using Labb3_QuizApp.Command;
using Labb3_QuizApp.Models;
using Labb3_QuizApp.Services;
using Labb3_QuizApp.Views;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace Labb3_QuizApp.ViewModels;

internal class MainWindowViewModel: ViewModelBase
{
    public ObservableCollection<QuestionPackViewModel> Packs { get; } = new();
	private QuestionPackViewModel _activePack;

	public DelegateCommand SwitchToPlayerViewCommand { get; }
	public DelegateCommand SwitchToConfigurationViewCommand { get; }

	private PlayerView _playerView;

	private UserControl _currentView;
	public UserControl CurrentView 
	{ 
		get => _currentView;
		set
		{
			_currentView = value;
			RaisePropertyChanged();
		}
	}

	public PlayerView PlayerView { 
		get => _playerView;
		set
		{
			_playerView = value;
			RaisePropertyChanged();
		}
	}
	private ConfigurationView _configurationView;

	public ConfigurationView ConfigurationView { 
		get => _configurationView;
		set
		{
			_configurationView = value;
			RaisePropertyChanged();
		}
	}

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
		PackHandlerService PackHandler = new PackHandlerService();

		SwitchToConfigurationViewCommand = new DelegateCommand(SwitchToConfigurationView);
		SwitchToPlayerViewCommand = new DelegateCommand(SwitchToPlayerView);

		CurrentView = new ConfigurationView();

		var allPacks = PackHandler.LoadAllPacks();

		ActivePack = new QuestionPackViewModel(allPacks[0]);

		/*var pack = new QuestionPack("MyQuestionPack");
		ActivePack = new QuestionPackViewModel(pack);
		ActivePack.Questions.Add(new Question("What is 1 + 1?", "2", "3", "4", "5"));
		ActivePack.Questions.Add(new Question("How though?", "Just because", "Unknown", "FIve.", "Yeah."));*/
	
    }

	public void SwitchToPlayerView(object? arg)
	{
		CurrentView = new PlayerView();
	}
	public void SwitchToConfigurationView(object? arg)
	{
		CurrentView = new ConfigurationView();
	}

}
