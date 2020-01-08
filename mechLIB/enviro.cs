using csmatio.io;
using csmatio.types;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mechLIB
{
    public class Enviro
    {
        public props_t phProps;
        public float[] Re;
        public float[] bloodV;
        public float[] bloodP;
        public float[] time;
        public Rope_t rope;
        public Enviro()
        { }
        public void PrepRun(props_t propStore, string loadFile)
        {
            phProps = propStore;
            if (loadFile.Length > 0)
            {
                ReadLoadFile(loadFile);
            }
            else
            {
                allocateTime(phProps.dt, phProps.Counts);
                rope = new Rope_t(phProps);
                GenerateLoad(C_t.x);
            }
        }

        private void ReadLoadFile(string loadFile)
        {

            MatFileReader mfr = new MatFileReader(loadFile);
            // now get the double values 
            time = (mfr.Content["tq"] as MLSingle).GetArray()[0];
            phProps.Counts = time.Length;
            float[] pmx = (mfr.Content["pmxq"] as MLSingle).GetArray()[0];
            float[] plx = (mfr.Content["plxq"] as MLSingle).GetArray()[0];
            float[] pmy = (mfr.Content["pmyq"] as MLSingle).GetArray()[0];
            float[] ply = (mfr.Content["plyq"] as MLSingle).GetArray()[0];

            rope = new Rope_t(phProps);
            //fill load to rope
            Re = new float[phProps.Counts];
            bloodV = new float[phProps.Counts];
            bloodP = new float[phProps.Counts];
            for (int t = 0; t < phProps.Counts; t++)
            {
                Re[t] = 0;
                bloodV[t] = 0;
                bloodP[t] = 0;
                rope.Nodes[0].deriv[t].p.x = rope.Nodes[0].deriv[0].p.x + pmx[t];
                rope.Nodes[0].deriv[t].p.y = rope.Nodes[0].deriv[0].p.y + pmy[t];

                int lastN = phProps.nodes - 1;
                rope.Nodes[lastN].deriv[t].p.x = rope.Nodes[lastN].deriv[0].p.x + plx[t];
                rope.Nodes[lastN].deriv[t].p.y = rope.Nodes[lastN].deriv[0].p.y + ply[t];
            }
            //choose load nodes
            rope.Nodes[0].LoadType = NodeLoad.p;
            rope.Nodes[phProps.nodes - 1].LoadType = NodeLoad.p;
        }

        private void allocateTime(float dt, int Counts)
        {
            time = new float[Counts];
            for (int i = 1; i < Counts; i++)
            {
                time[i] = time[i - 1] + dt;
            }
        }
        private void GenerateLoad(C_t axis)
        {
            Re = new float[phProps.Counts];
            bloodV = new float[phProps.Counts];
            bloodP = new float[phProps.Counts];
            float A = (float)Math.PI * (float)Math.Pow(phProps.D, 2) / 4;
            float maxLoad = ((phProps.E * A) / phProps.L / phProps.nodes) * phProps.MaxU;
            float freq = 1 / (phProps.Counts * phProps.dt);
            rope.Nodes[0].LoadType = NodeLoad.p;
            rope.Nodes[phProps.nodes - 1].LoadType = NodeLoad.p;
            for (int t = 0; t < phProps.Counts; t++)
            {
                rope.Nodes[0].deriv[t].p = rope.Nodes[0].deriv[0].p;
                rope.Nodes[phProps.nodes - 1].deriv[t].p = rope.Nodes[phProps.nodes - 1].deriv[0].p;
                //    Re[t] = 0;
                //    bloodV[t] = 0;
                //    bloodP[t] = 0;
                //    rope.Nodes[0].F[t].x = 0 - ((float)Math.Sin(2 * Math.PI * 0.5 * time[t] * freq) * maxLoad);
                //    //model.Nodes[0].deriv[t].p.z = model.Nodes[0].deriv[0].p.z;
                //    //model.Nodes[0].deriv[t].p.y = model.Nodes[0].deriv[0].p.y;
                //    //model.Nodes[0].deriv[t].p.x = 0 - ((time[t] + time[1]) * phProps.MaxU);
                //    //model.Nodes[0].deriv[t].v.x = (model.Nodes[0].deriv[t].p.x - (0 - (time[t] * phProps.MaxU))) / time[1];
                //    int lastN = phProps.nodes - 1;
                //    rope.Nodes[lastN].deriv[t].p.z = rope.Nodes[lastN].deriv[0].p.z;
                //    rope.Nodes[lastN].deriv[t].p.y = rope.Nodes[lastN].deriv[0].p.y;
                //    rope.Nodes[lastN].deriv[t].p.x = rope.Nodes[lastN].deriv[0].p.x;
                //    //model.Nodes[lastN].F[t].x = ((float)Math.Sin(2 * Math.PI * 0.5 * time[t] * freq) * maxLoad);
                //    //model.Nodes[lastN].deriv[t].p.x =  ((float)Math.Sin(2 * Math.PI * 0.5 * time[t] * freq) * maxLoad) + model.Nodes[lastN].deriv[0].p.x;
            }
        }
        public void Run()
        {
            //float A = (float)Math.PI * (float)Math.Pow(phProps.D, 2) / 4;
            //float maxLoad = ((phProps.E * A) / phProps.L / phProps.nodes) * phProps.MaxU;
            for (int t = 1; t < time.Length; t++)
            {
                Parallel.ForEach(rope.Elements, (elem, loopState) =>
                {
                    elem.CalcForce(ref rope, t, Re[t - 1], bloodV[t - 1], bloodP[t - 1]);
                });
                Parallel.ForEach(rope.Nodes, (node, loopState) =>
                {
                    float m = 0;
                    float c = 0;
                    node.GetPhysicParam(ref rope, t - 1, Re[t - 1], ref m, ref c);
                    node.GetForces(ref rope, t, m, c);
                    node.CalcAccel(t, m);
                    /*integrate*/
                    node.Integrate(t, t - 1, phProps.dt);
                });
                //foreach (var elem in rope.Elements)
                //{
                //    elem.CalcForce(ref rope, t, Re[t - 1], bloodV[t - 1], bloodP[t - 1]);
                //}
                //foreach (var node in rope.Nodes)
                //{
                //    float m = 0;
                //    float c = 0;
                //    node.GetPhysicParam(ref rope, t - 1, Re[t - 1], ref m, ref c);
                //    node.GetForces(ref rope, t, m, c);
                //    node.CalcAccel(t, m);
                //    /*integrate*/
                //    node.Integrate(t, t - 1, phProps.dt);
                //}
            }
            GC.Collect();
        }
    }
}
