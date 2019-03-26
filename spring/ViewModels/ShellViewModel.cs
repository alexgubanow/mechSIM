using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;

namespace spring.ViewModels
{
    public class ShellViewModel : BindableBase
    {
        private IEventAggregator _ea;
        public ShellViewModel(IEventAggregator ea)
        {
            _ea = ea;
            Title = "Spring sim";
            //MeasuringStartedEvent
            //StartStopButtonText = "Start";
            //_ea.GetEvent<MeasuringStartedEvent>().Subscribe((status) => StartStopButtonText = status ? "Stop" : "Start");
            //_ea.GetEvent<BissConnectionEvent>().Subscribe((status) => ConStatText = status ? "Connected" : "Disconnected");
            //_ea.GetEvent<AdapterChangedEvent>().Subscribe((val) => AdapterT= val);
            //_ea.GetEvent<StatusChangedEvent>().Subscribe((val) => StatusT= val);
            //_ea.GetEvent<CountsChangedEvent>().Subscribe((val) => CountsT= val.ToString());
            //_ea.GetEvent<DutValChangedEvent>().Subscribe((val) => DutValT = val.ToString());
            //_ea.GetEvent<RefValChangedEvent>().Subscribe((val) => RefValT = val.ToString());
            //_ea.GetEvent<DiffValChangedEvent>().Subscribe((val) => DiffValT = val.DiffVal.ToString());
            //ConnectButtonText = "Connect";
            //_ea.GetEvent<BissConnectionEvent>().Subscribe((status) => ConnectButtonText = status ? "Disconnect" : "Connect");
            //_ea.GetEvent<BissConnectionEvent>().Subscribe((status) => IsEn = status);
            //Usb = new BissFunc(ea);
            //_ea.GetEvent<BissConnReqEvent>().Publish(false);
        }

        private string _title;
        public string Title { get => _title; set => SetProperty(ref _title, value); }

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
