using Labb3_QuizApp.Services;
using Labb3_QuizApp.ViewModels;
using System.Windows;
namespace Labb3_QuizApp;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = new MainWindowViewModel();
        Closing += MainWindow_Closing;
        if (DataContext is MainWindowViewModel viewModel)
        {
            viewModel.SetMainWindow(this);
        }

    }
    private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
        if (DataContext is MainWindowViewModel viewModel)
        {
            try
            {
                var packs = viewModel.Packs.Select(p => p.GetModel()).ToList();
                var packHandler = new PackHandlerService();
                packHandler.SaveAllPacks(packs);
                
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error saving packs on exit: {ex.Message}");
            }
        }
    }

}