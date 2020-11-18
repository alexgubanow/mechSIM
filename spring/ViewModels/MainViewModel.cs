using HelixToolkit.Wpf;
using mechLIB;
using Microsoft.Win32;
using OxyPlot;
using OxyPlot.Series;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace spring.ViewModels
{
    public class ComputeIsStartedEvent : PubSubEvent { }
    public class GotResultsEvent : PubSubEvent { }
    public class MainViewModel : BindableBase
    {
        readonly System.Windows.Media.Effects.BlurEffect objBlur = new System.Windows.Media.Effects.BlurEffect
        {
            Radius = 4
        };

        private Visibility _overlay_Visibility;
        public Visibility overlay_Visibility { get => _overlay_Visibility; set => SetProperty(ref _overlay_Visibility, value); }
        private System.Windows.Media.Effects.Effect _win_Effect;
        public System.Windows.Media.Effects.Effect win_Effect { get => _win_Effect; set => SetProperty(ref _win_Effect, value); }
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
        private bool _IsConrolsEnabled = true;
        public bool IsConrolsEnabled
        {
            get => _IsConrolsEnabled;
            set => SetProperty(ref _IsConrolsEnabled, value);
        }
        private readonly IEventAggregator _ea;
        private EnviroWrapper world;
        Thread thrsim;
        public MainViewModel(IEventAggregator ea)
        {
            _ea = ea;
            win_Effect = null;
            overlay_Visibility = Visibility.Collapsed;
            _ea.GetEvent<NewComputeEvent>().Subscribe((FileName) => Compute(FileName));
            _ea.GetEvent<ComputeIsStartedEvent>().Subscribe(() => ApplyEffect(true));
            _ea.GetEvent<GotResultsEvent>().Subscribe(() => EnableControls());
            _ea.GetEvent<StopComputeEvent>().Subscribe(() => { ComputeStop(); EnableControls(); });
        }

        private void EnableControls()
        {
            ApplyEffect(false);
            IsConrolsEnabled = true;
        }

        private void Compute(string fileName)
        {
            _ea.GetEvent<ComputeIsStartedEvent>().Publish();
            thrsim = new Thread(delegate ()
            {
                Simulate(fileName);
            });
            thrsim.Start();
        }
        private void ComputeStop()
        {
            thrsim.Abort();
            thrsim = null;
            world.Dispose();
        }

        private void Simulate(string fileName)
        {
            var _ModelProperties = (ModelProperties)Application.Current.Properties["ModelProperties"];
            float[] TimeArr = Array.Empty<float>();
            DerivativesContainerManaged[][] Derivatives = Array.Empty<DerivativesContainerManaged[]>();
            //p = u = v = a = null;
            world = new EnviroWrapper();
            try
            {
                world.CreateWorld(_ModelProperties, fileName);
                world.Run((bool)Application.Current.Properties["NeedToSaveResults"]);
            }
            catch (RuntimeWrappedException e)
            {
                if (e.WrappedException is string s)
                {
                    MessageBox.Show(s);
                }
            }
            try
            {
                int step = 1;
                if (_ModelProperties.Counts > (int)SystemParameters.PrimaryScreenWidth / 2)
                {
                    step = _ModelProperties.Counts / ((int)SystemParameters.PrimaryScreenWidth / 2);
                }
                world.GetTimeArr(step, ref TimeArr);
                world.GetDerivatives(step, ref Derivatives);
                Application.Current.Properties["TimeArr"] = TimeArr;
                Application.Current.Properties["Derivatives"] = Derivatives;
                _ea.GetEvent<GotResultsEvent>().Publish();
            }
            catch (RuntimeWrappedException e)
            {
                if (e.WrappedException is string s)
                {
                    MessageBox.Show(s);
                }
            }
            if (world != null)
            {
                world.Dispose();
            }
        }
    }
}