using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mechLIB
{
    public class Enviro
    {
        public Enviro()
        { }
        public void PrepRun(props_t propStore)
        {
            phProps = propStore;
            allocateTime(phProps.dt, phProps.Counts);

            #region load file

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

            #endregion load file

            rope = new Rope_t(phProps);
            GenerateLoad(C_t.x, ref rope);
        }
        public props_t phProps;
        public float[] Re;
        public float[] bloodV;
        public float[] bloodP;
        public float[] time;
        public Rope_t rope;
        private void allocateTime(float dt, int Counts)
        {
            time = new float[Counts];
            for (int i = 1; i < Counts; i++)
            {
                time[i] = time[i - 1] + dt;
            }
        }
        private void GenerateLoad(C_t axis, ref Rope_t model)
        {
            Re = new float[phProps.Counts];
            bloodV = new float[phProps.Counts];
            bloodP = new float[phProps.Counts];
            float A = (float)Math.PI * (float)Math.Pow(phProps.D, 2) / 4;
            float maxLoad = ((phProps.E * A) / phProps.L / phProps.nodes) * phProps.MaxU;
            float freq = 1 / (phProps.Counts * phProps.dt);
            for (int t = 0; t < phProps.Counts; t++)
            {
                Re[t] = 0;
                bloodV[t] = 0;
                bloodP[t] = 0;
                model.Nodes[0].F[t].x = 0 - ((float)Math.Sin(2 * Math.PI * 0.5 * time[t] * freq) * maxLoad);
                //model.Nodes[0].deriv[t].p.z = model.Nodes[0].deriv[0].p.z;
                //model.Nodes[0].deriv[t].p.y = model.Nodes[0].deriv[0].p.y;
                //model.Nodes[0].deriv[t].p.x = 0 - ((time[t] + time[1]) * phProps.MaxU);
                //model.Nodes[0].deriv[t].v.x = (model.Nodes[0].deriv[t].p.x - (0 - (time[t] * phProps.MaxU))) / time[1];
                int lastN = phProps.nodes - 1;
                model.Nodes[lastN].deriv[t].p.z = model.Nodes[lastN].deriv[0].p.z;
                model.Nodes[lastN].deriv[t].p.y = model.Nodes[lastN].deriv[0].p.y;
                model.Nodes[lastN].deriv[t].p.x = model.Nodes[lastN].deriv[0].p.x;
                //model.Nodes[lastN].F[t].x = ((float)Math.Sin(2 * Math.PI * 0.5 * time[t] * freq) * maxLoad);
                //model.Nodes[lastN].deriv[t].p.x =  ((float)Math.Sin(2 * Math.PI * 0.5 * time[t] * freq) * maxLoad) + model.Nodes[lastN].deriv[0].p.x;
            }
        }
        public void Run()
        {
            //float A = (float)Math.PI * (float)Math.Pow(phProps.D, 2) / 4;
            //float maxLoad = ((phProps.E * A) / phProps.L / phProps.nodes) * phProps.MaxU;
            for (int t = 1; t < time.Length; t++)
            {
                foreach (var elem in rope.Elements)
                {
                    elem.CalcForce(ref rope, t, Re[t - 1], bloodV[t - 1], bloodP[t - 1]);
                }
                foreach (var node in rope.Nodes)
                {
                    float m = 0;
                    float c = 0;
                    node.GetPhysicParam(ref rope, t - 1, Re[t - 1], ref m, ref c);
                    node.GetForces(ref rope, t, m, c);
                    node.CalcAccel(t, m);
                    /*integrate*/
                    node.Integrate(t, t - 1, phProps.dt);
                }
            }
            GC.Collect();
        }
    }
}
