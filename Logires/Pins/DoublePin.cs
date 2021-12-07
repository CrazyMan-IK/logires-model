namespace Logires.Pins;

public class DoublePin : Pin<double>
{
    public DoublePin(bool isInput) : base(isInput)
    {

    }

    public override double GetDefaultValue()
    {
        return 0;
    }
}
