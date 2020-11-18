using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System.Windows;

namespace spring.ViewModels
{
    public class StopComputeEvent : PubSubEvent { }
    public class ClosingEvent : PubSubEvent { }
    public class ShellViewModel : BindableBase
    {
        private Visibility _overlay_Visibility;
        public Visibility overlay_Visibility { get => _overlay_Visibility; set => SetProperty(ref _overlay_Visibility, value); }
        private DelegateCommand _ComputeStopCommand;
        public DelegateCommand ComputeStopCommand => _ComputeStopCommand ??
            (_ComputeStopCommand = new DelegateCommand(() => _ea.GetEvent<StopComputeEvent>().Publish()));
        private DelegateCommand _ClosingCMD;
        public DelegateCommand ClosingCMD => _ClosingCMD ??
            (_ClosingCMD = new DelegateCommand(() => _ea.GetEvent<ClosingEvent>().Publish()));
        private string _title;
        public string Title { get => _title; set => SetProperty(ref _title, value); }

        private readonly IEventAggregator _ea;
        public ShellViewModel(IEventAggregator ea)
        {
            _ea = ea;
            overlay_Visibility = Visibility.Collapsed;
            _ea.GetEvent<ComputeIsStartedEvent>().Subscribe(() => overlay_Visibility = Visibility.Visible );
            _ea.GetEvent<GotResultsEvent>().Subscribe(() => overlay_Visibility = Visibility.Collapsed );
            _ea.GetEvent<StopComputeEvent>().Subscribe(() => overlay_Visibility = Visibility.Collapsed );
            Title = "Rope sim";
        }
    }
}
