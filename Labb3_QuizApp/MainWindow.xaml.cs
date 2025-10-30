using Labb3_QuizApp.Models;
using Labb3_QuizApp.ViewModels;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
namespace Labb3_QuizApp;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        var pack = new QuestionPack("MyQuestionPack");
        DataContext = new QuestionPackViewModel(pack);
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        QuestionPackViewModel viewModel = (DataContext as QuestionPackViewModel); 
        viewModel.Name = "New Name";
        viewModel.Questions.Add(new Question("What is 1 + 1?", "2", "3", "4", "5"));
    }
}