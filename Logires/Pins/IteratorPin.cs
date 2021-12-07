namespace Logires.Pins;

public class IteratorPin : Pin<LinkedListNode<object>>
{
    public IteratorPin(bool isInput) : base(isInput)
    {

    }

    public override LinkedListNode<object> GetDefaultValue()
    {
        return new LinkedListNode<object>(0);
    }
}
