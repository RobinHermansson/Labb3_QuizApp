using Labb3_QuizApp.ViewModels;

namespace Labb3_QuizApp.Services;

public interface IDialogService
{
    bool ShowConfirmation(string message, string title = "");
    Task<bool> ShowConfirmationAsync(string message, string title = "");
    Task ShowMessageAsync(string message, string title = "");
    void ShowMessage(string message, string title = "");
    void ShowError(string message, string title = "");

    bool? ShowOptionsDialog(QuestionPackViewModel packToEdit = null);
}
