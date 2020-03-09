using DotNetDoctor.csmatio.io;
using DotNetDoctor.csmatio.types;
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
        private float[][][][] NodesDerivs;
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
            NodesDerivs = null;
            string fileName = "";
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true) { fileName = openFileDialog.FileName; }
            
            world = new mechLIB_CPPWrapper.Enviro();
            world.CreateWorld(Props.DampRatio, Props.MaxU, Props.initDrop, Props.nodes, Props.E, Props.L,
                Props.D, Props.Counts, Props.dt, Props.ro, (mechLIB_CPPWrapper.PhModels)Props.phMod, fileName);
            world.Run();
            NodesDerivs = Array.Empty<float[][][]>();
            world.GetNodesDerivs(ref NodesDerivs);
            world.GetTimeArr(ref timeArr);
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
            if (NodesDerivs != null)
            {
                DrawPoints(Deriv);
                Draw3d(CurrT, Deriv);
            }
        }

        private void DrawPoints(int Deriv)
        {
            ClearDataView();
            if ((mechLIB_CPPWrapper.NodeLoad)Deriv == mechLIB_CPPWrapper.NodeLoad.f)
            {
                //foreach (var elem in world.rope.Elements)
                //{
                //    plotData("elem #" + elem.ID, elem.F);
                //}
                for (int i = 0; i < NodesDerivs.Length; i++)
                {
                    plotData("node #" + i, NodesDerivs[i][(int)mechLIB_CPPWrapper.Derivatives.f]);
                }
            }
            else
            {
                for (int i = 0; i < NodesDerivs.Length; i++)
                {
                    plotData("node #" + i, NodesDerivs[i][Deriv]);
                }
            }
        }

        public List<DataPoint> getDataPointList(float[] X, float[][] Y, mechLIB_CPPWrapper.C_t axis, int step)
        {
            List<DataPoint> tmp = new List<DataPoint>();
            for (int t = 0; t < X.Length; t += step)
            {
                switch (axis)
                {
                    case mechLIB_CPPWrapper.C_t.x:
                        tmp.Add(new DataPoint(X[t], Y[t][0]));
                        break;
                    case mechLIB_CPPWrapper.C_t.y:
                        tmp.Add(new DataPoint(X[t], Y[t][1]));
                        break;
                    case mechLIB_CPPWrapper.C_t.z:
                        tmp.Add(new DataPoint(X[t], Y[t][2]));
                        break;
                    default:
                        throw new System.Exception();
                }
            }
            return tmp;
        }
        
        private void plotData(string title, float[][] Y)
        {
            int step = 1;
            int maxPlotPx = (int)SystemParameters.PrimaryScreenWidth / 2;

            if (Y.Length > maxPlotPx)
            {
                step = Y.Length / maxPlotPx;
            }

            List<DataPoint> data = getDataPointList(timeArr, Y, mechLIB_CPPWrapper.C_t.x, step);
            LineSeries aweLineSeries = new LineSeries { Title = title };
            aweLineSeries.Points.AddRange(data);
            awePlotModelX.Series.Add(aweLineSeries);
            awePlotModelX.InvalidatePlot(true);
            data = getDataPointList(timeArr, Y, mechLIB_CPPWrapper.C_t.y, step);
            aweLineSeries = new LineSeries { Title = title };
            aweLineSeries.Points.AddRange(data);
            awePlotModelY.Series.Add(aweLineSeries);
            awePlotModelY.InvalidatePlot(true);
            data = getDataPointList(timeArr, Y, mechLIB_CPPWrapper.C_t.z, step);
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
                //    Center = new Point3D(world.rope.Nodes[0].deriv[t].p.X * 10E2,
                //                         world.rope.Nodes[0].deriv[t].p.Z * 10E2,
                //                         world.rope.Nodes[0].deriv[t].p.Y * 10E2),
                //    SideLength = .8,
                //    Fill = Brushes.Gray
                //});
                //Objs3d.Add(new CubeVisual3D
                //{
                //    Center = new Point3D(world.rope.Nodes[world.rope.Nodes.Length - 1].deriv[t].p.X * 10E2,
                //                         world.rope.Nodes[world.rope.Nodes.Length - 1].deriv[t].p.Z * 10E2,
                //                         world.rope.Nodes[world.rope.Nodes.Length - 1].deriv[t].p.Y * 10E2),
                //    SideLength = .8,
                //    Fill = Brushes.Gray
                //});
                //for (int node = 0; node < world.rope.Nodes.Length - 1; node++)
                //{
                //    Objs3d.Add(new LinesVisual3D
                //    {
                //        Points = { new Point3D(world.rope.Nodes[node].deriv[t].p.X * 10E2,
                //                                world.rope.Nodes[node].deriv[t].p.Z * 10E2,
                //                                world.rope.Nodes[node].deriv[t].p.Y * 10E2),
                //            new Point3D(world.rope.Nodes[node + 1].deriv[t].p.X * 10E2,
                //                        world.rope.Nodes[node + 1].deriv[t].p.Z * 10E2,
                //                        world.rope.Nodes[node + 1].deriv[t].p.Y * 10E2) },
                //        Thickness = 2,
                //        Color = Brushes.Blue.Color
                //    });
                //    Objs3d.Add(new SphereVisual3D
                //    {
                //        Center = new Point3D(world.rope.Nodes[node].deriv[t].p.X * 10E2,
                //                            world.rope.Nodes[node].deriv[t].p.Z * 10E2,
                //                            world.rope.Nodes[node].deriv[t].p.Y * 10E2),
                //        Radius = .3,
                //        Fill = Brushes.Black
                //    });
                //}
            });
        }
    }
}