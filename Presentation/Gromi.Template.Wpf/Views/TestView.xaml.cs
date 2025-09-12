using Gromi.Template.Wpf.ViewModels;
using System.Windows.Controls;

namespace Gromi.Template.Wpf.Views
{
    /// <summary>
    /// TestView.xaml 的交互逻辑
    /// </summary>
    public partial class TestView : Page
    {
        public TestView()
        {
            InitializeComponent();
            DataContext = new TestViewModel();
        }
    }
}