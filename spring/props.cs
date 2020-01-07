using mechLIB;

namespace spring
{
    public class props
    {
        public props()
        {
            store.E = 2E6f;
            store.L = 25E-3f;
            store.D = 1E-3f;
            store.ro = 1040;
            store.Counts = 20000;
            EndT = store.Counts - 1;
            store.dt = 5E-6f;
            store.nodes = 9;
            store.initDrop = 1E-08f;
            store.MaxU = 2E-3f;
            store.DampRatio = 0.4f;
        }
        public props_t store;
        public PhModels PhMod { get => store.phMod; set => store.phMod = value; }
        private int _EndT;
        public int EndT
        {
            get => _EndT; 
            set => _EndT = value;
        }
        public string DampRatiostr
        {
            get => store.DampRatio.ToString(); set
            {
                if (float.TryParse(value, out float tmp))
                {
                    store.DampRatio = tmp;
                }
            }
        }
        public string MaxUstr
        {
            get => store.MaxU.ToString(); set
            {
                if (float.TryParse(value, out float tmp))
                {
                    store.MaxU = tmp;
                }
            }
        }
        public string initDropstr
        {
            get => store.initDrop.ToString(); set
            {
                if (float.TryParse(value, out float tmp))
                {
                    store.initDrop = tmp;
                }
            }
        }
        public string nodesstr
        {
            get => store.nodes.ToString(); set
            {
                if (int.TryParse(value, out int tmp))
                {
                    store.nodes = tmp;
                }
            }
        }
        public string Estr
        {
            get => store.E.ToString(); set
            {
                if (float.TryParse(value, out float tmp))
                {
                    store.E = tmp;
                }
            }
        }
        public string Lstr
        {
            get => store.L.ToString(); set
            {
                if (float.TryParse(value, out float tmp))
                {
                    store.L = tmp;
                }
            }
        }
        public string Dstr
        {
            get => store.D.ToString(); set
            {
                if (float.TryParse(value, out float tmp))
                {
                    store.D = tmp;
                }
            }
        }
        public string Countstr
        {
            get => store.Counts.ToString(); set
            {
                if (int.TryParse(value, out int tmp))
                {
                    store.Counts = tmp;
                    EndT = tmp - 1;
                }
            }
        }
        public string dtstr
        {
            get => store.dt.ToString(); set
            {
                if (float.TryParse(value, out float tmp))
                {
                    store.dt = tmp;
                }
            }
        }

        public string rostr
        {
            get => store.ro.ToString(); set
            {
                if (float.TryParse(value, out float tmp))
                {
                    store.ro = tmp;
                }
            }
        }

    }
}