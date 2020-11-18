using HelixToolkit.Wpf;
using mechLIB;
using Microsoft.Win32;
using OxyPlot;
using OxyPlot.Series;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
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
        public int CurrT { get => _CurrT; set { _CurrT = value; Draw3d(value, SelDeriv); } }

        public Point3DCollection RopeCoords { get; set; }
        public ObservableCollection<Visual3D> Objs3d { get; set; }

        public float[] TimeArr { get { return (float[])Application.Current.Properties["TimeArr"]; } }
        public DerivativesContainerManaged[][] Derivatives { get { return (DerivativesContainerManaged[][])Application.Current.Properties["Derivatives"]; } }
        private bool IsArrayExist => Derivatives != null;
        public PlotModel awePlotModelX { get; set; }
        public PlotModel awePlotModelY { get; set; }
        public PlotModel awePlotModelZ { get; set; }
        private int _EndT;
        public int EndT
        {
            get => _EndT;
            set => SetProperty(ref _EndT, value);
        }
        public MultiPlotViewModel(IEventAggregator ea)
        {
            _ea = ea;
            _CurrT = 0;
            EndT = 1;
            Objs3d = new ObservableCollection<Visual3D>();
            awePlotModelX = new PlotModel { Title = "X axis" };
            awePlotModelY = new PlotModel { Title = "Y axis" };
            awePlotModelZ = new PlotModel { Title = "Z axis" };
            awePlotModelX.InvalidatePlot(true);
            awePlotModelY.InvalidatePlot(true);
            awePlotModelZ.InvalidatePlot(true);
            _ea.GetEvent<GotResultsEvent>().Subscribe(() => { ShowResults(SelDeriv); EndT = TimeArr.Length - 1; });
            _ea.GetEvent<ComputeIsStartedEvent>().Subscribe(() => ClearDataView());
            _ea.GetEvent<SelDerivChangedEvent>().Subscribe((var) => { if (IsArrayExist) { DrawPoints(var); } });
            _ea.GetEvent<SelDerivChangedEvent>().Subscribe((var) => SelDeriv = var );
        }

        private void ClearDataView()
        {
            CurrT = 0;
            awePlotModelX.Series.Clear();
            awePlotModelY.Series.Clear();
            awePlotModelZ.Series.Clear();
        }

        private void Load3d()
        {
            Objs3d.Add(new DefaultLights());
        }
        private void ShowResults(DerivativesEnum Deriv)
        {
            if (IsArrayExist)
            {
                DrawPoints(Deriv);
                Draw3d(CurrT, Deriv);
            }
        }
        private void DrawPoints(DerivativesEnum Deriv)
        {
            ClearDataView();
            for (int n = 0; n < Derivatives.Length; n++)
            {
                DataPoint[] XdataPoints = new DataPoint[Derivatives[n].Length];
                DataPoint[] YdataPoints = new DataPoint[Derivatives[n].Length];
                DataPoint[] ZdataPoints = new DataPoint[Derivatives[n].Length];
                switch (Deriv)
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
        private void Draw3d(int t, DerivativesEnum Deriv)
        {
            if (Derivatives != null && Derivatives.Length > 0)
            {
                Application.Current.Dispatcher.Invoke(delegate
                {
                    Objs3d.Clear();
                    Load3d();
                    Objs3d.Add(new CubeVisual3D
                    {
                        Center = new Point3D(Derivatives[0][t].p.x * 10E2,
                                             Derivatives[0][t].p.z * 10E2,
                                             Derivatives[0][t].p.y * 10E2),
                        SideLength = .8,
                        Fill = Brushes.Gray
                    });
                    Objs3d.Add(new CubeVisual3D
                    {
                        Center = new Point3D(Derivatives[Derivatives.Length - 1][t].p.x * 10E2,
                                             Derivatives[Derivatives.Length - 1][t].p.z * 10E2,
                                             Derivatives[Derivatives.Length - 1][t].p.y * 10E2),
                        SideLength = .8,
                        Fill = Brushes.Gray
                    });
                    for (int node = 0; node < Derivatives.Length - 1; node++)
                    {
                        Objs3d.Add(new LinesVisual3D
                        {
                            Points = { new Point3D(Derivatives[node][t].p.x * 10E2,
                                                Derivatives[node][t].p.z * 10E2,
                                                Derivatives[node][t].p.y * 10E2),
                            new Point3D(Derivatives[node + 1][t].p.x * 10E2,
                                        Derivatives[node + 1][t].p.z * 10E2,
                                        Derivatives[node + 1][t].p.y * 10E2) },
                            Thickness = 2,
                            Color = Brushes.Blue.Color
                        });
                        Objs3d.Add(new SphereVisual3D
                        {
                            Center = new Point3D(Derivatives[node][t].p.x * 10E2,
                                                Derivatives[node][t].p.z * 10E2,
                                                Derivatives[node][t].p.y * 10E2),
                            Radius = .3,
                            Fill = Brushes.Black
                        });
                    }
                });
            }
        }
    }
}