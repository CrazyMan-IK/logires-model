namespace Logires.Pins;

public class IteratorPin : Pin<LinkedListNode<object>?>
{
    public IteratorPin(bool isInput) : base(isInput)
    {

    }

    public override LinkedListNode<object>? GetDefaultValue()
    {
        return null;
    }

    public override Pin<LinkedListNode<object>?> Clone(bool isInput)
    {
        return new IteratorPin(isInput);
    }
}
