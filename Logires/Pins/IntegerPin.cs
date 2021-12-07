namespace Logires.Pins;

public class IntegerPin : Pin<int>
{
    public IntegerPin(bool isInput) : base(isInput)
    {

    }

    public override int GetDefaultValue()
    {
        return 0;
    }

    public override Pin<int> Clone(bool isInput)
    {
        return new IntegerPin(isInput);
    }
}
