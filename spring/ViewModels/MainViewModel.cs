using OxyPlot;
using OxyPlot.Series;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
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

        private float[] getT(float dt, int Counts)
        {
            float[] tCounts = new float[Counts];
            for (int i = 1; i < Counts; i++)
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
            int counts = 100;
            float E = 200E6f;
            float dt = 1E-7f;
            float D = 0.001f;
            float ro = 1040f;
            float L = 0.025f;
            int nodes = 6;
            float[] time = getT(dt, counts);
            Rope_t rope = new Rope_t(time, nodes, E, ro);
            rope.Sim();

            //for (int i = 0; i < rope.nds[0].F.Length; i++)
            //{
            //    Points.Add(new DataPoint(tCounts[i], rope.nds[0].a[i][0]));
            //}
            //LineSeries1.Points.AddRange(Points);
            //Points.Clear();
            for (int i = 0; i < rope.nds[1].F.Length; i++)
            {
                Points.Add(new DataPoint(time[i], rope.nds[1].a[i][0]));
            }
            LineSeries2.Points.AddRange(Points);
            Points.Clear();
            for (int i = 0; i < rope.nds[2].F.Length; i++)
            {
                Points.Add(new DataPoint(time[i], rope.nds[2].a[i][0]));
            }
            LineSeries3.Points.AddRange(Points);
            Points.Clear();
            for (int i = 0; i < rope.nds[3].F.Length; i++)
            {
                Points.Add(new DataPoint(time[i], rope.nds[3].a[i][0]));
            }
            LineSeries4.Points.AddRange(Points);
            ResPlotModel.InvalidatePlot(true);
        }

    }

}