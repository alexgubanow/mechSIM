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
    public class CurrTChangedEvent : PubSubEvent<int> { }

    public class SelDerivChangedEvent : PubSubEvent<int> { }

    public class GotResultsEvent : PubSubEvent { }

    public class Load3dEvent : PubSubEvent { }

    public class MainViewModel : BindableBase
    {

        private readonly IEventAggregator _ea;
        private ModelProperties _ModelProperties = new ModelProperties()
        {
            E = 6E6f,
            L = 25E-3f,
            D = 1E-3f,
            ro = 1040,
            Counts = 100000,
            dt = 1E-6f,
            nodes = 9,
            initDrop = 1E-08f,
            MaxU = 2E-2f,
            DampRatio = 1
        };
        private DerivativesContainerManaged[][] Derivatives;
        private float[] timeArr;
        private EnviroWrapper world;

        private int _CurrT;
        public int CurrT { get => _CurrT; set { _CurrT = value; Draw3d(value, SelDeriv); } }

        private DerivativesEnum selDeriv;
        public DerivativesEnum SelDeriv { get => selDeriv; set { selDeriv = value; if (IsArrayExist) { DrawPoints(value); } } }

        public Point3DCollection RopeCoords { get; set; }
        public ObservableCollection<Visual3D> Objs3d { get; set; }

        public PlotModel awePlotModelX { get; set; }
        public PlotModel awePlotModelY { get; set; }
        public PlotModel awePlotModelZ { get; set; }
        public float DampRatio
        {
            get => _ModelProperties.DampRatio;
            set => SetProperty(ref _ModelProperties.DampRatio, value);
        }
        public float MaxU
        {
            get => _ModelProperties.MaxU;
            set => SetProperty(ref _ModelProperties.MaxU, value);
        }
        public float initDrop
        {
            get => _ModelProperties.initDrop;
            set => SetProperty(ref _ModelProperties.initDrop, value);
        }
        public ulong nodes
        {
            get => _ModelProperties.nodes;
            set => SetProperty(ref _ModelProperties.nodes, value);
        }
        public float E
        {
            get => _ModelProperties.E;
            set => SetProperty(ref _ModelProperties.E, value);
        }
        public float L
        {
            get => _ModelProperties.L;
            set => SetProperty(ref _ModelProperties.L, value);
        }
        public float D
        {
            get => _ModelProperties.D;
            set => SetProperty(ref _ModelProperties.D, value);
        }
        public int Counts
        {
            get => _ModelProperties.Counts;
            set => SetProperty(ref _ModelProperties.Counts, value);
        }
        public float dt
        {
            get => _ModelProperties.dt;
            set => SetProperty(ref _ModelProperties.dt, value);
        }
        public float ro
        {
            get => _ModelProperties.ro;
            set => SetProperty(ref _ModelProperties.ro, value);
        }
        public PhysicalModelEnum PhysicalModel
        {
            get => _ModelProperties.PhysicalModel;
            set => SetProperty(ref _ModelProperties.PhysicalModel, value);
        }
        private bool _EnableConrols;
        public bool EnableConrols
        {
            get => !_isRunning;
            set => SetProperty(ref _EnableConrols, value);
        }
        private int _EndT;
        public int EndT
        {
            get => _EndT;
            set => SetProperty(ref _EndT, value);
        }

        private bool _NeedToSaveResults;
        public bool NeedToSaveResults
        {
            get => _NeedToSaveResults;
            set => SetProperty(ref _NeedToSaveResults, value);
        }

        private bool _isRunning;
        public bool IsRunning
        {
            get => _isRunning;
            set => SetProperty(ref _isRunning, value);
        }

        private DelegateCommand _ComputeTestCommand;
        public DelegateCommand ComputeTestCommand => _ComputeTestCommand ?? (_ComputeTestCommand = new DelegateCommand(() => Compute("")));

        private DelegateCommand _ComputeFileCommand;
        public DelegateCommand ComputeFileCommand => _ComputeFileCommand ?? (_ComputeFileCommand = new DelegateCommand(() =>
        {
            OpenFileDialog openFileDialog = new OpenFileDialog { DereferenceLinks = false };
            if (openFileDialog.ShowDialog() == true)
            { Compute(openFileDialog.FileName); }
        }));

        Thread thrsim;
        public MainViewModel(IEventAggregator ea)
        {
            _ea = ea;
            IsRunning = false;
            _CurrT = 0;
            EndT = 1;
            Objs3d = new ObservableCollection<Visual3D>();
            awePlotModelX = new PlotModel { Title = "X axis" };
            awePlotModelY = new PlotModel { Title = "Y axis" };
            awePlotModelZ = new PlotModel { Title = "Z axis" };
            awePlotModelX.InvalidatePlot(true);
            awePlotModelY.InvalidatePlot(true);
            awePlotModelZ.InvalidatePlot(true);
            EnableConrols = false;
            _ea.GetEvent<GotResultsEvent>().Subscribe(() => IsRunning = false);
            _ea.GetEvent<GotResultsEvent>().Subscribe(() => EnableConrols = true);
            _ea.GetEvent<GotResultsEvent>().Subscribe(() => thrsim = null);
            _ea.GetEvent<GotResultsEvent>().Subscribe(() => ShowResults(SelDeriv));
            _ea.GetEvent<ComputeEvent>().Subscribe((var) => IsRunning = var);
            _ea.GetEvent<ComputeEvent>().Subscribe((var) => EnableConrols = !var);
            _ea.GetEvent<ComputeEvent>().Subscribe((var) => { if (!var) { ComputeStop(); } });
        }

        private void Compute(string fileName)
        {
            if (!IsRunning)
            {
                _ea.GetEvent<ComputeEvent>().Publish(true);
                ClearDataView();
                thrsim = new Thread(delegate ()
                {
                    Simulate(fileName);
                });
                thrsim.Start();
            }
            else
            {
                MessageBox.Show("Wait till end of current calculation or STOP it");
            }
        }
        private void ComputeStop()
        {
            if (IsRunning && thrsim != null)
            {
                thrsim.Abort();
                thrsim = null;
                world.Dispose();
            }
        }

        private void Simulate(string fileName)
        {
            EndT = 1;
            Derivatives = null;
            //p = u = v = a = null;
            world = new EnviroWrapper();
            try
            {
                world.CreateWorld(_ModelProperties, fileName);
                world.Run(NeedToSaveResults);
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
                timeArr = Array.Empty<float>();
                Derivatives = Array.Empty<DerivativesContainerManaged[]>();
                int step = 1;
                if (_ModelProperties.Counts > (int)SystemParameters.PrimaryScreenWidth / 2)
                {
                    step = _ModelProperties.Counts / ((int)SystemParameters.PrimaryScreenWidth / 2);
                }
                world.GetTimeArr(step, ref timeArr);
                world.GetDerivatives(step, ref Derivatives);
                EndT = timeArr.Length - 1;
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
        private void ClearDataView()
        {
            CurrT = 0;
            awePlotModelX.Series.Clear();
            awePlotModelY.Series.Clear();
            awePlotModelZ.Series.Clear();
        }

        private void Load3d()
        {
            Objs3d.Add(new DefaultLights());
        }

        private void ShowResults(DerivativesEnum Deriv)
        {
            if (IsArrayExist)
            {
                DrawPoints(Deriv);
                Draw3d(CurrT, Deriv);
            }
        }

        private bool IsArrayExist => Derivatives != null;

        private void DrawPoints(DerivativesEnum Deriv)
        {
            ClearDataView();
            for (int n = 0; n < Derivatives.Length; n++)
            {
                DataPoint[] XdataPoints = new DataPoint[Derivatives[n].Length];
                DataPoint[] YdataPoints = new DataPoint[Derivatives[n].Length];
                DataPoint[] ZdataPoints = new DataPoint[Derivatives[n].Length];
                switch (Deriv)
                {
                    case DerivativesEnum.f:
                        Parallel.For(0, Derivatives[n].Length,
                            t =>
                            {
                                XdataPoints[t] = new DataPoint(timeArr[t], Derivatives[n][t].f.x);
                                YdataPoints[t] = new DataPoint(timeArr[t], Derivatives[n][t].f.y);
                                ZdataPoints[t] = new DataPoint(timeArr[t], Derivatives[n][t].f.z);
                            });
                        break;
                    case DerivativesEnum.p:
                        Parallel.For(0, Derivatives[n].Length,
                            t =>
                            {
                                XdataPoints[t] = new DataPoint(timeArr[t], Derivatives[n][t].p.x);
                                YdataPoints[t] = new DataPoint(timeArr[t], Derivatives[n][t].p.y);
                                ZdataPoints[t] = new DataPoint(timeArr[t], Derivatives[n][t].p.z);
                            });
                        break;
                    case DerivativesEnum.u:
                        Parallel.For(0, Derivatives[n].Length,
                            t =>
                            {
                                XdataPoints[t] = new DataPoint(timeArr[t], Derivatives[n][t].u.x);
                                YdataPoints[t] = new DataPoint(timeArr[t], Derivatives[n][t].u.y);
                                ZdataPoints[t] = new DataPoint(timeArr[t], Derivatives[n][t].u.z);
                            });
                        break;
                    case DerivativesEnum.v:
                        Parallel.For(0, Derivatives[n].Length,
                            t =>
                            {
                                XdataPoints[t] = new DataPoint(timeArr[t], Derivatives[n][t].v.x);
                                YdataPoints[t] = new DataPoint(timeArr[t], Derivatives[n][t].v.y);
                                ZdataPoints[t] = new DataPoint(timeArr[t], Derivatives[n][t].v.z);
                            });
                        break;
                    case DerivativesEnum.a:
                        Parallel.For(0, Derivatives[n].Length,
                            t =>
                            {
                                XdataPoints[t] = new DataPoint(timeArr[t], Derivatives[n][t].a.x);
                                YdataPoints[t] = new DataPoint(timeArr[t], Derivatives[n][t].a.y);
                                ZdataPoints[t] = new DataPoint(timeArr[t], Derivatives[n][t].a.z);
                            });
                        break;
                }
                LineSeries aweLineSeriesX = new LineSeries { Title = "node #" + n };
                aweLineSeriesX.Points.AddRange(XdataPoints);
                awePlotModelX.Series.Add(aweLineSeriesX);
                LineSeries aweLineSeriesY = new LineSeries { Title = "node #" + n };
                aweLineSeriesY.Points.AddRange(YdataPoints);
                awePlotModelY.Series.Add(aweLineSeriesY);
                LineSeries aweLineSeriesZ = new LineSeries { Title = "node #" + n };
                aweLineSeriesZ.Points.AddRange(ZdataPoints);
                awePlotModelZ.Series.Add(aweLineSeriesZ);
            }
            awePlotModelX.InvalidatePlot(true);
            awePlotModelY.InvalidatePlot(true);
            awePlotModelZ.InvalidatePlot(true);
        }
        private void Draw3d(int t, DerivativesEnum Deriv)
        {
            if (Derivatives != null && Derivatives.Length > 0)
            {
                Application.Current.Dispatcher.Invoke(delegate
                {
                    Objs3d.Clear();
                    Load3d();
                    Objs3d.Add(new CubeVisual3D
                    {
                        Center = new Point3D(Derivatives[0][t].p.x * 10E2,
                                             Derivatives[0][t].p.z * 10E2,
                                             Derivatives[0][t].p.y * 10E2),
                        SideLength = .8,
                        Fill = Brushes.Gray
                    });
                    Objs3d.Add(new CubeVisual3D
                    {
                        Center = new Point3D(Derivatives[Derivatives.Length - 1][t].p.x * 10E2,
                                             Derivatives[Derivatives.Length - 1][t].p.z * 10E2,
                                             Derivatives[Derivatives.Length - 1][t].p.y * 10E2),
                        SideLength = .8,
                        Fill = Brushes.Gray
                    });
                    for (int node = 0; node < Derivatives.Length - 1; node++)
                    {
                        Objs3d.Add(new LinesVisual3D
                        {
                            Points = { new Point3D(Derivatives[node][t].p.x * 10E2,
                                                Derivatives[node][t].p.z * 10E2,
                                                Derivatives[node][t].p.y * 10E2),
                            new Point3D(Derivatives[node + 1][t].p.x * 10E2,
                                        Derivatives[node + 1][t].p.z * 10E2,
                                        Derivatives[node + 1][t].p.y * 10E2) },
                            Thickness = 2,
                            Color = Brushes.Blue.Color
                        });
                        Objs3d.Add(new SphereVisual3D
                        {
                            Center = new Point3D(Derivatives[node][t].p.x * 10E2,
                                                Derivatives[node][t].p.z * 10E2,
                                                Derivatives[node][t].p.y * 10E2),
                            Radius = .3,
                            Fill = Brushes.Black
                        });
                    }
                });
            }
        }
    }
}