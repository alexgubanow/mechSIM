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

        public LineSeries LineSeries1 { get; set; }
        public LineSeries LineSeries2 { get; set; }
        public LineSeries LineSeries3 { get; set; }
        public LineSeries LineSeries4 { get; set; }

        public IList<DataPoint> Points { get; set; }

        private float[] time;
        private Rope_t rope;

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
            Estr = "200E6";
            Lstr = "25E-3";
            Dstr = "1E-3";
            rostr = "1040";
            Points = new List<DataPoint>();
            ResPlotModel = new PlotModel { Title = "Forces in rope" };
            //ResPlotModel.DefaultColors = new List<OxyColor>() { OxyColor.FromRgb(255, 0, 0), OxyColor.FromRgb(255, 255, 0), OxyColor.FromRgb(0, 255, 255) };
            LineSeries1 = new LineSeries();
            LineSeries1.Title = "1";
            LineSeries2 = new LineSeries();
            LineSeries2.Title = "2";
            LineSeries3 = new LineSeries();
            LineSeries3.Title = "3";
            LineSeries4 = new LineSeries();
            LineSeries4.Title = "4";
            //Points.Add(new DataPoint(0, 0));
            //Points.Add(new DataPoint(10, 10));
            //Points.Add(new DataPoint(50, 5));
            //LineSeries1.Points.AddRange(Points);
            ResPlotModel.Series.Add(LineSeries1);
            ResPlotModel.Series.Add(LineSeries2);
            ResPlotModel.Series.Add(LineSeries3);
            ResPlotModel.Series.Add(LineSeries4);
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

        private async void Compute_Click()
        {
            Points.Clear();
            LineSeries1.Points.Clear();
            LineSeries2.Points.Clear();
            LineSeries3.Points.Clear();
            LineSeries4.Points.Clear();
            int counts = 10000;
            //float E = 200E6f;
            float dt = 1E-7f;
            //float D = 0.001f;
            //float ro = 1040f;
            //float L = 0.025f;
            int nodes = 6;
            time = getT(dt, counts);
            rope = new Rope_t(time, nodes, L, E, D, ro);
            await Task.Run(Simulating);
            //for (int i = 0; i < rope.nds[0].F.Length; i++)
            //{
            //    Points.Add(new DataPoint(tCounts[i], rope.nds[0].a[i][0]));
            //}
            //LineSeries1.Points.AddRange(Points);
            //Points.Clear();
            //for (int i = 0; i < rope.Nodes[1].F.Length; i++)
            //{
            //    Points.Add(new DataPoint(time[i], rope.Nodes[1].a[i][0]));
            //}
            //LineSeries2.Points.AddRange(Points);
            //Points.Clear();
            //for (int i = 0; i < rope.Nodes[2].F.Length; i++)
            //{
            //    Points.Add(new DataPoint(time[i], rope.Nodes[2].a[i][0]));
            //}
            //LineSeries3.Points.AddRange(Points);
            //Points.Clear();
            //for (int i = 0; i < rope.Nodes[3].F.Length; i++)
            //{
            //    Points.Add(new DataPoint(time[i], rope.Nodes[3].a[i][0]));
            //}
            //LineSeries4.Points.AddRange(Points);
            //ResPlotModel.InvalidatePlot(true);
        }

        private void Simulating()
        {
            for (int t = 1; t < time.Length; t++)
            {
                rope.IterateOverNodes(t);
            }
            MessageBox.Show("done");
            GC.Collect();
        }
    }
}