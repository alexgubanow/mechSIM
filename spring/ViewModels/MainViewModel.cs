using HelixToolkit.Wpf;
using mechLIB;
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
        private Rope_t model;
        public props Props { get; set; }
        private int _EndT;
        public int EndT { get => _EndT; set { _EndT = value; } }

        private int _CurrT;
        public int CurrT { get => _CurrT; set { _CurrT = value; Draw3d(value, SelDeriv); } }

        private int selDeriv;
        public int SelDeriv { get => selDeriv; set { selDeriv = value; if (model != null) { DrawPoints(value); } } }

        public Point3DCollection RopeCoords { get; set; }
        public ObservableCollection<Visual3D> Objs3d { get; set; }

        public PlotModel awePlotModelX { get; set; }
        public PlotModel awePlotModelY { get; set; }
        public PlotModel awePlotModelZ { get; set; }

        public MainViewModel(IEventAggregator ea)
        {
            _ea = ea;
            Props = new props();
            EndT = Props.store.Counts - 1;
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

        private void getLoad(C_t axis, ref Rope_t model)
        {
            float A = (float)Math.PI * (float)Math.Pow(Props.store.D, 2) / 4;
            float maxLoad = ((Props.store.E * A) / Props.store.L / Props.store.nodes) * Props.store.MaxU;
            float freq = 1 / (Props.store.Counts * Props.store.dt);
            for (int t = 0; t < Props.store.Counts; t++)
            {
                model.Nodes[0].F[t].x = 0 - ((float)Math.Sin(2 * Math.PI * 0.5 * time[t] * freq) * maxLoad);
                //model.Nodes[0].deriv[t].p.z = model.Nodes[0].deriv[0].p.z;
                //model.Nodes[0].deriv[t].p.y = model.Nodes[0].deriv[0].p.y;
                //model.Nodes[0].deriv[t].p.x = 0 - ((time[t] + time[1]) * Props.store.MaxU);
                //model.Nodes[0].deriv[t].v.x = (model.Nodes[0].deriv[t].p.x - (0 - (time[t] * Props.store.MaxU))) / time[1];
                int lastN = Props.store.nodes - 1;
                //model.Nodes[lastN].deriv[t].p.z = model.Nodes[lastN].deriv[0].p.z;
                //model.Nodes[lastN].deriv[t].p.y = model.Nodes[lastN].deriv[0].p.y;
                //model.Nodes[lastN].deriv[t].p.x = model.Nodes[lastN].deriv[0].p.x;
                model.Nodes[lastN].F[t].x = ((float)Math.Sin(2 * Math.PI * 0.5 * time[t] * freq) * maxLoad);
                //model.Nodes[lastN].deriv[t].p.x =  ((float)Math.Sin(2 * Math.PI * 0.5 * time[t] * freq) * maxLoad) + model.Nodes[lastN].deriv[0].p.x;
            }
        }

        private async void Compute_Click()
        {
            //model = null;
            //_ea.GetEvent<ClearPlotsEvent>().Publish();
            time = getT(Props.store.dt, Props.store.Counts);

            CurrT = 100;
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

            model = new Rope_t(Props.store);
            foreach (var elem in model.Elements)
            {
                elem.CalcMass(ref model, Props.store.ro);
            }
            foreach (var node in model.Nodes)
            {
                node.CalcMass(ref model);
            }
            getLoad(C_t.x, ref model);
            await Task.Run(Simulating);
        }

        private void Simulating()
        {
            float A = (float)Math.PI * (float)Math.Pow(Props.store.D, 2) / 4;
            float maxLoad = ((Props.store.E * A) / Props.store.L / Props.store.nodes) * Props.store.MaxU;
            float w = 1000;
            float n = 1;
            for (int t = 1; t < time.Length; t++)
            {
                foreach (var elem in model.Elements)
                {
                    elem.CalcForce(ref model, t);
                }
                foreach (var node in model.Nodes)
                {
                    float w0 = maf.sqrt(node.k0 / node.m);
                    float Zm = maf.sqrt(maf.P2(2 * w0 * node.DampRatio) + (1 / maf.P2(w)) * maf.P2(maf.P2(w0) - maf.P2(w)));
                    float phi = maf.atan((2 * w * w0 * node.DampRatio) / (maf.P2(w) - maf.P2(w0))) + (n * maf.pi);
                    float xt = (maxLoad / (node.m * Zm * w)) * maf.sin(w * time[t] + phi);
                    //https://www.wolframalpha.com/input/?i=d%2Fdt+%28F0%2F%28w*k%29%29*sin%28wt%2Bphi%29
                    float vt = (maxLoad / (node.m * Zm)) * maf.cos(w * time[t] + phi);
                    //https://www.wolframalpha.com/input/?i=d%5E2%2Fdt%5E2+%28F0%2F%28w*k%29%29*sin%28wt%2Bphi%29
                    float at = (maxLoad / (node.m * Zm)) * (0 - maf.sin(w * time[t] + phi)) * w;
                    float Ft = (1 / node.m) * maxLoad * maf.sin(w * time[t]);
                    node.GetForces(ref model, t);
                    node.CalcAccel(ref model, t);
                    /*integrate*/
                    node.Integrate(t, t - 1, Props.store.dt);
                }
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
            if (model.Nodes != null)
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
                foreach (var elem in model.Elements)
                {
                    plotData("elem #" + elem.ID, elem.F);
                }
                foreach (var node in model.Nodes)
                {
                    plotData("node #" + node.ID, node.F);
                }
            }
            else
            {
                foreach (var node in model.Nodes)
                {
                    plotData("node #" + node.ID, ExtractArray(node.deriv, (N_t)Deriv));
                }
            }
        }

        public List<DataPoint> getDataPointList(float[] X, xyz_t[] Y, C_t axis)
        {
            List<DataPoint> tmp = new List<DataPoint>();
            for (int t = 0; t < X.Length; t += 100)
            {
                tmp.Add(new DataPoint(X[t], Y[t].GetByC(axis)));
            }
            return tmp;
        }

        private xyz_t[] ExtractArray(deriv_t[] derivs, N_t deriv)
        {
            xyz_t[] tmp = new xyz_t[time.Length];
            for (int t = 0; t < time.Length; t++)
            {
                tmp[t] = derivs[t].GetByN(deriv);
            }
            return tmp;
        }

        private void plotData(string title, xyz_t[] Y)
        {
            List<DataPoint> data = getDataPointList(time, Y, C_t.x);
            LineSeries aweLineSeries = new LineSeries { Title = title };
            aweLineSeries.Points.AddRange(data);
            awePlotModelX.Series.Add(aweLineSeries);
            awePlotModelX.InvalidatePlot(true);
            data = getDataPointList(time, Y, C_t.y);
            aweLineSeries = new LineSeries { Title = title };
            aweLineSeries.Points.AddRange(data);
            awePlotModelY.Series.Add(aweLineSeries);
            awePlotModelY.InvalidatePlot(true);
            data = getDataPointList(time, Y, C_t.z);
            aweLineSeries = new LineSeries { Title = title };
            aweLineSeries.Points.AddRange(data);
            awePlotModelZ.Series.Add(aweLineSeries);
            awePlotModelZ.InvalidatePlot(true);
        }

        private void Draw3d(int t, int Deriv)
        {
            if (model == null)
            {
                return;
            }
            Application.Current.Dispatcher.Invoke(delegate
            {
                Objs3d.Clear();
                Load3d();
                Objs3d.Add(new CubeVisual3D
                {
                    Center = new Point3D(model.Nodes[0].deriv[t].p.x * 10E2,
                                         model.Nodes[0].deriv[t].p.z * 10E2,
                                         model.Nodes[0].deriv[t].p.y * 10E2),
                    SideLength = .8,
                    Fill = Brushes.Gray
                });
                Objs3d.Add(new CubeVisual3D
                {
                    Center = new Point3D(model.Nodes[model.Nodes.Length - 1].deriv[t].p.x * 10E2,
                                         model.Nodes[model.Nodes.Length - 1].deriv[t].p.z * 10E2,
                                         model.Nodes[model.Nodes.Length - 1].deriv[t].p.y * 10E2),
                    SideLength = .8,
                    Fill = Brushes.Gray
                });
                for (int node = 0; node < model.Nodes.Length - 1; node++)
                {
                    Objs3d.Add(new LinesVisual3D
                    {
                        Points = { new Point3D(model.Nodes[node].deriv[t].p.x * 10E2,
                                                model.Nodes[node].deriv[t].p.z * 10E2,
                                                model.Nodes[node].deriv[t].p.y * 10E2),
                            new Point3D(model.Nodes[node + 1].deriv[t].p.x * 10E2,
                                        model.Nodes[node + 1].deriv[t].p.z * 10E2,
                                        model.Nodes[node + 1].deriv[t].p.y * 10E2) },
                        Thickness = 2,
                        Color = Brushes.Blue.Color
                    });
                    Objs3d.Add(new SphereVisual3D
                    {
                        Center = new Point3D(model.Nodes[node].deriv[t].p.x * 10E2,
                                            model.Nodes[node].deriv[t].p.z * 10E2,
                                            model.Nodes[node].deriv[t].p.y * 10E2),
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