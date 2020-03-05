using csmatio.io;
using csmatio.types;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
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
            Vector3 startCoord = new Vector3() { X= pmx[0], Y = pmy[0] };
            Vector3 endCoord = new Vector3() { X = plx[0], Y = ply[0] };
            phProps.L = Vector3.Distance(startCoord, endCoord);
            rope = new Rope_t(phProps, startCoord, endCoord);
            //fill load to rope
            Re = (mfr.Content["req"] as MLSingle).GetArray()[0];
            bloodV = (mfr.Content["bloodVq"] as MLSingle).GetArray()[0];
            bloodP = (mfr.Content["abpq"] as MLSingle).GetArray()[0];
            int lastN = phProps.nodes - 1;
            for (int t = 0; t < phProps.Counts; t++)
            {
                rope.Nodes[0].deriv[t].p.X = pmx[t];
                rope.Nodes[0].deriv[t].p.Y = pmy[t];
                rope.Nodes[0].deriv[t].u.X = rope.Nodes[0].deriv[t].p.X - rope.Nodes[0].deriv[0].p.X;

                rope.Nodes[lastN].deriv[t].p.X = plx[t];
                rope.Nodes[lastN].deriv[t].p.Y = ply[t];
                rope.Nodes[lastN].deriv[t].u.X = rope.Nodes[lastN].deriv[t].p.X - rope.Nodes[lastN].deriv[0].p.X;
            }
            //choose load nodes
            rope.Nodes[0].LoadType = NodeLoad.u;
            rope.Nodes[phProps.nodes - 1].LoadType = NodeLoad.u;
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
            float A = (float)Math.PI * maf.P2(phProps.D) / 4;
            float maxLoad = ((phProps.E * A) / phProps.L / phProps.nodes) * phProps.MaxU;
            float freq = 1 / (phProps.Counts * phProps.dt);
            rope.Nodes[0].LoadType = NodeLoad.p;
            rope.Nodes[phProps.nodes - 1].LoadType = NodeLoad.u;
            int lastN = phProps.nodes - 1;
            for (int t = 0; t < phProps.Counts; t++)
            {
                rope.Nodes[0].deriv[t].p = rope.Nodes[0].deriv[0].p;
                rope.Nodes[lastN].deriv[t].p = rope.Nodes[lastN].deriv[0].p;
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
                //    //model.Nodes[lastN].F[t].x = ((float)Math.Sin(2 * Math.PI * 0.5 * time[t] * freq) * maxLoad);
                //rope.Nodes[lastN].deriv[t].p.X = rope.Nodes[lastN].deriv[0].p.X;
                //rope.Nodes[lastN].deriv[t].p.X = 0 - ((float)Math.Sin(2 * Math.PI * time[t] * freq / 3) * phProps.MaxU) + rope.Nodes[lastN].deriv[0].p.X;
                rope.Nodes[lastN].deriv[t].p.Y = ((float)Math.Sin(2 * Math.PI * time[t] * freq) * phProps.MaxU) + rope.Nodes[lastN].deriv[0].p.Y;
                rope.Nodes[lastN].deriv[t].u = rope.Nodes[lastN].deriv[t].p - rope.Nodes[lastN].deriv[0].p;
                //rope.Nodes[lastN].deriv[t].v.X = ((float)Math.Cos(2 * Math.PI * 0.5 * time[t] * freq) * phProps.MaxU) + rope.Nodes[lastN].deriv[0].v.X;
            }
        }
        public void Run()
        {
            for (int t = 1; t < time.Length; t++)
            {
                rope.StepOverElems(t, Re[t - 1], bloodV[t - 1], bloodP[t - 1]);
                rope.StepOverNodes(t, Re[t - 1], phProps.dt);
                foreach (var elem in rope.Elements)
                {
                    rope.L[t] += elem.L[t];
                }
            }
            GC.Collect();
        }
    }
}
