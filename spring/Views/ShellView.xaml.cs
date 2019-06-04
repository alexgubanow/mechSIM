using System;
using MahApps.Metro.Controls;
using Prism.Ioc;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Navigation;
using Prism.Events;
using Prism.Regions;

namespace spring.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class ShellView
    {
        private readonly IContainerExtension _container;
        private readonly IRegionManager _regionManager;

        private readonly IEventAggregator _ea;

        public ShellView(IContainerExtension container, IRegionManager regionManager, IEventAggregator ea)
        {
            InitializeComponent();
            _container = container;
            _regionManager = regionManager;
            _ea = ea;
        }

        private void ShellView_OnLoaded(object sender, RoutedEventArgs e)
        {
            _regionManager.Regions["MainViewRegion"].Add(_container.Resolve<MainView>());
            //propsViewRegion
            _regionManager.Regions["propsViewRegion"].Add(_container.Resolve<propsView>());
            //_regionManager.Regions["plotsViewRegion"].Add(_container.Resolve<plotsView>());
            _regionManager.Regions["ForcePlotViewRegion"].Add(_container.Resolve<ForcePlotView>());
            _regionManager.Regions["DisplPlotViewRegion"].Add(_container.Resolve<DisplPlotView>());
            _regionManager.Regions["AccelPlotViewRegion"].Add(_container.Resolve<AccelPlotView>());
            _regionManager.Regions["VelPlotViewRegion"].Add(_container.Resolve<VelPlotView>());
            _regionManager.Regions["CoordPlotViewRegion"].Add(_container.Resolve<CoordPlotView>());
        }
    }
}
