using mechLIB;

namespace spring
{
    public class props
    {
        //public props(string _E, string _L, string _D, string _ro, string _Counts, string _dt, string _nodes, string _initDrop)
        //{
        //    Estr = _E;
        //    Lstr = _L;
        //    Dstr = _D;
        //    rostr = _ro;
        //    Countstr = _Counts;
        //    dtstr = _dt;
        //    nodesstr = _nodes;
        //    initDropstr = _initDrop;
        //}

        //public props(float _E, float _L, float _D, float _ro, int _Counts, float _dt, int _nodes, float _initDrop)
        //{
        //    E = _E;
        //    L = _L;
        //    D = _D;
        //    ro = _ro;
        //    Counts = _Counts;
        //    dt = _dt;
        //    nodes = _nodes;
        //    initDrop = _initDrop;
        //}

        public props()
        {
            store.E = 2E6f;
            store.L = 25E-3f;
            store.D = 1E-3f;
            store.ro = 1040;
            store.Counts = 3000;
            store.dt = 1E-7f;
            store.nodes = 7;
            store.initDrop = 1E-1f;
            store.MaxU = 2E-3f;
        }
        public props_t store;

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