using mechLIB;
using Microsoft.Win32;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System.Windows;

namespace spring.ViewModels
{
    public class SelDerivChangedEvent : PubSubEvent<DerivativesEnum> { }
    public class NewComputeEvent : PubSubEvent<string> { }
    public class TopRibbonViewModel : BindableBase
    {
        private readonly IEventAggregator _ea;
        private ModelProperties ModelProperties { get { return (ModelProperties)Application.Current.Properties["ModelProperties"]; } }
        private DerivativesEnum selDeriv;
        public DerivativesEnum SelDeriv { get => selDeriv; set { selDeriv = value; _ea.GetEvent<SelDerivChangedEvent>().Publish(value); } }
        public float MaxU
        {
            get => ModelProperties.MaxU;
            set => SetProperty(ref ModelProperties.MaxU, value);
        }
        public ulong nodes
        {
            get => ModelProperties.nodes;
            set => SetProperty(ref ModelProperties.nodes, value);
        }
        public float E
        {
            get => ModelProperties.E;
            set => SetProperty(ref ModelProperties.E, value);
        }
        public float L
        {
            get => ModelProperties.L;
            set => SetProperty(ref ModelProperties.L, value);
        }
        public float D
        {
            get => ModelProperties.D;
            set => SetProperty(ref ModelProperties.D, value);
        }
        public float ObservationTime
        {
            get => ModelProperties.ObservationTime;
            set => SetProperty(ref ModelProperties.ObservationTime, value);
        }
        public float dt
        {
            get => ModelProperties.dt;
            set => SetProperty(ref ModelProperties.dt, value);
        }
        public float ro
        {
            get => ModelProperties.ro;
            set => SetProperty(ref ModelProperties.ro, value);
        }
        public PhysicalModelEnum PhysicalModel
        {
            get => ModelProperties.PhysicalModel;
            set => SetProperty(ref ModelProperties.PhysicalModel, value);
        }
        public bool NeedToSaveResults
        {
            get => (bool)Application.Current.Properties["NeedToSaveResults"];
            set { bool tmp = false; SetProperty(ref tmp, value); Application.Current.Properties["NeedToSaveResults"] = tmp; }
        }

        private DelegateCommand _ComputeTestCommand;
        public DelegateCommand ComputeTestCommand => _ComputeTestCommand ?? (_ComputeTestCommand = new DelegateCommand(() => _ea.GetEvent<NewComputeEvent>().Publish("")));

        private DelegateCommand _ComputeFileCommand;
        public DelegateCommand ComputeFileCommand => _ComputeFileCommand ?? (_ComputeFileCommand = new DelegateCommand(() =>
        {
            OpenFileDialog openFileDialog = new OpenFileDialog { DereferenceLinks = false };
            if (openFileDialog.ShowDialog() == true)
            { _ea.GetEvent<NewComputeEvent>().Publish(openFileDialog.FileName); }
        }));

        public TopRibbonViewModel(IEventAggregator ea)
        {
            _ea = ea;
            Application.Current.Properties["NeedToSaveResults"] = false;
            Application.Current.Properties["ModelProperties"] = new ModelProperties()
            {
                E = 6E6f,
                L = 25E-3f,
                D = 1E-3f,
                ro = 1040,
                ObservationTime = 0.8f,
                dt = 1E-6f,
                nodes = 9,
                MaxU = 2E-2f
            };
        }
    }
}