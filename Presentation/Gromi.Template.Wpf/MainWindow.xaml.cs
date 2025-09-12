using Gromi.Template.Wpf.Views;
using System.Windows;

namespace Gromi.Template.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MainFrame.Navigate(new TestView()); // 导航到 TestView
        }
    }
}