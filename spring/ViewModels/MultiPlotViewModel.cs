using HelixToolkit.Wpf;
using HelixToolkit.Wpf.SharpDX;
using mechLIB;
using Microsoft.Win32;
using OxyPlot;
using OxyPlot.Annotations;
using OxyPlot.Series;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace spring.ViewModels
{
    public class CurrTChangedEvent : PubSubEvent<int> { }
    public class Load3dEvent : PubSubEvent { }
    public class MultiPlotViewModel : BindableBase
    {
        private readonly IEventAggregator _ea;
        private DerivativesEnum SelDeriv;

        private int _CurrT;
        public int CurrT { get => _CurrT; set { _CurrT = value; UpdateTimeLinePosition(awePlotModelX);
                UpdateTimeLinePosition(awePlotModelY);
                UpdateTimeLinePosition(awePlotModelZ); Update3dPositions(); } }

        public float[] TimeArr { get { return (float[])Application.Current.Properties["TimeArr"]; } }
        public DerivativesContainerManaged[][] Derivatives 
        { get { return (DerivativesContainerManaged[][])Application.Current.Properties["Derivatives"]; } }
        private ModelProperties ModelProperties 
        { get { return (ModelProperties)Application.Current.Properties["ModelProperties"]; } }
        private bool IsArrayExist => Derivatives != null;

        private PlotModel _awePlotModelX = new PlotModel { Title = "X axis" };
        public PlotModel awePlotModelX
        {
            get { return _awePlotModelX; }
            set { SetProperty(ref _awePlotModelX, value); }
        }

        private PlotModel _awePlotModelY = new PlotModel { Title = "Y axis" };
        public PlotModel awePlotModelY
        {
            get { return _awePlotModelY; }
            set { SetProperty(ref _awePlotModelY, value); }
        }

        private PlotModel _awePlotModelZ = new PlotModel { Title = "Z axis" };
        public PlotModel awePlotModelZ
        {
            get { return _awePlotModelZ; }
            set { SetProperty(ref _awePlotModelZ, value); }
        }
        private HelixToolkit.Wpf.SharpDX.Camera _Camera;
        public HelixToolkit.Wpf.SharpDX.Camera Camera
        {
            get { return _Camera; }
            set { SetProperty(ref _Camera, value); }
        }
        private LineGeometry3D _Lines3d = new LineGeometry3D() { IsDynamic = true };
        public LineGeometry3D Lines3d
        {
            get { return _Lines3d; }
            set { SetProperty(ref _Lines3d, value); }
        }
        private PointGeometry3D _BlackPoints3d = new PointGeometry3D() { IsDynamic = true };
        public PointGeometry3D BlackPoints3d
        {
            get { return _BlackPoints3d; }
            set { SetProperty(ref _BlackPoints3d, value); }
        }
        private PointGeometry3D _RedPoints3d = new PointGeometry3D() { IsDynamic = true };
        public PointGeometry3D RedPoints3d
        {
            get => _RedPoints3d;
            set => SetProperty(ref _RedPoints3d, value);
        }
        private IEffectsManager _EffectsManager;
        public IEffectsManager EffectsManager
        {
            get => _EffectsManager;
            set => SetProperty(ref _EffectsManager, value);
        }
        private int _EndT;
        public int EndT
        {
            get => _EndT;
            set => SetProperty(ref _EndT, value);
        }
        private DelegateCommand<object> _An3dObjectSelectedCMD;
        public DelegateCommand<object> An3dObjectSelectedCMD =>
            _An3dObjectSelectedCMD ?? (_An3dObjectSelectedCMD = new DelegateCommand<object>(ExecuteAn3dObjectSelectedCMD));

        void ExecuteAn3dObjectSelectedCMD(object sender)
        {
            var viewport = sender as Viewport3DX;
            MessageBox.Show("dcs");
        }
        private readonly SynchronizationContext context = SynchronizationContext.Current;
        public MultiPlotViewModel(IEventAggregator ea)
        {
            EffectsManager = new DefaultEffectsManager();
            Camera = new HelixToolkit.Wpf.SharpDX.PerspectiveCamera
            {
                Position = new Point3D(0, 0, 40),
                LookDirection = new Vector3D(0, 0, -40),
                UpDirection = new Vector3D(0, 1, 0)
            };
            _ea = ea;
            _ea.GetEvent<GotResultsEvent>().Subscribe(() => { ShowResults(); EndT = TimeArr.Length - 1; });
            _ea.GetEvent<ComputeIsStartedEvent>().Subscribe(() => ClearDataView());
            _ea.GetEvent<SelDerivChangedEvent>().Subscribe((var) => { SelDeriv = var; if (IsArrayExist) { DrawPlots(); } });
            UpdateTimeLinePosition(awePlotModelX);
            UpdateTimeLinePosition(awePlotModelY);
            UpdateTimeLinePosition(awePlotModelZ);
        }

        private void UpdateTimeLinePosition(PlotModel plotModel)
        {
            if (TimeArr != null && plotModel.DefaultYAxis != null)
            {
                if (plotModel.Annotations.Count > 0)
                {
                    (plotModel.Annotations[0] as LineAnnotation).MinimumY = plotModel.DefaultYAxis.ActualMinimum * 2;
                    (plotModel.Annotations[0] as LineAnnotation).MaximumY = plotModel.DefaultYAxis.ActualMaximum * 2;
                    (plotModel.Annotations[0] as LineAnnotation).X = TimeArr[CurrT];
                }
                else
                {
                    var annotation = new LineAnnotation
                    {
                        Color = OxyColors.Blue,
                        MinimumY = awePlotModelX.DefaultYAxis.ActualMinimum * 2,
                        MaximumY = awePlotModelX.DefaultYAxis.ActualMaximum * 2,
                        X = TimeArr[CurrT],
                        LineStyle = LineStyle.Solid,
                        Type = LineAnnotationType.Vertical
                    };
                    plotModel.Annotations.Add(annotation);
                }
                plotModel.InvalidatePlot(true);
            }
        }

        private void ClearDataView()
        {
            CurrT = 0;
            awePlotModelX.Series.Clear();
            awePlotModelY.Series.Clear();
            awePlotModelZ.Series.Clear();
        }
        private void ShowResults()
        {
            if (IsArrayExist)
            {
                DrawPlots();
                Draw3d();
            }
        }
        private void DrawPlots()
        {
            ClearDataView();
            for (int n = 0; n < Derivatives.Length; n++)
            {
                DataPoint[] XdataPoints = new DataPoint[Derivatives[n].Length];
                DataPoint[] YdataPoints = new DataPoint[Derivatives[n].Length];
                DataPoint[] ZdataPoints = new DataPoint[Derivatives[n].Length];
                switch (SelDeriv)
                {
                    case DerivativesEnum.f:
                        Parallel.For(0, Derivatives[n].Length,
                            t =>
                            {
                                XdataPoints[t] = new DataPoint(TimeArr[t], Derivatives[n][t].f.x);
                                YdataPoints[t] = new DataPoint(TimeArr[t], Derivatives[n][t].f.y);
                                ZdataPoints[t] = new DataPoint(TimeArr[t], Derivatives[n][t].f.z);
                            });
                        break;
                    case DerivativesEnum.p:
                        Parallel.For(0, Derivatives[n].Length,
                            t =>
                            {
                                XdataPoints[t] = new DataPoint(TimeArr[t], Derivatives[n][t].p.x);
                                YdataPoints[t] = new DataPoint(TimeArr[t], Derivatives[n][t].p.y);
                                ZdataPoints[t] = new DataPoint(TimeArr[t], Derivatives[n][t].p.z);
                            });
                        break;
                    case DerivativesEnum.u:
                        Parallel.For(0, Derivatives[n].Length,
                            t =>
                            {
                                XdataPoints[t] = new DataPoint(TimeArr[t], Derivatives[n][t].u.x);
                                YdataPoints[t] = new DataPoint(TimeArr[t], Derivatives[n][t].u.y);
                                ZdataPoints[t] = new DataPoint(TimeArr[t], Derivatives[n][t].u.z);
                            });
                        break;
                    case DerivativesEnum.v:
                        Parallel.For(0, Derivatives[n].Length,
                            t =>
                            {
                                XdataPoints[t] = new DataPoint(TimeArr[t], Derivatives[n][t].v.x);
                                YdataPoints[t] = new DataPoint(TimeArr[t], Derivatives[n][t].v.y);
                                ZdataPoints[t] = new DataPoint(TimeArr[t], Derivatives[n][t].v.z);
                            });
                        break;
                    case DerivativesEnum.a:
                        Parallel.For(0, Derivatives[n].Length,
                            t =>
                            {
                                XdataPoints[t] = new DataPoint(TimeArr[t], Derivatives[n][t].a.x);
                                YdataPoints[t] = new DataPoint(TimeArr[t], Derivatives[n][t].a.y);
                                ZdataPoints[t] = new DataPoint(TimeArr[t], Derivatives[n][t].a.z);
                            });
                        break;
                }
                LineSeries aweLineSeriesX = new LineSeries { Title = "node #" + n };
                aweLineSeriesX.Points.AddRange(XdataPoints);
                awePlotModelX.Series.Add(aweLineSeriesX);
                LineSeries aweLineSeriesY = new LineSeries { Title = "node #" + n };
                aweLineSeriesY.Points.AddRange(YdataPoints);
                awePlotModelY.Series.Add(aweLineSeriesY);
                LineSeries aweLineSeriesZ = new LineSeries { Title = "node #" + n };
                aweLineSeriesZ.Points.AddRange(ZdataPoints);
                awePlotModelZ.Series.Add(aweLineSeriesZ);
            }
            awePlotModelX.InvalidatePlot(true);
            awePlotModelY.InvalidatePlot(true);
            awePlotModelZ.InvalidatePlot(true);
        }
        private void Draw3d()
        {
            if (Derivatives != null && Derivatives.Length > 0)
            {
                Vector3Collection NodePositions = GetNodePositions(0);
                Vector3Collection ElementPositions = GetElementPositions(0);
                var indices = new IntCollection(ElementPositions.Count * 2);
                for (int i = 0; i < ElementPositions.Count - 1; ++i)
                {
                    indices.Add(i);
                    indices.Add(i + 1);
                }
                Lines3d.Indices = indices;
                var RedPositions = new Vector3Collection
                {
                    new Vector3(Derivatives[0][CurrT].p.x * 10E2f,
                                        Derivatives[0][CurrT].p.y * 10E2f,
                                        Derivatives[0][CurrT].p.z * 10E2f),
                    new Vector3(Derivatives[Derivatives.Length - 1][CurrT].p.x * 10E2f,
                                        Derivatives[Derivatives.Length - 1][CurrT].p.y * 10E2f,
                                        Derivatives[Derivatives.Length - 1][CurrT].p.z * 10E2f)
                };
                var l = ModelProperties.L * 1000;
                context.Send((o) =>
                {
                    Lines3d.Positions = ElementPositions;
                    BlackPoints3d.Positions = NodePositions;
                    RedPoints3d.Positions = RedPositions;
                    Camera.Position = new Point3D(l / 2, 0, l * 2);
                    Camera.LookDirection = new Vector3D(0, 0, -(l * 2));
                    Camera.UpDirection = new Vector3D(0, 1, 0);
                }, null);
            }
        }
        private Vector3Collection GetElementPositions(int timeMoment)
        {
            var Positions = new Vector3Collection();
            for (int node = 0; node < Derivatives.Length - 1; node++)
            {
                Positions.Add(new Vector3(Derivatives[node][timeMoment].p.x * 10E2f,
                                        Derivatives[node][timeMoment].p.y * 10E2f,
                                        Derivatives[node][timeMoment].p.z * 10E2f));
                Positions.Add(new Vector3(Derivatives[node + 1][timeMoment].p.x * 10E2f,
                                        Derivatives[node + 1][timeMoment].p.y * 10E2f,
                                        Derivatives[node + 1][timeMoment].p.z * 10E2f));
            }
            return Positions;
        }
        private Vector3Collection GetNodePositions(int timeMoment)
        {
            var Positions = new Vector3Collection();
            for (int node = 1; node < Derivatives.Length -1; node++)
            {
                Positions.Add(new Vector3(Derivatives[node][timeMoment].p.x * 10E2f,
                                        Derivatives[node][timeMoment].p.y * 10E2f,
                                        Derivatives[node][timeMoment].p.z * 10E2f));
            }
            return Positions;
        }
        private void Update3dPositions()
        {
            if (Derivatives != null && Derivatives.Length > 0 && Lines3d.Positions != null)
            {
                var RedPositions = new Vector3Collection
                {
                    new Vector3(Derivatives[0][CurrT].p.x * 10E2f,
                                        Derivatives[0][CurrT].p.y * 10E2f,
                                        Derivatives[0][CurrT].p.z * 10E2f),
                    new Vector3(Derivatives[Derivatives.Length - 1][CurrT].p.x * 10E2f,
                                        Derivatives[Derivatives.Length - 1][CurrT].p.y * 10E2f,
                                        Derivatives[Derivatives.Length - 1][CurrT].p.z * 10E2f)
                };
                Vector3Collection NodePositions = GetNodePositions(CurrT);
                Vector3Collection ElementPositions = GetElementPositions(CurrT);
                context.Send((o) =>
                {
                    Lines3d.Positions = ElementPositions;
                    BlackPoints3d.Positions = NodePositions;
                    RedPoints3d.Positions = RedPositions;
                }, null);
            }
        }
    }
}