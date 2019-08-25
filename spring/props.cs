namespace spring
{
    public class props
    {
        public props(string _E, string _L, string _D, string _ro, string _Counts, string _dt, string _nodes)
        {
            Estr = _E;
            Lstr = _L;
            Dstr = _D;
            rostr = _ro;
            Countstr = _Counts;
            dtstr = _dt;
            nodesstr = _nodes;
        }

        public props(float _E, float _L, float _D, float _ro, int _Counts, float _dt, int _nodes)
        {
            E = _E;
            L = _L;
            D = _D;
            ro = _ro;
            Counts = _Counts;
            dt = _dt;
            nodes = _nodes;
        }

        public props()
        {
            E = 2E6f;
            L = 25E-3f;
            D = 1E-3f;
            ro = 1040;
            Counts = 3000;
            dt = 1E-7f;
            nodes = 7;
        }

        public int nodes;

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

        public float E;

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

        public float L;

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

        public float D;

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

        public int Counts;

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

        public float dt;

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

        public float ro;

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