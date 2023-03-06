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
}
