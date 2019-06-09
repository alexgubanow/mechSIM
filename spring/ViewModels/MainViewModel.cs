﻿using Microsoft.Win32;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.IO;
using System.Threading.Tasks;

namespace spring.ViewModels
{
    public class MainViewModel : BindableBase
    {
        private readonly IEventAggregator _ea;

        private float[] time;
        private float[][][] load;
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

        private float[][][] getLoad(NodeLoad ltype, C axis, int nodes, int Counts)
        {
            float[][][] tCounts = new float[nodes][][];
            for (int node = 0; node < nodes; node++)
            {
                float maxUx = 0.01f * L / nodeCount / 100;
                float A = (float)Math.PI * (float)Math.Pow(D, 2) / 4;
                float maxLoad = ((E * A) / L / nodeCount) * maxUx;
                if (node == nodes /2)
                {
                    tCounts[node] = new float[Counts][];
                    tCounts[node][0] = new float[3];
                    for (int t = 1; t < Counts; t++)
                    {
                        tCounts[node][t] = new float[3];
                        switch (ltype)
                        {
                            case NodeLoad.u:
                                maxUx = 0.01f * L / nodeCount / 100;
                                float ut = (float)Math.Sin(2 * Math.PI * 0.5 * time[t]) * maxUx;
                                //float ut = (maxUx / (0 - t)) + maxUx;
                                tCounts[node][t][(int)axis] = ut;
                                //tCounts[node][t][0] = ((E * A) / L / nodeCount) * ut;
                                break;

                            case NodeLoad.a:
                                break;

                            case NodeLoad.f:
                                //(maxUx / (0 - t)) + maxUx
                                tCounts[node][t][(int)axis] = tCounts[node][t - 1][(int)axis] + (maxLoad / (0 - t)) + maxLoad;
                                //tCounts[node][t][0] = tCounts[node][t - 1][0] + maxLoad / Counts;
                                break;

                            case NodeLoad.p:
                                break;

                            case NodeLoad.none:
                                break;

                            default:
                                break;
                        }
                    }
                }
            }
            return tCounts;
        }

        private async void Compute_Click()
        {
            _ea.GetEvent<ClearPlotsEvent>().Publish();
            time = getT(dt, Counts);
            load = getLoad(NodeLoad.f, C.y, nodeCount, Counts);
            //OpenFileDialog openFileDialog = new OpenFileDialog();
            //if (openFileDialog.ShowDialog() == true)
            //{
            //    string fileName = openFileDialog.FileName;
            //    // deserialize JSON directly from a file
            //    using (StreamReader file = File.OpenText(fileName))
            //    {
            //        JsonSerializer serializer = new JsonSerializer();
            //        Load ld = (Load)serializer.Deserialize(file, typeof(Load));
            //        load = new float[nodeCount][][];
            //        load[nodeCount - 1] = new float[Counts][];
            //        for (int i = 0; i < Counts; i++)
            //        {
            //            load[nodeCount - 1][i] = new float[3];
            //            load[nodeCount - 1][i][0] = ld.x[i];
            //            load[nodeCount - 1][i][1] = ld.y[i];
            //        }
            //    }
            //    ////txtOutput.Text = txtOutput.Text + "Attempting to read the file '" + fileName + "'...";
            //    //try
            //    //{

                    
            //    //}
            //    //catch (Exception)
            //    //{
            //    //    throw new Exception();
            //    //    //txtOutput.Text = txtOutput.Text + "Invalid MAT-file!\n";
            //    //    //MessageBox.Show("Invalid binary MAT-file! Please select a valid binary MAT-file.",
            //    //    //    "Invalid MAT-file", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            //    //}
                    
            //}
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
        private float[] ExtractArray(float[][] tm, C axis)
        {
            float[] tmp = new float[time.Length];
            for (int t = 0; t < time.Length; t++)
            {
                tmp[t] = tm[t][(int)axis];
            }

            return tmp;
        }

        private void DrawPoints()
        {
            float[] fdv = ExtractArray(load[rope.Nodes.Length /2 ], C.x);
            _ea.GetEvent<DrawForceEvent>().Publish(new DataToDraw() { X = time, Y = fdv, Title = "Fext" });
            foreach (var node in rope.Nodes)
            {
                foreach (C ax in Enum.GetValues(typeof(C)))
                {
                    float[] tmp = ExtractArray(node.tm, N.f, ax);
                    _ea.GetEvent<DrawForceEvent>().Publish(new DataToDraw() { X = time, Y = tmp, Title = "node #" + node.NodeID, axis = ax });
                    tmp = null;
                    tmp = ExtractArray(node.tm, N.u, ax);
                    _ea.GetEvent<DrawDisplEvent>().Publish(new DataToDraw() { X = time, Y = tmp, Title = "node #" + node.NodeID, axis = ax });
                    tmp = null;
                    tmp = ExtractArray(node.tm, N.v, ax);
                    _ea.GetEvent<DrawVelEvent>().Publish(new DataToDraw() { X = time, Y = tmp, Title = "node #" + node.NodeID, axis = ax });
                    tmp = null;
                    tmp = ExtractArray(node.tm, N.a, ax);
                    _ea.GetEvent<DrawAccelEvent>().Publish(new DataToDraw() { X = time, Y = tmp, Title = "node #" + node.NodeID, axis = ax });
                    tmp = null;
                    tmp = ExtractArray(node.tm, N.p, ax);
                    _ea.GetEvent<DrawCoordEvent>().Publish(new DataToDraw() { X = time, Y = tmp, Title = "node #" + node.NodeID, axis = ax });
                }
            }
        }
    }
}