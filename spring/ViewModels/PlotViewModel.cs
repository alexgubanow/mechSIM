using HelixToolkit.Wpf;
using OxyPlot;
using OxyPlot.Series;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace spring.ViewModels
{
    public class DrawPlotEvent : PubSubEvent<DataToDraw> { }
    public class Draw3dPlotEvent : PubSubEvent<float> { }
    public class DrawTlineEvent : PubSubEvent<float> { }
    public class Load3dEvent : PubSubEvent { }
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
            awePlotModelY = new PlotModel { Title = "Y axis" };
            awePlotModelZ = new PlotModel { Title = "Z axis" };
            awePlotModelX.InvalidatePlot(true);
            awePlotModelY.InvalidatePlot(true);
            awePlotModelZ.InvalidatePlot(true);
            _ea.GetEvent<DrawPlotEvent>().Subscribe((value) => DrawPoints(value));
            _ea.GetEvent<Draw3dPlotEvent>().Subscribe((value) => Draw3d(value));
            _ea.GetEvent<ClearPlotsEvent>().Subscribe(() => ClearPlot());
            _ea.GetEvent<Load3dEvent>().Subscribe(() => Load3d());
            //_ea.GetEvent<DrawTlineEvent>().Subscribe((value) => DrawTline(value));
            _ea.GetEvent<DrawTlineEvent>().Subscribe((value) => Position = value);
        }

        public float Position { get; set; }
        public PlotModel awePlotModelX { get; set; }
        public PlotModel awePlotModelY { get; set; }
        public PlotModel awePlotModelZ { get; set; }
        private void ClearPlot()
        {
            awePlotModelX.Series.Clear();
            awePlotModelY.Series.Clear();
            awePlotModelZ.Series.Clear();
        }
        void Load3d()
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
            Objs3d.Add(new CubeVisual3D
            {
                Center = new Point3D(1, 0, 6),
                SideLength = 1,
                Fill = Brushes.Gray
            });
            Objs3d.Add(new CubeVisual3D
            {
                Center = new Point3D(13, 0, 6),
                SideLength = 1,
                Fill = Brushes.Gray
            });
        }
        private void DrawPoints(DataToDraw data)
        {
            LineSeries aweLineSeries = new LineSeries { Title = data.Title };
            aweLineSeries.Points.AddRange(data.getDataPointList(C.x));
            awePlotModelX.Series.Add(aweLineSeries);
            awePlotModelX.InvalidatePlot(true);
            aweLineSeries = new LineSeries { Title = data.Title };
            aweLineSeries.Points.AddRange(data.getDataPointList(C.y));
            awePlotModelY.Series.Add(aweLineSeries);
            awePlotModelY.InvalidatePlot(true);
            aweLineSeries = new LineSeries { Title = data.Title };
            aweLineSeries.Points.AddRange(data.getDataPointList(C.z));
            awePlotModelZ.Series.Add(aweLineSeries);
            awePlotModelZ.InvalidatePlot(true);
        }
        void Clear3d()
        {
            for (int i = Objs3d.Count-1; i > 0; i--)
            {
                if (Objs3d[i].GetType() == typeof(LinesVisual3D))
                {
                    Objs3d.Remove(Objs3d[i]);
                }
            }
        }
        private void Draw3d(float t)
        {
            Application.Current.Dispatcher.Invoke(delegate
            {
                int d = 10;
                Clear3d();
                Objs3d.Add(new LinesVisual3D
                {
                    Points = { new Point3D(1, 0, 6), new Point3D(2, 0, 4) },
                    Thickness = 2,
                    Color = Brushes.Blue.Color
                });
                Objs3d.Add(new SphereVisual3D
                {
                    Center = new Point3D(2, 0, 4),
                    Radius = .5,
                    Fill = Brushes.Black
                });
                Objs3d.Add(new LinesVisual3D
                {
                    Points = { new Point3D(2, 0, 4), new Point3D(4, 0, 2) },
                    Thickness = 2,
                    Color = Brushes.Blue.Color
                });
                Objs3d.Add(new SphereVisual3D
                {
                    Center = new Point3D(4, 0, 2),
                    Radius = .5,
                    Fill = Brushes.Black
                });
                Objs3d.Add(new LinesVisual3D
                {
                    Points = { new Point3D(4, 0, 2), new Point3D(7, 0, 2) },
                    Thickness = 2,
                    Color = Brushes.Blue.Color
                });
                Objs3d.Add(new SphereVisual3D
                {
                    Center = new Point3D(7, 0, 2),
                    Radius = .5,
                    Fill = Brushes.Black
                });
                Objs3d.Add(new LinesVisual3D
                {
                    Points = { new Point3D(7, 0, 2), new Point3D(10, 0, 2) },
                    Thickness = 2,
                    Color = Brushes.Blue.Color
                });
                Objs3d.Add(new SphereVisual3D
                {
                    Center = new Point3D(10, 0, 2),
                    Radius = .5,
                    Fill = Brushes.Black
                });
                Objs3d.Add(new LinesVisual3D
                {
                    Points = { new Point3D(10, 0, 2), new Point3D(12, 0, 4) },
                    Thickness = 2,
                    Color = Brushes.Blue.Color
                });
                Objs3d.Add(new SphereVisual3D
                {
                    Center = new Point3D(12, 0, 4),
                    Radius = .5,
                    Fill = Brushes.Black
                });
                Objs3d.Add(new LinesVisual3D
                {
                    Points = { new Point3D(12, 0, 4), new Point3D(13, 0, 6) },
                    Thickness = 2,
                    Color = Brushes.Blue.Color
                });
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