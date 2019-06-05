﻿using OxyPlot;
using OxyPlot.Series;
using Prism.Events;
using Prism.Mvvm;

namespace spring.ViewModels
{
    public class DrawCoordEvent : PubSubEvent<DataToDraw> { }
    public class CoordPlotViewModel : BindableBase
    {
        private readonly IEventAggregator _ea;

        public CoordPlotViewModel(IEventAggregator ea)
        {
            _ea = ea;
            awePlotModelX = new PlotModel { Title = "Coordinates in X axis of rope" };
            awePlotModelX.InvalidatePlot(true);
            awePlotModelY = new PlotModel { Title = "Coordinates in Y axis of rope" };
            awePlotModelY.InvalidatePlot(true);
            awePlotModelZ = new PlotModel { Title = "Coordinates in Z axis of rope" };
            awePlotModelZ.InvalidatePlot(true);
            _ea.GetEvent<DrawCoordEvent>().Subscribe((value) => DrawPoints(value));
            _ea.GetEvent<ClearPlotsEvent>().Subscribe(() => ClearPlot());
        }

        public PlotModel awePlotModelX { get; set; }
        public PlotModel awePlotModelY { get; set; }
        public PlotModel awePlotModelZ { get; set; }
        private void ClearPlot()
        {
            awePlotModelX.Series.Clear();
            awePlotModelY.Series.Clear();
            awePlotModelZ.Series.Clear();
        }
        private void DrawPoints(DataToDraw data)
        {
            LineSeries aweLineSeries = new LineSeries { Title = data.Title };
            aweLineSeries.Points.AddRange(data.getDataPointList());
            switch (data.axis)
            {
                case C.x:
                    awePlotModelX.Series.Add(aweLineSeries);
                    awePlotModelX.InvalidatePlot(true);
                    break;
                case C.y:
                    awePlotModelY.Series.Add(aweLineSeries);
                    awePlotModelY.InvalidatePlot(true);
                    break;
                case C.z:
                    awePlotModelZ.Series.Add(aweLineSeries);
                    awePlotModelZ.InvalidatePlot(true);
                    break;
                default:
                    break;
            }
        }
    }
}