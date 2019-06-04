using OxyPlot;
using OxyPlot.Series;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace spring.ViewModels
{
    public class plotsViewModel : BindableBase
    {
        private readonly IEventAggregator _ea;
        public plotsViewModel(IEventAggregator ea)
        {
            _ea = ea;
            //ForceLineSeriesAray = new List<LineSeries>();
            //DisplLineSeriesAray = new List<LineSeries>();
            //AccelLineSeriesAray = new List<LineSeries>();
            //VelLineSeriesAray = new List<LineSeries>();
            //CoordLineSeriesAray = new List<LineSeries>();
            //Points = new List<DataPoint>();
            //ForcePlotModel = new PlotModel { Title = "Forces in rope" };
            //DisplPlotModel = new PlotModel { Title = "Displacements in rope" };
            //AccelPlotModel = new PlotModel { Title = "Accelerations in rope" };
            //VelPlotModel = new PlotModel { Title = "Velocities in rope" };
            //CoordPlotModel = new PlotModel { Title = "Coordinates of rope" };

            //ForcePlotModel.InvalidatePlot(true);
            //DisplPlotModel.InvalidatePlot(true);
            //AccelPlotModel.InvalidatePlot(true);
            //VelPlotModel.InvalidatePlot(true);
            //CoordPlotModel.InvalidatePlot(true);
        }

        //public PlotModel ForcePlotModel { get; set; }
        //public PlotModel DisplPlotModel { get; set; }
        //public PlotModel AccelPlotModel { get; set; }
        //public PlotModel VelPlotModel { get; set; }
        //public PlotModel CoordPlotModel { get; set; }
        //private List<LineSeries> ForceLineSeriesAray { get; set; }
        //private List<LineSeries> DisplLineSeriesAray { get; set; }
        //private List<LineSeries> AccelLineSeriesAray { get; set; }
        //private List<LineSeries> VelLineSeriesAray { get; set; }
        //private List<LineSeries> CoordLineSeriesAray { get; set; }

        //public IList<DataPoint> Points { get; set; }
    }
}
