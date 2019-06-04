using OxyPlot;
using OxyPlot.Series;
using Prism.Events;
using Prism.Mvvm;
using System.Collections.Generic;

namespace spring.ViewModels
{
    public class DrawForceEvent : PubSubEvent<DataToDraw> { }
    public class ForcePlotViewModel : BindableBase
    {
        private readonly IEventAggregator _ea;

        public ForcePlotViewModel(IEventAggregator ea)
        {
            _ea = ea;
            awePlotModel = new PlotModel { Title = "Forces in rope" };
            awePlotModel.InvalidatePlot(true);
            _ea.GetEvent<DrawForceEvent>().Subscribe((value) => DrawPoints(value));
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