using Gromi.Template.Wpf.Configurations;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace Gromi.Template.Wpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddAutoInjectConfiguration();
        }
    }
}