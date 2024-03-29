namespace Logires.Pins;

public class BitPin : Pin<List<bool>>
{
    public BitPin(bool isInput) : base(isInput)
    {

    }

    public override List<bool> GetDefaultValue()
    {
        return new List<bool>();
    }

    public override string ToString()
    {
    	  return $"{nameof(BitPin)}: [{string.Join(", ", Value)}]";
    }
}
