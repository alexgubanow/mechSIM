using Prism.Events;

namespace spring
{
    public class ComputeEvent : PubSubEvent<bool> { }
    public class NodesChangedEvent : PubSubEvent<int> { }
    public class EChangedEvent : PubSubEvent<float> { }
    public class LChangedEvent : PubSubEvent<float> { }
    public class DChangedEvent : PubSubEvent<float> { }
    public class CountsChangedEvent : PubSubEvent<int> { }
    public class dtChangedEvent : PubSubEvent<float> { }
    public class roChangedEvent : PubSubEvent<float> { }

}
