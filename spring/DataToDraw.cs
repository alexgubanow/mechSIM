using OxyPlot;
using System.Collections.Generic;
using System.Windows.Media.Media3D;

namespace spring
{
    public class DataToDraw
    {
        public string Title { get; set; }
        public float[] X { get; set; }
        public float[][] Y { get; set; }

        public List<DataPoint> getDataPointList(C axis)
        {
            List<DataPoint> tmp = new List<DataPoint>();
            for (int t = 0; t < X.Length; t++)
            {
                tmp.Add(new DataPoint(X[t], Y[t][(int)axis]));
            }
            return tmp;
        }
    }
}
