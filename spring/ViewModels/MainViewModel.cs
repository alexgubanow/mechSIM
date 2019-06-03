using OxyPlot;
using OxyPlot.Series;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;

namespace spring.ViewModels
{
    public partial class MainViewModel : BindableBase
    {
        private readonly IEventAggregator _ea;

        public PlotModel ResPlotModel { get; set; }
        private List<LineSeries> LineSeriesAray { get; set; }

        public IList<DataPoint> Points { get; set; }

        private float[] time;
        float[][] load;
        private Rope_t rope;
        private int nodes;
        private string _Estr;
        public string Estr { get => _Estr; set { _Estr = value; float.TryParse(value, out E); } }

        private float E;

        private string _Lstr;
        public string Lstr { get => _Lstr; set { _Lstr = value; float.TryParse(value, out L); } }

        private float L;

        private string _Dstr;
        public string Dstr { get => _Dstr; set { _Dstr = value; float.TryParse(value, out D); } }

        private float D;

        private string _rostr;

        public string rostr
        {
            get => _rostr; set
            {
                if (float.TryParse(value, out ro))
                {
                    _rostr = value;
                }
            }
        }

        private float ro;

        public MainViewModel(IEventAggregator ea)
        {
            _ea = ea;
            LineSeriesAray = new List<LineSeries>();
            Estr = "200E6";
            Lstr = "25E-3";
            Dstr = "1E-3";
            rostr = "1040";
            Points = new List<DataPoint>();
            ResPlotModel = new PlotModel { Title = "Forces in rope" };
            //ResPlotModel.DefaultColors = new List<OxyColor>() { OxyColor.FromRgb(255, 0, 0), OxyColor.FromRgb(255, 255, 0), OxyColor.FromRgb(0, 255, 255) };
            ResPlotModel.InvalidatePlot(true);
            _ea.GetEvent<ComputeEvent>().Subscribe(() => Compute_Click());
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

        private float[][] getLoad(float maxLoad, int nodes, int Counts)
        {
            float[][] tCounts = new float[nodes][];
            for (int node = 0; node < nodes; node++)
            {
                tCounts[node] = new float[Counts];
                if (node == 0)
                {
                    for (int i = 1; i < Counts; i++)
                    {
                        tCounts[node][i] = tCounts[node][i - 1] + maxLoad / Counts;
                    }
                }
            }
            return tCounts;
        }

        private async void Compute_Click()
        {
            Points.Clear();
            LineSeriesAray.Clear();
            ResPlotModel.Series.Clear();
            int counts = 1000;
            float dt = 1E-7f;
            nodes = 6;
            float maxUx = L / nodes / 100 * 2;
            float A = (float)Math.PI * (float)Math.Pow(D, 2) / 4;
            float maxLoad = ((E * A) / L / nodes) * maxUx;
            load = getLoad(maxLoad, nodes, counts);
            time = getT(dt, counts);
            rope = new Rope_t(time, nodes, L, E, D, ro, ref load);
            await Task.Run(Simulating);
        }

        private void Simulating()
        {
            for (int t = 1; t < time.Length; t++)
            {
                rope.IterateOverNodes(t);
            }
            for (int node = 0; node < nodes; node++)
            {
                LineSeries LineSeries1 = new LineSeries();
                LineSeries1.Title = "node #" + node;
                for (int t = 0; t < time.Length; t++)
                {
                    LineSeries1.Points.Add(new DataPoint(time[t], rope.Nodes[node].tm[t][N.f][C.x]));
                }
                LineSeriesAray.Add(LineSeries1);
            }
            foreach (var ls in LineSeriesAray)
            {
                ResPlotModel.Series.Add(ls);
            }
            ResPlotModel.InvalidatePlot(true);
            MessageBox.Show("done");
            GC.Collect();
        }
    }
}