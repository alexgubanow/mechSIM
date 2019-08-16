using HelixToolkit.Wpf;
using OxyPlot;
using OxyPlot.Series;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace spring.ViewModels
{
    public class DrawPlotEvent : PubSubEvent<DataToDraw> { }
    public class Update3dEvent : PubSubEvent<DataToDraw> { }
    public class RemoveObjEvent : PubSubEvent { }
    public class PlotViewModel : BindableBase
    {
        private readonly IEventAggregator _ea;
        public Point3DCollection RopeCoords { get; set; }
        public ObservableCollection<Visual3D> Objs3d { get; set; }
        public PlotViewModel(IEventAggregator ea)
        {
            _ea = ea;
            Objs3d = new ObservableCollection<Visual3D>();
            awePlotModelX = new PlotModel { Title = "X axis" };
            awePlotModelX.InvalidatePlot(true);
            awePlotModelY = new PlotModel { Title = "Y axis" };
            awePlotModelY.InvalidatePlot(true);
            awePlotModelZ = new PlotModel { Title = "Z axis" };
            awePlotModelZ.InvalidatePlot(true);
            _ea.GetEvent<DrawPlotEvent>().Subscribe((value) => DrawPoints(value));
            _ea.GetEvent<ClearPlotsEvent>().Subscribe(() => ClearPlot());
            _ea.GetEvent<RemoveObjEvent>().Subscribe(() => RemoveObj_Handle());
            RopeCoords = new Point3DCollection
            {
                new Point3D(0, 0, 0),
                new Point3D(3, 0, 8),
                new Point3D(3, 0, 8),
                new Point3D(5, 0, 8)
            };

            Rect3D BoundingBox = new Rect3D(0, 0, 0, 100, 100, 100);
            double TickSize = 3;

            //Objs3d.Add(new DefaultLights());

            //double bbSize = Math.Max(Math.Max(BoundingBox.SizeX, BoundingBox.SizeY), BoundingBox.SizeZ);

            //Objs3d.Add(new GridLinesVisual3D
            //{
            //    Center = new Point3D(0, 0, 0),
            //    Length = BoundingBox.SizeX,
            //    Width = BoundingBox.SizeY,
            //    MinorDistance = TickSize,
            //    MajorDistance = bbSize,
            //    Thickness = bbSize / 1000,
            //    Fill = Brushes.Gray
            //});
            //Objs3d.Add(new CubeVisual3D
            //{
            //    Center = new Point3D(0, 0, 10),
            //    SideLength = 2,
            //    Fill = Brushes.Gray
            //});
            //Objs3d.Add(new CubeVisual3D
            //{
            //    Center = new Point3D(20, 0, 10),
            //    SideLength = 2,
            //    Fill = Brushes.Gray
            //});
            this.Add();
            this.Add();
            Objs3d.Add(new CubeVisual3D
            {
                Center = new Point3D(0, 0, 10),
                SideLength = 2,
                Fill = Brushes.Gray
            });
            Objs3d.Add(new CubeVisual3D
            {
                Center = new Point3D(20, 0, 10),
                SideLength = 2,
                Fill = Brushes.Gray
            });
        }

        public void Add()
        {
            if (this.Objs3d.Count == 0)
            {
                this.Objs3d.Add(new DefaultLights());
            }

            this.Objs3d.Add(new BoxVisual3D {});
        }
        private DelegateCommand _RemoveObj;

        public DelegateCommand RemoveObj => _RemoveObj ?? (_RemoveObj = new DelegateCommand(() => _ea.GetEvent<RemoveObjEvent>().Publish()));
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
                    awePlotModelY.  Series.Add(aweLineSeries);
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
        void RemoveObj_Handle()
        {
            this.Objs3d.RemoveAt(this.Objs3d.Count - 1);
            Objs3d.Add(new CubeVisual3D
            {
                Center = new Point3D(0, 0, 10),
                SideLength = 2,
                Fill = Brushes.Gray
            });
            Objs3d.Add(new CubeVisual3D
            {
                Center = new Point3D(20, 0, 10),
                SideLength = 2,
                Fill = Brushes.Gray
            });
        }
    }
}