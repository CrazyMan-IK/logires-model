using Logires.Pins;

namespace Logires.Interfaces;

public interface IHaveOutputs
{
	IEnumerable<IPin> Outputs { get; }
}
