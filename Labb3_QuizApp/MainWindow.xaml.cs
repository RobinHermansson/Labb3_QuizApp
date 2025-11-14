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
        if (DataContext is MainWindowViewModel viewModel)
        {
            viewModel.SetMainWindow(this);
        }

    }

}