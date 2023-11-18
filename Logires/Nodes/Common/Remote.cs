using Logires.Pins;

using Logires.Interfaces;

namespace Logires.Nodes;

public abstract class Remote : Node
{
	public string Name { get; set; } = "default";
}
