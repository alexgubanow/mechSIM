using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Threading.Tasks;

namespace spring.ViewModels
{
    public class MainViewModel : BindableBase
    {
        private readonly IEventAggregator _ea;

        private float[] time;
        private float[][] load;
        private Rope_t rope;

        public int nodeCount { get; set; }
        public float E { get; set; }
        public float L { get; set; }
        public float D { get; set; }
        public int Counts { get; set; }
        public float dt { get; set; }
        public float ro { get; set; }

        public MainViewModel(IEventAggregator ea)
        {
            _ea = ea;
            _ea.GetEvent<ComputeEvent>().Subscribe(() => Compute_Click());
            _ea.GetEvent<NodesChangedEvent>().Subscribe((value) => nodeCount = value);
            _ea.GetEvent<EChangedEvent>().Subscribe((value) => E = value);
            _ea.GetEvent<LChangedEvent>().Subscribe((value) => L = value);
            _ea.GetEvent<DChangedEvent>().Subscribe((value) => D = value);
            _ea.GetEvent<CountsChangedEvent>().Subscribe((value) => Counts = value);
            _ea.GetEvent<dtChangedEvent>().Subscribe((value) => dt = value);
            _ea.GetEvent<roChangedEvent>().Subscribe((value) => ro = value);
        }

        private DelegateCommand _Compute;
        public DelegateCommand Compute => _Compute ?? (_Compute = new DelegateCommand(() => _ea.GetEvent<ComputeEvent>().Publish()));

        private float[] getT(float dt, int Counts)
        {
            float[] tCounts = new float[Counts];
            for (int i = 1; i < Counts; i++)
            {
                tCounts[i] = tCounts[i - 1] + dt;
            }
            return tCounts;
        }

        private float[][] getLoad(float maxLoad, int nodes, int Counts)
        {
            float[][] tCounts = new float[nodes][];
            for (int node = 0; node < nodes; node++)
            {
                tCounts[node] = new float[Counts];
                if (node == (int)nodes / 2)
                {
                    for (int i = 1; i < Counts; i++)
                    {
                        tCounts[node][i] = tCounts[node][i - 1] + maxLoad / Counts;
                    }
                }
            }
            return tCounts;
        }

        private async void Compute_Click()
        {
            _ea.GetEvent<ClearPlotsEvent>().Publish();
            float maxUx = 0.05f * L / nodeCount / 100;
            float A = (float)Math.PI * (float)Math.Pow(D, 2) / 4;
            float maxLoad = ((E * A) / L / nodeCount) * maxUx;
            load = getLoad(maxLoad, nodeCount, Counts);
            time = getT(dt, Counts);
            rope = new Rope_t(time, nodeCount, L, E, D, ro, ref load);
            await Task.Run(Simulating);
        }

        private void Simulating()
        {
            for (int t = 1; t < time.Length; t++)
            {
                rope.IterateOverNodes(t);
            }
            DrawPoints();
            GC.Collect();
        }
        private float[] ExtractArray(float[][][] tm, N deriv, C axis)
        {
            float[] tmp = new float[time.Length];
            for (int t = 0; t < time.Length; t++)
            {
                tmp[t] = tm[t][(int)deriv][(int)axis];
            }

            return tmp;
        }
        private void DrawPoints()
        {
            _ea.GetEvent<DrawForceEvent>().Publish(new DataToDraw() { X = time, Y = load[0], Title = "Fext" });
            foreach (var node in rope.Nodes)
            {
                float[] tmp = ExtractArray(node.tm, N.f, C.x);
                _ea.GetEvent<DrawForceEvent>().Publish(new DataToDraw() { X = time, Y = tmp, Title = "node #" + node.NodeID });
                tmp = null;
                tmp = ExtractArray(node.tm, N.u, C.x);
                _ea.GetEvent<DrawDisplEvent>().Publish(new DataToDraw() { X = time, Y = tmp, Title = "node #" + node.NodeID });
                tmp = null;
                tmp = ExtractArray(node.tm, N.v, C.x);
                _ea.GetEvent<DrawVelEvent>().Publish(new DataToDraw() { X = time, Y = tmp, Title = "node #" + node.NodeID });
                tmp = null;
                tmp = ExtractArray(node.tm, N.a, C.x);
                _ea.GetEvent<DrawAccelEvent>().Publish(new DataToDraw() { X = time, Y = tmp, Title = "node #" + node.NodeID });
                tmp = null;
                tmp = ExtractArray(node.tm, N.p, C.x);
                _ea.GetEvent<DrawCoordEvent>().Publish(new DataToDraw() { X = time, Y = tmp, Title = "node #" + node.NodeID });
            }
        }
    }
}