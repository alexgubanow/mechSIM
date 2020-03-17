using mechLIB;
using Prism.Mvvm;

namespace spring
{
    public class props : BindableBase
    {
        public props()
        {
            E = 6E6f;
            L = 25E-3f;
            D = 1E-3f;
            ro = 1040;
            Counts = 20000;
            dt = 1E-6f;
            nodes = 9;
            initDrop = 1E-08f;
            MaxU = 2E-3f;
            DampRatio = 0.9f;
        }
        public float DampRatio;
        public float MaxU;
        public float initDrop;
        public int nodes;
        public float E;
        public float L;
        public float D;
        public int Counts;
        public float dt;
        public float ro;
        public PhModels phMod;
        public PhModels PhMod { get => phMod; set => phMod = value; }
        
        public string DampRatiostr
        {
            get => DampRatio.ToString(); set
            {
                if (float.TryParse(value, out float tmp))
                {
                    DampRatio = tmp;
                }
            }
        }
        public string MaxUstr
        {
            get => MaxU.ToString(); set
            {
                if (float.TryParse(value, out float tmp))
                {
                    MaxU = tmp;
                }
            }
        }
        public string initDropstr
        {
            get => initDrop.ToString(); set
            {
                if (float.TryParse(value, out float tmp))
                {
                    initDrop = tmp;
                }
            }
        }
        public string nodesstr
        {
            get => nodes.ToString(); set
            {
                if (int.TryParse(value, out int tmp))
                {
                    nodes = tmp;
                }
            }
        }
        public string Estr
        {
            get => E.ToString(); set
            {
                if (float.TryParse(value, out float tmp))
                {
                    E = tmp;
                }
            }
        }
        public string Lstr
        {
            get => L.ToString(); set
            {
                if (float.TryParse(value, out float tmp))
                {
                    L = tmp;
                }
            }
        }
        public string Dstr
        {
            get => D.ToString(); set
            {
                if (float.TryParse(value, out float tmp))
                {
                    D = tmp;
                }
            }
        }
        public string Countstr
        {
            get => Counts.ToString(); set
            {
                if (int.TryParse(value, out int tmp))
                {
                    Counts = tmp;
                }
            }
        }
        public string dtstr
        {
            get => dt.ToString(); set
            {
                if (float.TryParse(value, out float tmp))
                {
                    dt = tmp;
                }
            }
        }

        public string rostr
        {
            get => ro.ToString(); set
            {
                if (float.TryParse(value, out float tmp))
                {
                    ro = tmp;
                }
            }
        }

    }
}