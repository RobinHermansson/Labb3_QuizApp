using Labb3_QuizApp.ViewModels;
using Labb3_QuizApp.Windows;
using System.Windows;
namespace Labb3_QuizApp.Services;

public class DialogService : IDialogService
{
    public bool? ShowOptionsDialog(QuestionPackViewModel packToEdit = null)
    {
        var viewModel = new OptionsWindowViewModel(packToEdit);
        
        var window = new OptionsWindow
        {
            DataContext = viewModel
        };
        
        var result = window.ShowDialog();
        
        return viewModel.DialogResult ? true : (result == true ? true : false);
    }
    
    // Simple dialogs
    public bool ShowConfirmation(string message, string title = "")
    {
        return MessageBox.Show(message, title, MessageBoxButton.YesNo) == MessageBoxResult.Yes;
    }
    
    public void ShowMessage(string message, string title = "")
    {
        MessageBox.Show(message, title);
    }
    
    public void ShowError(string message, string title = "Error")
    {
        MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Error);
    }

    public Task<bool> ShowConfirmationAsync(string message, string title = "")
    {
        throw new NotImplementedException();
    }

    public Task ShowMessageAsync(string message, string title = "")
    {
        throw new NotImplementedException();
    }
}
