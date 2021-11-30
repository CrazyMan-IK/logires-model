using Logires.Pins;

namespace Logires.Interfaces;

public interface IHaveInputs
{
	IEnumerable<Pin> Inputs { get; }
}
