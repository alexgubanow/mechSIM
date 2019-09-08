using HelixToolkit.Wpf;
using OxyPlot;
using OxyPlot.Series;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        public float[] time;
        private float[][][] load;
        public Node_t[] Nodes;

        public props Props { get; set; }
        public float EndT { get => Props.Counts - 1; }

        private int _CurrT;
        public int CurrT { get => _CurrT; set { _CurrT = value; Draw3d(value, SelDeriv); } }

        private int selDeriv;
        public int SelDeriv { get => selDeriv; set { selDeriv = value; if (Nodes != null) { ShowDeriv(value); } } }

        private void ShowDeriv(int Derivative)
        {
        }

        public Point3DCollection RopeCoords { get; set; }
        public ObservableCollection<Visual3D> Objs3d { get; set; }

        public PlotModel awePlotModelX { get; set; }
        public PlotModel awePlotModelY { get; set; }
        public PlotModel awePlotModelZ { get; set; }

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
            //_ea.GetEvent<GotResultsEvent>().Subscribe(() => ShowResults());
            //_ea.GetEvent<ClearPlotsEvent>().Subscribe(() => ClearPlot());
            _ea.GetEvent<ComputeEvent>().Subscribe(() => Compute_Click());
            //Load3dEvent
            //_ea.GetEvent<Load3dEvent>().Subscribe(() => Load3d());
        }

        private DelegateCommand _Compute;

        public DelegateCommand Compute => _Compute ?? (_Compute = new DelegateCommand(() => _ea.GetEvent<ComputeEvent>().Publish()));

        private float[] getT(float dt, int Counts)
        {
            float[] tCounts = new float[Counts];
            for (int i = 1; i < Counts; i++)
            {
                tCounts[i] = tCounts[i - 1] + dt;
            }
            return tCounts;
        }

        private float[][][] getLoad(NodeLoad ltype, C axis, int nodes, int Counts)
        {
            float[][][] tCounts = new float[nodes][][];
            for (int node = 0; node < nodes; node++)
            {
                float maxUx = 0.01f * Props.L / nodes / 100;
                float A = (float)Math.PI * (float)Math.Pow(Props.D, 2) / 4;
                float maxLoad = ((Props.E * A) / Props.L / nodes) * maxUx;
                if (node != 0 && node != nodes - 1)
                {
                    tCounts[node] = new float[Counts][];
                    tCounts[node][0] = new float[3];
                    for (int t = 1; t < Counts; t++)
                    {
                        tCounts[node][t] = new float[3];
                        switch (ltype)
                        {
                            case NodeLoad.u:
                                maxUx = 0.01f * Props.L / nodes / 100;
                                float ut = (float)Math.Sin(2 * Math.PI * 0.5 * time[t]) * maxUx;
                                //float ut = (maxUx / (0 - t)) + maxUx;
                                tCounts[node][t][(int)axis] = ut;
                                //tCounts[node][t][0] = ((E * A) / L / nodeCount) * ut;
                                break;

                            case NodeLoad.a:
                                break;

                            case NodeLoad.f:
                                //(maxUx / (0 - t)) + maxUx
                                //tCounts[node][t][(int)axis] = 0 - ((float)Math.Sin(2 * Math.PI * 0.5 * time[t]) * maxUx);
                                tCounts[node][t][(int)axis] = 0 - (tCounts[node][t - 1][(int)axis] + (maxLoad / (0 - t)) + maxLoad);
                                //tCounts[node][t][0] = tCounts[node][t - 1][0] + maxLoad / Counts;
                                break;

                            case NodeLoad.p:
                                break;

                            case NodeLoad.none:
                                break;

                            default:
                                break;
                        }
                    }
                }
            }
            return tCounts;
        }

        private async void Compute_Click()
        {
            Nodes = null;
            //_ea.GetEvent<ClearPlotsEvent>().Publish();
            time = getT(Props.dt, Props.Counts);
            load = getLoad(NodeLoad.f, C.y, Props.nodes, Props.Counts);

            #region load file

            //OpenFileDialog openFileDialog = new OpenFileDialog();
            //if (openFileDialog.ShowDialog() == true)
            //{
            //    string fileName = openFileDialog.FileName;
            //    // deserialize JSON directly from a file
            //    using (StreamReader file = File.OpenText(fileName))
            //    {
            //        JsonSerializer serializer = new JsonSerializer();
            //        Load ld = (Load)serializer.Deserialize(file, typeof(Load));
            //        load = new float[nodeCount][][];
            //        load[nodeCount - 1] = new float[Counts][];
            //        for (int i = 0; i < Counts; i++)
            //        {
            //            load[nodeCount - 1][i] = new float[3];
            //            load[nodeCount - 1][i][0] = ld.x[i];
            //            load[nodeCount - 1][i][1] = ld.y[i];
            //        }
            //    }
            //    ////txtOutput.Text = txtOutput.Text + "Attempting to read the file '" + fileName + "'...";
            //    //try
            //    //{
            //    //}
            //    //catch (Exception)
            //    //{
            //    //    throw new Exception();
            //    //    //txtOutput.Text = txtOutput.Text + "Invalid MAT-file!\n";
            //    //    //MessageBox.Show("Invalid binary MAT-file! Please select a valid binary MAT-file.",
            //    //    //    "Invalid MAT-file", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            //    //}
            //}

            #endregion load file

            SetupNodesPositions();
            foreach (var node in Nodes)
            {
                Rope_t.EvalLinksLength(Nodes, node.NodeID, Props.D, Props.ro);
            }
            await Task.Run(Simulating);
        }

        private void SetupNodesPositions()
        {
            float dl = Props.L / Props.nodes;
            float pos = 0;
            Nodes = new Node_t[Props.nodes];
            Nodes[0] = new Node_t(Props.Counts, new float[3] { pos, 0, 0 }, NodeFreedom.locked, NodeLoad.none, 0, new int[1] { 1 }, Props.E, Props.D);
            pos += dl;
            for (int i = 1; i < Nodes.Length - 1; i++)
            {
                Nodes[i] = new Node_t(Props.Counts, new float[3] { pos, 0, 0 }, NodeFreedom.xyz, NodeLoad.none, i, new int[2] { i - 1, i + 1 }, Props.E, Props.D);
                pos += dl;
            }
            Nodes[Nodes.Length - 1] = new Node_t(Props.Counts, new float[3] { pos, 0, 0 }, NodeFreedom.locked, NodeLoad.none, Nodes.Length - 1, new int[1] { Nodes.Length - 2 }, Props.E, Props.D);

            //Nodes[0].tm[0][(int)N.p] = new float[] { float.Parse("1e-3", CultureInfo.InvariantCulture), float.Parse("6e-3", CultureInfo.InvariantCulture), 0 };
            //Nodes[1].tm[0][(int)N.p] = new float[] { float.Parse("2e-3", CultureInfo.InvariantCulture), float.Parse("4e-3", CultureInfo.InvariantCulture), 0 };
            //Nodes[2].tm[0][(int)N.p] = new float[] { float.Parse("4e-3", CultureInfo.InvariantCulture), float.Parse("2e-3", CultureInfo.InvariantCulture), 0 };
            //Nodes[3].tm[0][(int)N.p] = new float[] { float.Parse("7e-3", CultureInfo.InvariantCulture), float.Parse("2e-3", CultureInfo.InvariantCulture), 0 };
            //Nodes[4].tm[0][(int)N.p] = new float[] { float.Parse("10e-3", CultureInfo.InvariantCulture), float.Parse("2e-3", CultureInfo.InvariantCulture), 0 };
            //Nodes[5].tm[0][(int)N.p] = new float[] { float.Parse("12e-3", CultureInfo.InvariantCulture), float.Parse("4e-3", CultureInfo.InvariantCulture), 0 };
            //Nodes[6].tm[0][(int)N.p] = new float[] { float.Parse("13e-3", CultureInfo.InvariantCulture), float.Parse("6e-3", CultureInfo.InvariantCulture), 0 };
            Nodes[Nodes.Length / 2].LoadType = NodeLoad.f;
            //Nodes[Nodes.Length - 1].freedom = NodeFreedom.xyz;
            for (int i = 0; i < Nodes.Length; i++)
            {
                Nodes[i].tm[0][(int)N.p] = new float[] { i * dl, Props.initDrop * (float)Math.Pow((i * dl) - Props.L / 2, 2) + 1E-3f, 0 };
            }
        }

        private void Simulating()
        {
            for (int t = 1; t < time.Length; t++)
            {
                Rope_t.IterateOverNodes(Nodes, t, Props.dt, load);
            }
            //_ea.GetEvent<GotResultsEvent>().Publish();
            ShowResults(SelDeriv);
            GC.Collect();
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
            ClearDataView();
            if (Nodes != null)
            {
                DrawPoints(Deriv);
                Draw3d(CurrT, Deriv);
            }
        }

        private void DrawPoints(int Deriv)
        {
            if ((N)SelDeriv == N.f)
            {
                plotData("Fext", load[Nodes.Length / 2]);
            }
            foreach (var node in Nodes)
            {
                float[][] tmp = ExtractArray(node.tm, (N)SelDeriv);
                plotData("node #" + node.NodeID, tmp);
                tmp = null;
            }
        }

        public List<DataPoint> getDataPointList(float[] X, float[][] Y, C axis)
        {
            List<DataPoint> tmp = new List<DataPoint>();
            for (int t = 0; t < X.Length; t++)
            {
                tmp.Add(new DataPoint(X[t], Y[t][(int)axis]));
            }
            return tmp;
        }

        private void plotData(string title, float[][] Y)
        {
            List<DataPoint> data = getDataPointList(time, Y, C.x);
            LineSeries aweLineSeries = new LineSeries { Title = title };
            aweLineSeries.Points.AddRange(data);
            awePlotModelX.Series.Add(aweLineSeries);
            awePlotModelX.InvalidatePlot(true);
            data = getDataPointList(time, Y, C.y);
            aweLineSeries = new LineSeries { Title = title };
            aweLineSeries.Points.AddRange(data);
            awePlotModelY.Series.Add(aweLineSeries);
            awePlotModelY.InvalidatePlot(true);
            data = getDataPointList(time, Y, C.z);
            aweLineSeries = new LineSeries { Title = title };
            aweLineSeries.Points.AddRange(data);
            awePlotModelZ.Series.Add(aweLineSeries);
            awePlotModelZ.InvalidatePlot(true);
        }

        private float[][] ExtractArray(float[][][] tm, N deriv)
        {
            float[][] tmp = new float[time.Length][];
            for (int t = 0; t < time.Length; t++)
            {
                tmp[t] = tm[t][(int)deriv];
            }
            return tmp;
        }

        private void Draw3d(int t, int Deriv)
        {
            Application.Current.Dispatcher.Invoke(delegate
            {
                Objs3d.Clear();
                Load3d();
                Objs3d.Add(new CubeVisual3D
                {
                    Center = new Point3D(Nodes[0].tm[t][(int)N.p][(int)C.x] * 10E2, Nodes[0].tm[t][(int)N.p][(int)C.z] * 10E2, Nodes[0].tm[t][(int)N.p][(int)C.y] * 10E2),
                    SideLength = .8,
                    Fill = Brushes.Gray
                });
                Objs3d.Add(new CubeVisual3D
                {
                    Center = new Point3D(Nodes[Nodes.Length - 1].tm[t][(int)N.p][(int)C.x] * 10E2, Nodes[Nodes.Length - 1].tm[t][(int)N.p][(int)C.z] * 10E2, Nodes[Nodes.Length - 1].tm[t][(int)N.p][(int)C.y] * 10E2),
                    SideLength = .8,
                    Fill = Brushes.Gray
                });
                for (int node = 0; node < Nodes.Length - 1; node++)
                {
                    Objs3d.Add(new LinesVisual3D
                    {
                        Points = { new Point3D(Nodes[node].tm[t][(int)N.p][(int)C.x] * 10E2, Nodes[node].tm[t][(int)N.p][(int)C.z] * 10E2, Nodes[node].tm[t][(int)N.p][(int)C.y] * 10E2),
                            new Point3D(Nodes[node + 1].tm[t][(int)N.p][(int)C.x] * 10E2, Nodes[node + 1].tm[t][(int)N.p][(int)C.z] * 10E2, Nodes[node + 1].tm[t][(int)N.p][(int)C.y] * 10E2) },
                        Thickness = 2,
                        Color = Brushes.Blue.Color
                    });
                    Objs3d.Add(new SphereVisual3D
                    {
                        Center = new Point3D(Nodes[node].tm[t][(int)N.p][(int)C.x] * 10E2, Nodes[node].tm[t][(int)N.p][(int)C.z] * 10E2, Nodes[node].tm[t][(int)N.p][(int)C.y] * 10E2),
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