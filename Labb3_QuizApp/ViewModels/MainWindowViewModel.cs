using Labb3_QuizApp.Command;
using Labb3_QuizApp.Models;
using Labb3_QuizApp.Services;
using Labb3_QuizApp.Windows;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;

namespace Labb3_QuizApp.ViewModels;

internal class MainWindowViewModel : ViewModelBase
{
    private readonly IDialogService _dialogService;

    private bool _isFullscreen;
    public bool IsFullscreen
    {
        get => _isFullscreen;
        set
        {
            _isFullscreen = value;
            RaisePropertyChanged();
        }
    }

    public event EventHandler? FullScreenToggleRequested;

    private ViewModelBase _currentViewModel;
    public ViewModelBase CurrentViewModel
    {
        get => _currentViewModel;
        set
        {
            _currentViewModel = value;
            RaisePropertyChanged();
        }
    }
    public ObservableCollection<QuestionPackViewModel> Packs { get; } = new();
    private QuestionPackViewModel _activePack;

    private QuestionPackGeneratorAPIService _importerService;
    private PackHandlerService _packHandlerService;

    public AsyncDelegateCommand ImportExternalQuestionPackCommand { get; }
    public AsyncDelegateCommand OpenExternalImportOptionsCommand { get; }

    public DelegateCommand SwitchToPlayerViewCommand { get; }
    public DelegateCommand SwitchToConfigurationViewCommand { get; }
    public DelegateCommand CreateNewPackCommand { get; }

    public DelegateCommand DeleteActivePackCommand { get; }
    public DelegateCommand FullScreenToggleCommand { get; }

    public DelegateCommand ExitGameCommand { get; }
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
        _dialogService = new DialogService();
        PlayerViewModel = new PlayerViewModel(this, _dialogService);
        ConfigurationViewModel = new ConfigurationViewModel(this);
        QuestionPackGeneratorAPIService = new QuestionPackGeneratorAPIService();
        CreateNewPackCommand = new DelegateCommand(CreateNewPack);

        _packHandlerService = new PackHandlerService();

        _importerService = new QuestionPackGeneratorAPIService();
        ImportExternalQuestionPackCommand = new AsyncDelegateCommand(ImportExternalQuestionPack);
        OpenExternalImportOptionsCommand = new AsyncDelegateCommand(OpenExternalImportOptions);

        SwitchToConfigurationViewCommand = new DelegateCommand(SwitchToConfigurationView, CanSwitchToConfigurationView);
        SwitchToPlayerViewCommand = new DelegateCommand(SwitchToPlayerView, CanSwitchToPlayerView);

        SelectNewActivePackCommand = new DelegateCommand(SelectNewActivePack);
        DeleteActivePackCommand = new DelegateCommand(DeleteActivePack);

        FullScreenToggleCommand = new DelegateCommand(FullScreenToggle);

        ExitGameCommand = new DelegateCommand(ExitGame);


        CurrentViewModel = ConfigurationViewModel;

        var allPacks = _packHandlerService.LoadAllPacks();
        allPacks.ForEach(p => Packs.Add(new QuestionPackViewModel(p)));
        if (allPacks.Count == 0)
        {
            var question = new Question("What is the meaning of life?", "42", "Living.", "Life of Brian.", "It is 42, so pick that.");
            ActivePack = new QuestionPackViewModel(new QuestionPack("Starter pack", Difficulty.Medium));
            ActivePack.Questions.Add(question);
        }
        else
        {
            ActivePack = Packs[0];
        }

    }

    public void SwitchToPlayerView(object? arg)
    {
        CurrentViewModel = PlayerViewModel;
        PlayerViewModel.PlayGame(arg);
        SwitchToConfigurationViewCommand.RaiseCanExecuteChanged();
    }

    public bool CanSwitchToPlayerView(object? arg)
    {
        return !(CurrentViewModel is PlayerViewModel) && ActivePack?.Questions.Count > 0;
    }
    public void SwitchToConfigurationView(object? arg)
    {
        CurrentViewModel = ConfigurationViewModel;
    }

    public bool CanSwitchToConfigurationView(object? arg)
    {
        return !(CurrentViewModel is ConfigurationViewModel);
    }

    public void CreateNewPack(object? arg)
    {
        var viewModel = new OptionsWindowViewModel();
        ShowOptionsWindow(viewModel);
        if (viewModel.DialogResult)
        {
            var newPack = viewModel.CreateQuestionPack();
            var newPackViewModel = new QuestionPackViewModel(newPack);
            Packs.Add(newPackViewModel);
            ActivePack = newPackViewModel;
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
        bool? saved = ShowOptionsWindow(viewModel);
        if (saved != null && saved is true)
        {
            _dialogService.ShowMessage("Saved pack.", "Saved.");
        }
    }

    private bool? ShowOptionsWindow(OptionsWindowViewModel viewModel)
    {
        var window = new OptionsWindow { DataContext = viewModel };
        viewModel.CloseRequested += (s, e) => window.Close();
        window.ShowDialog();
        return viewModel.DialogResult ? true : false;
    }
    public async Task ImportExternalQuestionPack(object? arg)
    {

        try
        {
            var pack = await _importerService.GetQuestionPackAsync();
            if (pack != null)
            {
                var packViewModel = new QuestionPackViewModel(pack);
                Packs.Add(packViewModel);
                ActivePack = packViewModel;
            }
        }
        catch (Exception ex)
        {
            _dialogService.ShowError($"Error importing question pack.", "ERROR");
            Debug.WriteLine($"Error importing question pack: {ex.Message}");
        }

    }
    public async Task OpenExternalImportOptions(object? arg)
    {
        var viewModel = new ExternalImportOptionsViewModel(_importerService);
        var window = new ExternalImportOptionsWindow
        {
            DataContext = viewModel
        };

        viewModel.CloseRequested += (s, e) => window.Close();

        await viewModel.InitializeAsync();
        window.ShowDialog();

        if (viewModel.DialogResult)
        {
            try
            {
                string categoryParam = viewModel.SelectedCategory?.id == 0 ?
                                  "" : viewModel.SelectedCategory?.id.ToString();

                var pack = await _importerService.GetQuestionPackAsync(
                    viewModel.NumberOfQuestions,
                    categoryParam,
                    viewModel.SelectedDifficulty);

                if (pack != null)
                {
                    var packViewModel = new QuestionPackViewModel(pack);
                    Packs.Add(packViewModel);
                    ActivePack = packViewModel;
                }
                _dialogService.ShowMessage("Successfully imported question pack", "Success!");
            }
            catch (Exception ex)
            {
                _dialogService.ShowError("Error importing pack.", "ERROR");
                Debug.WriteLine($"Error importing pack: {ex.Message}");
            }
        }
    }

    public void FullScreenToggle(object? arg)
    {
        
        IsFullscreen = !IsFullscreen;

        FullScreenToggleRequested?.Invoke(this, EventArgs.Empty);
    }


    public void ExitGame(object? arg)
    {
        var packs = Packs.Select(p => p.GetModel()).ToList();
        _packHandlerService.SaveAllPacks(packs);

        Application.Current.Shutdown();
    }
}
