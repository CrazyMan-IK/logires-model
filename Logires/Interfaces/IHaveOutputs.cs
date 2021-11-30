using Logires.Pins;

namespace Logires.Interfaces;

public interface IHaveOutputs
{
	IEnumerable<Pin> Outputs { get; }
}
