using HelixToolkit.Wpf;
using Microsoft.Win32;
using OxyPlot;
using OxyPlot.Series;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Numerics;
using System.Runtime.InteropServices;
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
        private mechLIB_CPPWrapper.DataPointCPP[][] F;
        private mechLIB_CPPWrapper.DataPointCPP[][] p;
        private mechLIB_CPPWrapper.DataPointCPP[][] u;
        private mechLIB_CPPWrapper.DataPointCPP[][] v;
        private mechLIB_CPPWrapper.DataPointCPP[][] a;
        private float[] timeArr;
        private mechLIB_CPPWrapper.Enviro world;

        private int _CurrT;
        public int CurrT { get => _CurrT; set { _CurrT = value; Draw3d(value, SelDeriv); } }

        private int selDeriv;
        public int SelDeriv { get => selDeriv; set { selDeriv = value; if (world != null) { DrawPoints(value); } } }

        public Point3DCollection RopeCoords { get; set; }
        public ObservableCollection<Visual3D> Objs3d { get; set; }

        public PlotModel awePlotModelX { get; set; }
        public PlotModel awePlotModelY { get; set; }
        public PlotModel awePlotModelZ { get; set; }

        Thread thrsim;
        public MainViewModel(IEventAggregator ea)
        {
            _ea = ea;
            Props = new props();
            selDeriv = 0;
            _CurrT = 0;
            Objs3d = new ObservableCollection<Visual3D>();
            awePlotModelX = new PlotModel { Title = "X axis" };
            awePlotModelY = new PlotModel { Title = "Y axis" };
            awePlotModelZ = new PlotModel { Title = "Z axis" };
            awePlotModelX.InvalidatePlot(true);
            awePlotModelY.InvalidatePlot(true);
            awePlotModelZ.InvalidatePlot(true);
            _ea.GetEvent<GotResultsEvent>().Subscribe(() => thrsim = null);
            _ea.GetEvent<GotResultsEvent>().Subscribe(() => ShowResults(SelDeriv));
            _ea.GetEvent<ComputeEvent>().Subscribe((var) => Compute_Click(var));
            _ea.GetEvent<ClearPlotsEvent>().Subscribe(() => ClearDataView());
            //Load3dEvent
            //_ea.GetEvent<Load3dEvent>().Subscribe(() => Load3d());
        }

        private void Compute_Click(bool IsRunning)
        {
            if (IsRunning && thrsim == null)
            {
                _ea.GetEvent<ClearPlotsEvent>().Publish();
                thrsim = new Thread(delegate ()
                {
                    try
                    {
                        Simulate();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                        world.Destroy();
                        world = null;
                        _ea.GetEvent<GotResultsEvent>().Publish();
                    }
                });
                thrsim.Start();
            }

            else
            {
                thrsim.Abort();
                thrsim = null;
                world = null;

            }
        }

        private void Simulate()
        {
            F = null;
            p = u = v = a = null;
            string fileName = "";
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true) { fileName = openFileDialog.FileName; }

            world = new mechLIB_CPPWrapper.Enviro();
            world.CreateWorld(Props.DampRatio, Props.MaxU, Props.initDrop, Props.nodes, Props.E, Props.L,
                Props.D, Props.Counts, Props.dt, Props.ro, (mechLIB_CPPWrapper.PhModels)Props.phMod, fileName);
            world.Run();
            timeArr = Array.Empty<float>();
            F = Array.Empty<mechLIB_CPPWrapper.DataPointCPP[]>();
            p = Array.Empty<mechLIB_CPPWrapper.DataPointCPP[]>();
            u = Array.Empty<mechLIB_CPPWrapper.DataPointCPP[]>();
            v = Array.Empty<mechLIB_CPPWrapper.DataPointCPP[]>();
            a = Array.Empty<mechLIB_CPPWrapper.DataPointCPP[]>();
            int step = 1;
            if (Props.Counts > (int)SystemParameters.PrimaryScreenWidth / 2)
            {
                step = Props.Counts / (int)SystemParameters.PrimaryScreenWidth / 2;
            }
            world.GetTimeArr(step, ref timeArr);
            world.GetNodesF(step, ref F);
            world.GetNodesP(step, ref p);
            world.GetNodesU(step, ref u);
            world.GetNodesV(step, ref v);
            world.GetNodesA(step, ref a);
            world.Destroy();
            _ea.GetEvent<GotResultsEvent>().Publish();
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
            if (F != null && p != null && u != null && v != null && a != null)
            {
                DrawPoints(Deriv);
                Draw3d(CurrT, Deriv);
            }
        }

        private void DrawPoints(int Deriv)
        {
            ClearDataView();
            if (Deriv == (int)mechLIB_CPPWrapper.NodeLoad.f)
            {
                //foreach (var elem in world.rope.Elements)
                //{
                //    plotData("elem #" + elem.ID, elem.F);
                //}
                for (int n = 0; n < F.Length; n++)
                {
                    plotData("node #" + n, ref F[n]);
                }
            }
            else
            {
                switch ((mechLIB_CPPWrapper.Derivatives)Deriv)
                {
                    case mechLIB_CPPWrapper.Derivatives.p:
                        for (int n = 0; n < p.Length; n++)
                        {
                            plotData("node #" + n, ref p[n]);
                        }
                        break;
                    case mechLIB_CPPWrapper.Derivatives.u:
                        for (int n = 0; n < u.Length; n++)
                        {
                            plotData("node #" + n, ref u[n]);
                        }
                        break;
                    case mechLIB_CPPWrapper.Derivatives.v:
                        for (int n = 0; n < v.Length; n++)
                        {
                            plotData("node #" + n, ref v[n]);
                        }
                        break;
                    case mechLIB_CPPWrapper.Derivatives.a:
                        for (int n = 0; n < a.Length; n++)
                        {
                            plotData("node #" + n, ref a[n]);
                        }
                        break;
                    case mechLIB_CPPWrapper.Derivatives.maxDerivatives:
                        throw new System.Exception();
                    default:
                        throw new System.Exception();
                }
            }
        }

        public List<DataPoint> getDataPointListX(float[] X, mechLIB_CPPWrapper.DataPointCPP[] Y)
        {
            List<DataPoint> tmp = new List<DataPoint>();
            tmp.AddRange(new DataPoint[X.Length]);
            Parallel.For(0, X.Length,
                t =>
                {
                    tmp[t] = new DataPoint(X[t], Y[t].X);
                });
            return tmp;
        }
        public List<DataPoint> getDataPointListY(float[] X, mechLIB_CPPWrapper.DataPointCPP[] Y)
        {
            List<DataPoint> tmp = new List<DataPoint>();
            tmp.AddRange(new DataPoint[X.Length]);
            Parallel.For(0, X.Length,
                t =>
                {
                    tmp.Add(new DataPoint(X[t], Y[t].Y));
                });
            return tmp;
        }
        public List<DataPoint> getDataPointListZ(float[] X, mechLIB_CPPWrapper.DataPointCPP[] Y)
        {
            List<DataPoint> tmp = new List<DataPoint>();
            tmp.AddRange(new DataPoint[X.Length]);
            Parallel.For(0, X.Length,
                t =>
                   {
                       tmp.Add(new DataPoint(X[t], Y[t].Z));
                   });
            return tmp;
        }
        private void plotData(string title, ref mechLIB_CPPWrapper.DataPointCPP[] Y)
        {
            LineSeries aweLineSeries = new LineSeries { Title = title };
            List<DataPoint> data = getDataPointListX(timeArr, Y);
            aweLineSeries.Points.AddRange(data);
            awePlotModelX.Series.Add(aweLineSeries);
            awePlotModelX.InvalidatePlot(true);
            data = getDataPointListY(timeArr, Y);
            aweLineSeries = new LineSeries { Title = title };
            aweLineSeries.Points.AddRange(data);
            awePlotModelY.Series.Add(aweLineSeries);
            awePlotModelY.InvalidatePlot(true);
            data = getDataPointListZ(timeArr, Y);
            aweLineSeries = new LineSeries { Title = title };
            aweLineSeries.Points.AddRange(data);
            awePlotModelZ.Series.Add(aweLineSeries);
            awePlotModelZ.InvalidatePlot(true);
        }

        private void Draw3d(int t, int Deriv)
        {
            if (world == null)
            {
                return;
            }
            Application.Current.Dispatcher.Invoke(delegate
            {
                Objs3d.Clear();
                Load3d();
                //Objs3d.Add(new CubeVisual3D
                //{
                //    Center = new Point3D(NodesDerivs[0][(int)mechLIB_CPPWrapper.Derivatives.p][t][0] * 10E2,
                //                         NodesDerivs[0][(int)mechLIB_CPPWrapper.Derivatives.p][t][2] * 10E2,
                //                         NodesDerivs[0][(int)mechLIB_CPPWrapper.Derivatives.p][t][1] * 10E2),
                //    SideLength = .8,
                //    Fill = Brushes.Gray
                //});
                //Objs3d.Add(new CubeVisual3D
                //{
                //    Center = new Point3D(NodesDerivs[NodesDerivs.Length - 1][(int)mechLIB_CPPWrapper.Derivatives.p][t][0] * 10E2,
                //                         NodesDerivs[NodesDerivs.Length - 1][(int)mechLIB_CPPWrapper.Derivatives.p][t][2] * 10E2,
                //                         NodesDerivs[NodesDerivs.Length - 1][(int)mechLIB_CPPWrapper.Derivatives.p][t][1] * 10E2),
                //    SideLength = .8,
                //    Fill = Brushes.Gray
                //});
                //for (int node = 0; node < NodesDerivs.Length - 1; node++)
                //{
                //    Objs3d.Add(new LinesVisual3D
                //    {
                //        Points = { new Point3D(NodesDerivs[node][(int)mechLIB_CPPWrapper.Derivatives.p][t][0] * 10E2,
                //                                NodesDerivs[node][(int)mechLIB_CPPWrapper.Derivatives.p][t][2] * 10E2,
                //                                NodesDerivs[node][(int)mechLIB_CPPWrapper.Derivatives.p][t][1] * 10E2),
                //            new Point3D(NodesDerivs[node + 1][(int)mechLIB_CPPWrapper.Derivatives.p][t][0] * 10E2,
                //                        NodesDerivs[node + 1][(int)mechLIB_CPPWrapper.Derivatives.p][t][2] * 10E2,
                //                        NodesDerivs[node + 1][(int)mechLIB_CPPWrapper.Derivatives.p][t][1] * 10E2) },
                //        Thickness = 2,
                //        Color = Brushes.Blue.Color
                //    });
                //    Objs3d.Add(new SphereVisual3D
                //    {
                //        Center = new Point3D(NodesDerivs[node][(int)mechLIB_CPPWrapper.Derivatives.p][t][0] * 10E2,
                //                            NodesDerivs[node][(int)mechLIB_CPPWrapper.Derivatives.p][t][2] * 10E2,
                //                            NodesDerivs[node][(int)mechLIB_CPPWrapper.Derivatives.p][t][1] * 10E2),
                //        Radius = .3,
                //        Fill = Brushes.Black
                //    });
                //}
            });
        }
    }
}