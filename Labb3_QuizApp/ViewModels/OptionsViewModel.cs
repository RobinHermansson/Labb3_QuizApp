using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labb3_QuizApp.ViewModels;

class OptionsViewModel
{
    private ConfigurationViewModel _configurationViewModel;
    private QuestionPackViewModel ActivePack
    { get => _configurationViewModel.ActivePack; }

    public OptionsViewModel(ConfigurationViewModel configurationViewModel)
    {
        _configurationViewModel = configurationViewModel;
    }
}
