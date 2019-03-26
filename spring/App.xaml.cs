using System;
using spring.Views;
using Prism.Ioc;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Navigation;
using Prism.Events;

namespace spring
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<ShellView>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {

        }
    }
}
