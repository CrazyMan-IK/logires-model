namespace Logires.Pins;

public class BooleanPin : Pin<bool>
{
    public BooleanPin(bool isInput) : base(isInput)
    {

    }

    public override bool GetDefaultValue()
    {
        return false;
    }

    public override Pin<bool> Clone(bool isInput)
    {
        return new BooleanPin(isInput);
    }
}
