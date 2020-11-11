using HelixToolkit.Wpf;
using mechLIB_CPP;
using Microsoft.Win32;
using OxyPlot;
using OxyPlot.Series;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public props Props { get; set; }
        private DataPointCPP[][] F;
        private DataPointCPP[][] p;
        private DataPointCPP[][] u;
        private DataPointCPP[][] v;
        private DataPointCPP[][] a;
        private float[] timeArr;
        private EnviroWrapper world;

        private int _CurrT;
        public int CurrT { get => _CurrT; set { _CurrT = value; Draw3d(value, SelDeriv); } }

        private int selDeriv;
        public int SelDeriv { get => selDeriv; set { selDeriv = value; if (IsArrayExist) { DrawPoints(value); } } }

        public Point3DCollection RopeCoords { get; set; }
        public ObservableCollection<Visual3D> Objs3d { get; set; }

        public PlotModel awePlotModelX { get; set; }
        public PlotModel awePlotModelY { get; set; }
        public PlotModel awePlotModelZ { get; set; }
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
            Props = new props();
            selDeriv = 0;
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
                world.Destroy();
                world = null;
            }
        }

        private void Simulate(string fileName)
        {
            EndT = 1;
            F = null;
            p = u = v = a = null;
            world = new EnviroWrapper();
            try
            {
                world.CreateWorld(Props.DampRatio, Props.MaxU, Props.initDrop, Props.nodes, Props.E, Props.L,
                    Props.D, Props.Counts, Props.dt, Props.ro, (PhModels)Props.phMod, fileName);
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
                F = Array.Empty<DataPointCPP[]>();
                p = Array.Empty<DataPointCPP[]>();
                u = Array.Empty<DataPointCPP[]>();
                v = Array.Empty<DataPointCPP[]>();
                a = Array.Empty<DataPointCPP[]>();
                int step = 1;
                if (Props.Counts > (int)SystemParameters.PrimaryScreenWidth / 2)
                {
                    step = Props.Counts / ((int)SystemParameters.PrimaryScreenWidth / 2);
                }
                world.GetTimeArr(step, ref timeArr);
                world.GetNodesF(step, ref F);
                world.GetNodesP(step, ref p);
                world.GetNodesU(step, ref u);
                world.GetNodesV(step, ref v);
                world.GetNodesA(step, ref a);
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
                world.Destroy();
                world = null;
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

        private void ShowResults(int Deriv)
        {
            if (IsArrayExist)
            {
                DrawPoints(Deriv);
                Draw3d(CurrT, Deriv);
            }
        }

        private bool IsArrayExist => F != null && p != null && u != null && v != null && a != null;

        private void DrawPoints(int Deriv)
        {
            ClearDataView();
            if (Deriv == (int)NodeLoad.f)
            {
                //foreach (var elem in world.rope.Elements)
                //{
                //    plotData("elem #" + elem.ID, elem.F);
                //}
                for (int n = 0; n < F.Length; n++)
                {
                    PlotData("node #" + n, F[n]);
                }
            }
            else
            {
                switch ((Derivatives)Deriv)
                {
                    case Derivatives.p:
                        for (int n = 0; n < p.Length; n++)
                        {
                            PlotData("node #" + n, p[n]);
                        }
                        break;
                    case Derivatives.u:
                        for (int n = 0; n < u.Length; n++)
                        {
                            PlotData("node #" + n, u[n]);
                        }
                        break;
                    case Derivatives.v:
                        for (int n = 0; n < v.Length; n++)
                        {
                            PlotData("node #" + n, v[n]);
                        }
                        break;
                    case Derivatives.a:
                        for (int n = 0; n < a.Length; n++)
                        {
                            PlotData("node #" + n, a[n]);
                        }
                        break;
                    case Derivatives.maxDerivatives:
                        throw new System.Exception();
                    default:
                        throw new System.Exception();
                }
            }
            awePlotModelX.InvalidatePlot(true);
            awePlotModelY.InvalidatePlot(true);
            awePlotModelZ.InvalidatePlot(true);
        }

        private void PlotData(string title, DataPointCPP[] Y)
        {
            plotDataX(title, timeArr, Y);
            plotDataY(title, timeArr, Y);
            plotDataZ(title, timeArr, Y);
        }
        public void plotDataX(string title, float[] X, DataPointCPP[] Y)
        {
            List<DataPoint> tmp = new List<DataPoint>();
            tmp.AddRange(new DataPoint[X.Length]);
            Parallel.For(0, X.Length,
                t =>
                {
                    tmp[t] = new DataPoint(X[t], Y[t].X);
                });
            LineSeries aweLineSeries = new LineSeries { Title = title };
            aweLineSeries.Points.AddRange(tmp);
            awePlotModelX.Series.Add(aweLineSeries);
        }
        public void plotDataY(string title, float[] X, DataPointCPP[] Y)
        {
            List<DataPoint> tmp = new List<DataPoint>();
            tmp.AddRange(new DataPoint[X.Length]);
            Parallel.For(0, X.Length,
                t =>
                {
                    tmp[t] = new DataPoint(X[t], Y[t].Y);
                });
            LineSeries aweLineSeries = new LineSeries { Title = title };
            aweLineSeries.Points.AddRange(tmp);
            awePlotModelY.Series.Add(aweLineSeries);
        }
        public void plotDataZ(string title, float[] X, DataPointCPP[] Y)
        {
            List<DataPoint> tmp = new List<DataPoint>();
            tmp.AddRange(new DataPoint[X.Length]);
            Parallel.For(0, X.Length,
                t =>
                {
                    tmp[t] = new DataPoint(X[t], Y[t].Z);
                });
            LineSeries aweLineSeries = new LineSeries { Title = title };
            aweLineSeries.Points.AddRange(tmp);
            awePlotModelZ.Series.Add(aweLineSeries);
        }

        private void Draw3d(int t, int Deriv)
        {
            if (p != null && p.Length > 0)
            {
                Application.Current.Dispatcher.Invoke(delegate
                {
                    Objs3d.Clear();
                    Load3d();
                    Objs3d.Add(new CubeVisual3D
                    {
                        Center = new Point3D(p[0][t].X * 10E2,
                                             p[0][t].Z * 10E2,
                                             p[0][t].Y * 10E2),
                        SideLength = .8,
                        Fill = Brushes.Gray
                    });
                    Objs3d.Add(new CubeVisual3D
                    {
                        Center = new Point3D(p[p.Length - 1][t].X * 10E2,
                                             p[p.Length - 1][t].Z * 10E2,
                                             p[p.Length - 1][t].Y * 10E2),
                        SideLength = .8,
                        Fill = Brushes.Gray
                    });
                    for (int node = 0; node < p.Length - 1; node++)
                    {
                        Objs3d.Add(new LinesVisual3D
                        {
                            Points = { new Point3D(p[node][t].X * 10E2,
                                                p[node][t].Z * 10E2,
                                                p[node][t].Y * 10E2),
                            new Point3D(p[node + 1][t].X * 10E2,
                                        p[node + 1][t].Z * 10E2,
                                        p[node + 1][t].Y * 10E2) },
                            Thickness = 2,
                            Color = Brushes.Blue.Color
                        });
                        Objs3d.Add(new SphereVisual3D
                        {
                            Center = new Point3D(p[node][t].X * 10E2,
                                                p[node][t].Z * 10E2,
                                                p[node][t].Y * 10E2),
                            Radius = .3,
                            Fill = Brushes.Black
                        });
                    }
                });
            }
        }
    }
}