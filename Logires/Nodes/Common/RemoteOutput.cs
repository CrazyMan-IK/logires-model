using System.Net;
using Logires.Pins;
using Logires.Interfaces;

namespace Logires.Nodes;

public class RemoteOutput : Remote, IHaveOutputs
{
    private readonly IntegerPin _outputA = new IntegerPin(false);
    private readonly IntegerPin _outputB = new IntegerPin(false);
    private readonly IntegerPin _outputC = new IntegerPin(false);
    private readonly IntegerPin _outputD = new IntegerPin(false);
    private readonly BitPin _outputE = new BitPin(false);

    private byte[] _lastIPBytes = new byte[] { 0, 0, 0, 0 };
    private List<bool> _lastValue = new List<bool>();

    public RemoteOutput()
    {
        Multicaster.Instance.MessageReceived += OnMessageReceived;
    }

    public IEnumerable<IPin> Outputs
    {
        get
        {
            yield return _outputA;
            yield return _outputB;
            yield return _outputC;
            yield return _outputD;
            yield return _outputE;
        }
    }

    public override void Update(long ticks)
    {
        _outputA.Value = _lastIPBytes[0];
        _outputB.Value = _lastIPBytes[1];
        _outputC.Value = _lastIPBytes[2];
        _outputD.Value = _lastIPBytes[3];

        _outputE.Value = _lastValue;
    }

    private void OnMessageReceived(IPAddress ip, Message message)
    {
        if (message.ID != Name)
        {
            return;
        }
        if (message.Value is not List<bool> value)
        {
            return;
        }

        _lastIPBytes = ip.GetAddressBytes();
        _lastValue = value;
    }
}
