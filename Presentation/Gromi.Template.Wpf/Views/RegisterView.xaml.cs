using Gromi.Template.Wpf.ViewModels;
using System.Windows.Controls;

namespace Gromi.Template.Wpf.Views
{
    /// <summary>
    /// RegisterView.xaml 的交互逻辑
    /// </summary>
    public partial class RegisterView : Page
    {
        public RegisterView()
        {
            InitializeComponent();
            DataContext = new RegisterViewModel();
        }
    }
}