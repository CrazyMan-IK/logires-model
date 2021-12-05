using Logires.Pins;

namespace Logires.Interfaces;

public interface IHaveInputs
{
	IEnumerable<IPin> Inputs { get; }
}
