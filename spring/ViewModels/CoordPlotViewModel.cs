using OxyPlot;
using OxyPlot.Series;
using Prism.Events;
using Prism.Mvvm;
using System.Collections.Generic;

namespace spring.ViewModels
{
    public class DrawCoordEvent : PubSubEvent<DataToDraw> { }
    public class CoordPlotViewModel : BindableBase
    {
        private readonly IEventAggregator _ea;

        public CoordPlotViewModel(IEventAggregator ea)
        {
            _ea = ea;
            awePlotModel = new PlotModel { Title = "Coordinates of rope" };
            awePlotModel.InvalidatePlot(true);
            _ea.GetEvent<DrawCoordEvent>().Subscribe((value) => DrawPoints(value));
            _ea.GetEvent<ClearPlotsEvent>().Subscribe(() => ClearPlot());
        }

        public PlotModel awePlotModel { get; set; }
        private void ClearPlot()
        {
            awePlotModel.Series.Clear();
        }

        private void DrawPoints(DataToDraw data)
        {
            LineSeries aweLineSeries = new LineSeries { Title = data.Title  };
            aweLineSeries.Points.AddRange(data.getDataPointList());
            awePlotModel.Series.Add(aweLineSeries);
            awePlotModel.InvalidatePlot(true);
        }
    }
}