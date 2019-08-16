using Prism.Events;

namespace spring
{
    //ClearPlotsEvent
    public class ClearPlotsEvent : PubSubEvent { }
    public class ComputeEvent : PubSubEvent { }
    public class NodesChangedEvent : PubSubEvent<int> { }
    public class EChangedEvent : PubSubEvent<float> { }
    public class LChangedEvent : PubSubEvent<float> { }
    public class DChangedEvent : PubSubEvent<float> { }
    public class CountsChangedEvent : PubSubEvent<int> { }
    public class dtChangedEvent : PubSubEvent<float> { }
    public class roChangedEvent : PubSubEvent<float> { }

}
