using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System.Windows;

namespace spring.ViewModels
{
    public class ShellViewModel : BindableBase
    {
        readonly System.Windows.Media.Effects.BlurEffect objBlur = new System.Windows.Media.Effects.BlurEffect
        {
            Radius = 4
        };

        private Visibility _overlay_Visibility;
        public Visibility overlay_Visibility { get => _overlay_Visibility; set => SetProperty(ref _overlay_Visibility, value); }
        private System.Windows.Media.Effects.Effect _win_Effect;
        public System.Windows.Media.Effects.Effect win_Effect { get => _win_Effect; set => SetProperty(ref _win_Effect, value); }

        private IEventAggregator _ea;
        public ShellViewModel(IEventAggregator ea)
        {
            _ea = ea;
            Title = "Rope sim";
            win_Effect = null;
            overlay_Visibility = Visibility.Collapsed;
            _ea.GetEvent<ComputeEvent>().Subscribe((var) => ApplyEffect(var));
            _ea.GetEvent<GotResultsEvent>().Subscribe(() => ApplyEffect(false));
        }
        private DelegateCommand _ComputeStopCommand;
        public DelegateCommand ComputeStopCommand => _ComputeStopCommand ?? (_ComputeStopCommand = new DelegateCommand(() =>
        _ea.GetEvent<ComputeEvent>().Publish(false)));
        private string _title;
        public string Title { get => _title; set => SetProperty(ref _title, value); }
        private void ApplyEffect(bool IsRuninng)
        {
            if (IsRuninng)
            {
                overlay_Visibility = Visibility.Visible;
                win_Effect = objBlur;
            }
            else
            {
                win_Effect = null;
                overlay_Visibility = Visibility.Collapsed;
            }
        }
    }
}
