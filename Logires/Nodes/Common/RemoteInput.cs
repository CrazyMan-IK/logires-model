using Logires.Pins;
using Logires.Interfaces;

namespace Logires.Nodes;

public class RemoteInput : Remote, IHaveInputs
{
    private readonly BitPin _input = new BitPin(true);

    public RemoteInput()
    {

    }

    public IEnumerable<IPin> Inputs
    {
        get
        {
            yield return _input;
        }
    }

    public override void Update(long ticks)
    {
        Multicaster.Instance.BroadcastMessage(new Message()
        {
            ID = Name,
            Value = _input.Value
        });
    }
}
