using OxyPlot;
using OxyPlot.Series;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;

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
        private List<LineSeries> ListPointAray { get; set; }

        public IList<DataPoint> Points { get; set; }
        public MainViewModel(IEventAggregator ea)
        {
            _ea = ea;
            Points = new List<DataPoint>();
            ResPlotModel = new PlotModel { Title = "Error against reference encoder" };
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
            _ea.GetEvent<GetITEvent>().Subscribe(() => GetF_Click());
        }

        private DelegateCommand _GetIT;
        public DelegateCommand GetIT => _GetIT ?? (_GetIT = new DelegateCommand(() => _ea.GetEvent<GetITEvent>().Publish()));

        private float[] getT(float dt, float tmax)
        {
            int maxI = (int)(tmax / dt);
            float[] tCounts = new float[maxI];
            for (int i = 1; i < maxI; i++)
            {
                tCounts[i] = tCounts[i - 1] + dt;
            }
            return tCounts;
        }
        private void GetF_Click()
        {
            Points.Clear();
            LineSeries1.Points.Clear();
            LineSeries2.Points.Clear();
            LineSeries3.Points.Clear();
            LineSeries4.Points.Clear();
            float tmax = 0.9f;
            float dt = 0.00005f;
            float[] tCounts = getT(dt, tmax);
            Rope_t rope = new Rope_t(dt, tCounts.Length, 4);
            rope.Sim();

            //for (int i = 0; i < rope.nds[0].F.Length; i++)
            //{
            //    Points.Add(new DataPoint(tCounts[i], rope.nds[0].a[i][0]));
            //}
            //LineSeries1.Points.AddRange(Points);
            //Points.Clear();
            for (int i = 0; i < rope.nds[1].F.Length; i++)
            {
                Points.Add(new DataPoint(tCounts[i], rope.nds[1].a[i][0]));
            }
            LineSeries2.Points.AddRange(Points);
            Points.Clear();
            for (int i = 0; i < rope.nds[2].F.Length; i++)
            {
                Points.Add(new DataPoint(tCounts[i], rope.nds[2].a[i][0]));
            }
            LineSeries3.Points.AddRange(Points);
            Points.Clear();
            for (int i = 0; i < rope.nds[3].F.Length; i++)
            {
                Points.Add(new DataPoint(tCounts[i], rope.nds[3].a[i][0]));
            }
            LineSeries4.Points.AddRange(Points);
            ResPlotModel.InvalidatePlot(true);
        }

    }

}