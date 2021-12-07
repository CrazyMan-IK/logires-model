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

    public override Pin<double> Clone(bool isInput)
    {
        return new DoublePin(isInput);
    }
}
