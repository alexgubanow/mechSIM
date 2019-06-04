using OxyPlot;
using System.Collections.Generic;

namespace spring
{
    public class DataToDraw
    {
        public float[] Y { get; set; }
        public string Title { get; set; }
        public float[] X { get; set; }

        public List<DataPoint> getDataPointList()
        {
            List<DataPoint> tmp = new List<DataPoint>();
            for (int t = 0; t < X.Length; t++)
            {
                tmp.Add(new DataPoint(X[t], Y[t]));
            }
            return tmp;
        }
    }
}
