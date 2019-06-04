using Prism.Events;
using Prism.Mvvm;

namespace spring.ViewModels
{
    public class propsViewModel : BindableBase
    {
        private readonly IEventAggregator _ea;
        public propsViewModel(IEventAggregator ea)
        {
            _ea = ea;
            Estr = "2E6";
            Lstr = "25E-3";
            Dstr = "1E-3";
            rostr = "1040";
            Countstr = "3000";
            dtstr = "1E-6";
            nodesstr = "10";
        }
        private string _nodesstr;
        public string nodesstr
        {
            get => _nodesstr; set
            {
                if (int.TryParse(value, out int tmp))
                {
                    _nodesstr = value;
                    _ea.GetEvent<NodesChangedEvent>().Publish(tmp);
                }
            }
        }

        private string _Estr;
        public string Estr
        {
            get => _Estr; set
            {
                if (float.TryParse(value, out float tmp))
                {
                    _Estr = value;
                    _ea.GetEvent<EChangedEvent>().Publish(tmp);
                }
            }
        }


        private string _Lstr;
        public string Lstr
        {
            get => _Lstr; set
            {
                if (float.TryParse(value, out float tmp))
                {
                    _Lstr = value;
                    _ea.GetEvent<LChangedEvent>().Publish(tmp);
                }
            }
        }

        private string _Dstr;
        public string Dstr
        {
            get => _Dstr; set
            {
                if (float.TryParse(value, out float tmp))
                {
                    _Dstr = value;
                    _ea.GetEvent<DChangedEvent>().Publish(tmp);
                }
            }
        }

        private string _Countstr;
        public string Countstr
        {
            get => _Countstr; set
            {
                if (int.TryParse(value, out int tmp))
                {
                    _Countstr = value;
                    _ea.GetEvent<CountsChangedEvent>().Publish(tmp);
                }
            }
        }

        private string _dtstr;
        public string dtstr
        {
            get => _dtstr; set
            {
                if (float.TryParse(value, out float tmp))
                {
                    _dtstr = value;
                    _ea.GetEvent<dtChangedEvent>().Publish(tmp);
                }
            }
        }

        private string _rostr;

        public string rostr
        {
            get => _rostr; set
            {
                if (float.TryParse(value, out float tmp))
                {
                    _rostr = value;
                    _ea.GetEvent<roChangedEvent>().Publish(tmp);
                }
            }
        }


    }
}