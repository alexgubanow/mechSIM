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
        private System.Windows.Media.Effects.BlurEffect _win_Effect;
        public System.Windows.Media.Effects.BlurEffect win_Effect { get => _win_Effect; set => SetProperty(ref _win_Effect, value); }

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
        //private string _adapterT;
        //public string AdapterT{ get => _adapterT; set => SetProperty(ref _adapterT, value); }

        //private string _statusT;
        //public string StatusT { get => _statusT; set => SetProperty(ref _statusT, value); }

        //private string _countsT;
        //public string CountsT { get => _countsT; set => SetProperty(ref _countsT, value); }

        //private string _dutValT;
        //public string DutValT { get => _dutValT; set => SetProperty(ref _dutValT, value); }

        //private string _diffValT;
        //public string DiffValT { get => _diffValT; set => SetProperty(ref _diffValT, value); }

        //private string _refValT;
        //public string RefValT { get => _refValT; set => SetProperty(ref _refValT, value); }

        //private string _conStatText;
        //public string ConStatText { get => _conStatText; set => SetProperty(ref _conStatText, value); }

        ////StartStopButtonText
        //private string _startStopButtonText;
        //public string StartStopButtonText { get => _startStopButtonText; set => SetProperty(ref _startStopButtonText, value); }
        ////ConnectButtonText
        //private string _connectButtonText;
        //public string ConnectButtonText { get => _connectButtonText; set => SetProperty(ref _connectButtonText, value); }

        //private DelegateCommand _connSwitch;
        //public DelegateCommand ConnSwitch => _connSwitch ?? (_connSwitch = new DelegateCommand(() => _ea.GetEvent<BissConnReqEvent>().Publish(true)));

        //private DelegateCommand _startStopMes;
        //public DelegateCommand StartStopMes => _startStopMes ?? (_startStopMes = new DelegateCommand(() => _ea.GetEvent<StartStopMesEvent>().Publish()));

        //private DelegateCommand _resetMes;
        //public DelegateCommand ResetMes => _resetMes ?? (_resetMes = new DelegateCommand(() => _ea.GetEvent<ResetMesEvent>().Publish()));

        //private bool _isEn;

        //public bool IsEn
        //{
        //    get => _isEn;
        //    set => SetProperty(ref _isEn, value);
        //}
    }
}
