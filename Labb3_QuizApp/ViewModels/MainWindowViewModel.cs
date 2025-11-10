using Labb3_QuizApp.Command;
using Labb3_QuizApp.Models;
using Labb3_QuizApp.Services;
using Labb3_QuizApp.Views;
using Labb3_QuizApp.Windows;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace Labb3_QuizApp.ViewModels;

internal class MainWindowViewModel : ViewModelBase
{
    public ObservableCollection<QuestionPackViewModel> Packs { get; } = new();
    private QuestionPackViewModel _activePack;

    public DelegateCommand SwitchToPlayerViewCommand { get; }
    public DelegateCommand SwitchToConfigurationViewCommand { get; }
    public DelegateCommand CreateNewPackCommand { get; }
    
    public DelegateCommand DeleteActivePackCommand { get; }
    private QuestionPackViewModel _selectedPack;
    public QuestionPackViewModel SelectedPack
    {
        get => _selectedPack;
        set
        {
            _selectedPack = value;
            RaisePropertyChanged();
        }
    }
    public DelegateCommand SelectNewActivePackCommand { get; }


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

    public PlayerView PlayerView
    {
        get => _playerView;
        set
        {
            _playerView = value;
            RaisePropertyChanged();
        }
    }
    private ConfigurationView _configurationView;

    public ConfigurationView ConfigurationView
    {
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
        set
        {
            _activePack = value;
            RaisePropertyChanged();
            PlayerViewModel.RaisePropertyChanged(nameof(PlayerViewModel.ActivePack));
            ConfigurationViewModel.RaisePropertyChanged(nameof(ConfigurationViewModel.ActivePack));
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
        CreateNewPackCommand = new DelegateCommand(CreateNewPack);

        PackHandlerService PackHandler = new PackHandlerService();

        SwitchToConfigurationViewCommand = new DelegateCommand(SwitchToConfigurationView);
        SwitchToPlayerViewCommand = new DelegateCommand(SwitchToPlayerView);

        SelectNewActivePackCommand = new DelegateCommand(SelectNewActivePack);
        DeleteActivePackCommand = new DelegateCommand(DeleteActivePack);

        CurrentView = new ConfigurationView();

        var allPacks = PackHandler.LoadAllPacks();
        allPacks.ForEach(p => Packs.Add(new QuestionPackViewModel(p)));
        ActivePack = Packs[0];

    }

    public void SwitchToPlayerView(object? arg)
    {
        CurrentView = new PlayerView();
        PlayerViewModel.PlayGame(arg);
    }
    public void SwitchToConfigurationView(object? arg)
    {
        CurrentView = new ConfigurationView();
    }
    public void CreateNewPack(object? arg)
    {
        var viewModel = new OptionsWindowViewModel();
        ShowOptionsDialog(viewModel);
        if (viewModel.DialogResult)
        {
            var newPack = viewModel.CreateQuestionPack();
            var newPackViewModel = new QuestionPackViewModel(newPack);
            Packs.Add(newPackViewModel);
        }
    }

    public void SelectNewActivePack(object? arg)
    {
        ActivePack = (QuestionPackViewModel)arg;
    }
    public void DeleteActivePack(object? arg)
    {
        Packs.Remove(ActivePack);
        if (Packs.Count != 0)
        {
            ActivePack = Packs[0];
        }
        else
        {
            ActivePack = new QuestionPackViewModel(new QuestionPack("NewDefaultPack"));
        }
    }
    public void OpenActivePackOptions(object? arg)
    {
        
        if (ActivePack == null) return;
    
        var viewModel = new OptionsWindowViewModel(ActivePack);
        ShowOptionsDialog(viewModel);
    }

    public void ShowOptionsDialog(OptionsWindowViewModel viewModel)
    {
        var optionsWindow = new OptionsWindow
        {
            DataContext = viewModel
        };
        
        viewModel.SetDialogWindow(optionsWindow);
        optionsWindow.ShowDialog();
    }

}
