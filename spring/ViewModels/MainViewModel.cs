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
        private Enviro world;

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
                world = new Enviro();
                thrsim = new Thread(delegate ()
                { Simulate(); });
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
            string fileName = "";
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true) { fileName = openFileDialog.FileName; }
            world.PrepRun(Props.store, fileName);
            world.Run();
            //ShowResults(SelDeriv);
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
            Rect3D BoundingBox = new Rect3D(0, 0, 0, 100, 100, 100);
            Objs3d.Add(new DefaultLights());

            double bbSize = Math.Max(Math.Max(BoundingBox.SizeX, BoundingBox.SizeY), BoundingBox.SizeZ);

            Objs3d.Add(new GridLinesVisual3D
            {
                Center = new Point3D(0, 0, 0),
                Length = BoundingBox.SizeX,
                Width = BoundingBox.SizeY,
                MinorDistance = 3,
                MajorDistance = bbSize,
                Thickness = bbSize / 1000,
                Fill = Brushes.Gray
            });
        }

        private void ShowResults(int Deriv)
        {
            if (world.rope.Nodes != null)
            {
                DrawPoints(Deriv);
                Draw3d(CurrT, Deriv);
            }
        }

        private void DrawPoints(int Deriv)
        {
            ClearDataView();
            if ((NodeLoad)Deriv == NodeLoad.f)
            {
                foreach (var elem in world.rope.Elements)
                {
                    plotData("elem #" + elem.ID, elem.F);
                }
                foreach (var node in world.rope.Nodes)
                {
                    plotData("node #" + node.ID, node.F);
                }
            }
            else
            {
                foreach (var node in world.rope.Nodes)
                {
                    plotData("node #" + node.ID, ExtractArray(node.deriv, (N_t)Deriv));
                    //plotData("An node #" + node.ID, ExtractArray(node.derivAn, (N_t)Deriv));
                }
            }
        }

        public List<DataPoint> getDataPointList(float[] X, xyz_t[] Y, C_t axis, int step)
        {
            List<DataPoint> tmp = new List<DataPoint>();
            for (int t = 0; t < X.Length; t += step)
            {
                tmp.Add(new DataPoint(X[t], Y[t].GetByC(axis)));
            }
            return tmp;
        }

        private xyz_t[] ExtractArray(deriv_t[] derivs, N_t deriv)
        {
            xyz_t[] tmp = new xyz_t[world.time.Length];
            for (int t = 0; t < world.time.Length; t++)
            {
                tmp[t] = derivs[t].GetByN(deriv);
            }
            return tmp;
        }

        private void plotData(string title, xyz_t[] Y)
        {
            int step = 1;
            int maxPlotPx = (int)SystemParameters.PrimaryScreenWidth / 2;

            if (Y.Length > maxPlotPx)
            {
                step = Y.Length / maxPlotPx;
            }

            List<DataPoint> data = getDataPointList(world.time, Y, C_t.x, step);
            LineSeries aweLineSeries = new LineSeries { Title = title };
            aweLineSeries.Points.AddRange(data);
            awePlotModelX.Series.Add(aweLineSeries);
            awePlotModelX.InvalidatePlot(true);
            data = getDataPointList(world.time, Y, C_t.y, step);
            aweLineSeries = new LineSeries { Title = title };
            aweLineSeries.Points.AddRange(data);
            awePlotModelY.Series.Add(aweLineSeries);
            awePlotModelY.InvalidatePlot(true);
            data = getDataPointList(world.time, Y, C_t.z, step);
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
                Objs3d.Add(new CubeVisual3D
                {
                    Center = new Point3D(world.rope.Nodes[0].deriv[t].p.x * 10E2,
                                         world.rope.Nodes[0].deriv[t].p.z * 10E2,
                                         world.rope.Nodes[0].deriv[t].p.y * 10E2),
                    SideLength = .8,
                    Fill = Brushes.Gray
                });
                Objs3d.Add(new CubeVisual3D
                {
                    Center = new Point3D(world.rope.Nodes[world.rope.Nodes.Length - 1].deriv[t].p.x * 10E2,
                                         world.rope.Nodes[world.rope.Nodes.Length - 1].deriv[t].p.z * 10E2,
                                         world.rope.Nodes[world.rope.Nodes.Length - 1].deriv[t].p.y * 10E2),
                    SideLength = .8,
                    Fill = Brushes.Gray
                });
                for (int node = 0; node < world.rope.Nodes.Length - 1; node++)
                {
                    Objs3d.Add(new LinesVisual3D
                    {
                        Points = { new Point3D(world.rope.Nodes[node].deriv[t].p.x * 10E2,
                                                world.rope.Nodes[node].deriv[t].p.z * 10E2,
                                                world.rope.Nodes[node].deriv[t].p.y * 10E2),
                            new Point3D(world.rope.Nodes[node + 1].deriv[t].p.x * 10E2,
                                        world.rope.Nodes[node + 1].deriv[t].p.z * 10E2,
                                        world.rope.Nodes[node + 1].deriv[t].p.y * 10E2) },
                        Thickness = 2,
                        Color = Brushes.Blue.Color
                    });
                    Objs3d.Add(new SphereVisual3D
                    {
                        Center = new Point3D(world.rope.Nodes[node].deriv[t].p.x * 10E2,
                                            world.rope.Nodes[node].deriv[t].p.z * 10E2,
                                            world.rope.Nodes[node].deriv[t].p.y * 10E2),
                        Radius = .3,
                        Fill = Brushes.Black
                    });
                }
            });
        }

        private void UpdTline(int t, int Deriv)
        {
            Application.Current.Dispatcher.Invoke(delegate
            {
                for (int obj = 4; obj < Objs3d.Count - 1; obj++)
                {
                    //Objs3d[obj].SetValue(new DependencyProperty, "" );

                    //Objs3d.Add(new LinesVisual3D
                    //{
                    //    Points = { new Point3D(Nodes[node].tm[t][(int)N.p][(int)C.x] * 10E2, Nodes[node].tm[t][(int)N.p][(int)C.z] * 10E2, Nodes[node].tm[t][(int)N.p][(int)C.y] * 10E2),
                    //        new Point3D(Nodes[node + 1].tm[t][(int)N.p][(int)C.x] * 10E2, Nodes[node + 1].tm[t][(int)N.p][(int)C.z] * 10E2, Nodes[node + 1].tm[t][(int)N.p][(int)C.y] * 10E2) },
                    //    Thickness = 2,
                    //    Color = Brushes.Blue.Color
                    //});
                    //Objs3d.Add(new SphereVisual3D
                    //{
                    //    Center = new Point3D(Nodes[node].tm[t][(int)N.p][(int)C.x] * 10E2, Nodes[node].tm[t][(int)N.p][(int)C.z] * 10E2, Nodes[node].tm[t][(int)N.p][(int)C.y] * 10E2),
                    //    Radius = .5,
                    //    Fill = Brushes.Black
                    //});
                }
            });
        }

        private void DrawTline(float t)
        {
            //Application.Current.Dispatcher.Invoke(delegate
            //{
            //    awePlotModelX.PlotView.ShowTracker(new TrackerHitResult() { Text = t.ToString(), DataPoint = new DataPoint(t, 0) });
            //    awePlotModelY.PlotView.ShowTracker(new TrackerHitResult() { Text = t.ToString(), DataPoint = new DataPoint(t, 0) });
            //    awePlotModelZ.PlotView.ShowTracker(new TrackerHitResult() { Text = t.ToString(), DataPoint = new DataPoint(t, 0) });
            //});
            //LineSeries aweLineSeriesX = new LineSeries { Title = "current time" };
            //aweLineSeriesX.Points.AddRange(new DataPoint[2] { new DataPoint(t, -1f), new DataPoint(t, 1f) });
            //LineSeries aweLineSeriesY = new LineSeries { Title = "current time" };
            //aweLineSeriesY.Points.AddRange(new DataPoint[2] { new DataPoint(t, -1f), new DataPoint(t, 1f) });
            //LineSeries aweLineSeriesZ = new LineSeries { Title = "current time" };
            //aweLineSeriesZ.Points.AddRange(new DataPoint[2] { new DataPoint(t, -1f), new DataPoint(t, 1f) });
            //if (awePlotModelX.Series.Count > 0)
            //{
            //    awePlotModelX.Series.RemoveAt(0);
            //}
            //awePlotModelX.Series.Insert(0, aweLineSeriesX);
            //awePlotModelX.InvalidatePlot(true);
            //if (awePlotModelY.Series.Count > 0)
            //{
            //    awePlotModelY.Series.RemoveAt(0);
            //}
            //awePlotModelY.Series.Insert(0, aweLineSeriesY);
            //awePlotModelY.InvalidatePlot(true);
            //if (awePlotModelZ.Series.Count > 0)
            //{
            //    awePlotModelZ.Series.RemoveAt(0);
            //}
            //awePlotModelZ.Series.Insert(0, aweLineSeriesZ);
            //awePlotModelZ.InvalidatePlot(true);
        }
    }
}