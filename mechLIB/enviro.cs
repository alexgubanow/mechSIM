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
            List<string> loadStrs = new List<string>();
            using (StreamReader sr = File.OpenText(loadFile))
            {
                string line = string.Empty;
                //read load back
                while ((line = sr.ReadLine()) != null)
                {
                    loadStrs.Add(line);
                }
            }
            float[] time = new float[loadStrs.Count];
            float[] pmx = new float[loadStrs.Count];
            float[] plx = new float[loadStrs.Count];
            float[] pmy = new float[loadStrs.Count];
            float[] ply = new float[loadStrs.Count];
            //t	PMx	PMy	PLx	PLy
            for (int i = 0; i < loadStrs.Count; i++)
            {
                string[] tmp = loadStrs[i].Split('\t');
                time[i] = Convert.ToSingle(tmp[0]);
                pmx[i] = Convert.ToSingle(tmp[1]);
                plx[i] = Convert.ToSingle(tmp[2]);
                pmy[i] = Convert.ToSingle(tmp[3]);
                ply[i] = Convert.ToSingle(tmp[4]);
            }
            //interpolate
            int ipCounts = (int)(loadStrs.Count * (time[1] - time[0]) / phProps.dt);
            phProps.Counts = ipCounts;
            allocateTime(phProps.dt, ipCounts);
            float[] pmxip = new float[ipCounts];
            float[] plxip = new float[ipCounts];
            float[] pmyip = new float[ipCounts];
            float[] plyip = new float[ipCounts];
            int ratio = (int)((time[1] - time[0]) / phProps.dt);
            maf.Linip(pmx, ratio, ref pmxip);
            maf.Linip(plx, ratio, ref plxip);
            maf.Linip(pmy, ratio, ref pmyip);
            maf.Linip(ply, ratio, ref plyip);
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
                rope.Nodes[0].deriv[t].p.x = rope.Nodes[0].deriv[0].p.x + pmxip[t];
                rope.Nodes[0].deriv[t].p.y = rope.Nodes[0].deriv[0].p.y + pmyip[t];

                int lastN = phProps.nodes - 1;
                rope.Nodes[lastN].deriv[t].p.x = rope.Nodes[lastN].deriv[0].p.x + plxip[t];
                rope.Nodes[lastN].deriv[t].p.y = rope.Nodes[lastN].deriv[0].p.y + plyip[t];
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
            rope.Nodes[0].LoadType = NodeLoad.f;
            rope.Nodes[phProps.nodes - 1].LoadType = NodeLoad.p;
            //for (int t = 0; t < phProps.Counts; t++)
            //{
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
            //}
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
