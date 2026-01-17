using Labb3_QuizApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Labb3_QuizApp.Windows
{
    /// <summary>
    /// Interaction logic for ExternalImportOptionsWIndow.xaml
    /// </summary>
    public partial class ExternalImportOptionsWindow : Window
    {
        public ExternalImportOptionsWindow()
        {
            InitializeComponent();
            Loaded += ExternalImportOptionsWindow_Loaded;
        }
        private async void ExternalImportOptionsWindow_Loaded(object sender, RoutedEventArgs e)
    {
        if (DataContext is ExternalImportOptionsViewModel vm)
        {
            await vm.InitializeAsync();
        }
    }
    }
}
