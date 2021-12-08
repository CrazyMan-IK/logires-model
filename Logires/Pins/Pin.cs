/*import EventEmmiter from 'events';

export default abstract class Pin extends EventEmmiter {
  private _linkedPins: Pin[] = [];
  private _isInput = true;
  private _needUpdate = false;

  public abstract Value: any;

  public constructor(isInput: boolean) {
    super();
    this._isInput = isInput;
  }

  public get LinkedPins(): Pin[] {
    return [...this._linkedPins];
  }

  public connect(pin: Pin): void {
    if (this.isConnectedWith(pin)) {
      return;
    }

    if (!this._isInput) {
      pin.connect(this);
      return;
    }

    if (this._linkedPins.length > 0) {
      this.disconnect(this._linkedPins[0]);
    }

    this._linkedPins.push(pin);
    pin._linkedPins.push(this);
  }

  public disconnect(pin: Pin): void {
    const i = this._linkedPins.findIndex((x) => x === pin);

    if (i > -1) {
      this._linkedPins.splice(i, 1);
      pin.disconnect(this);
    }
  }

  public isConnectedWith(pin: Pin): boolean {
    return this._linkedPins.includes(pin);
  }

  public update(ticks: number): void {
    if (!this._isInput) {
      this._needUpdate = false;
      return;
    }

    if (this._linkedPins.length < 1) {
      return;
    }

    const [otherPin] = this._linkedPins;
    otherPin.requestUpdate(ticks);

    this.Value = otherPin.Value;
  }

  public markDirty() {
    this._needUpdate = true;
  }

  private requestUpdate(ticks: number) {
    if (!this._needUpdate) {
      return;
    }

    this._needUpdate = false;
    this.emit('UpdateRequested', ticks);
  }
}

export abstract class TypedPin<T> extends Pin {
  public abstract Value: T;
}
*/

using Logires.Interfaces;

namespace Logires.Pins;

public abstract class Pin<T> : IPin, IPin<T>
{
    private readonly List<IPin> _receivers = new List<IPin>();
    private readonly List<IPin> _linkedPins = new List<IPin>();
    private readonly bool _isInput = true;
    private bool _needUpdate = false;

    public Pin(bool isInput)
    {
        _isInput = isInput;
        Value = GetDefaultValue();
    }

    public T Value { get; set; }
    public IReadOnlyList<IPin> LinkedPins => _linkedPins;
    public bool IsInput => _isInput;

    public void Connect(IPin other)
    {
        if (!other.CanConnectTo<T>(this))
        {
            throw new InvalidOperationException();
        }

        if (IsConnectedWith(other))
        {
            return;
        }

        if (!_isInput)
        {
            _linkedPins.Add(other);
            other.Connect(this);
            return;
        }

        if (_linkedPins.Count > 0)
        {
            Disconnect(_linkedPins[0]);
        }

        _linkedPins.Add(other);
        other.Connect(this);
    }

    public void ConnectReceiver(IPin other)
    {
				if (!other.CanConnectTo<T>(this))
				{
					  throw new InvalidOperationException();
				}
    
				if (_receivers.Contains(other))
				{
					  return;
				}
    
    	  _receivers.Add(other);
    	  other.ConnectReceiver(this);
    }

    public void Disconnect(IPin other)
    {
        if (!IsConnectedWith(other))
        {
            return;
        }

        _linkedPins.Remove(other);
        other.Disconnect(this);
    }

    public bool IsConnectedWith(IPin other)
    {
        return _linkedPins.Contains(other);
    }

    public bool CanConnectTo<T2>(IPin other)
    {
        var isDifferentSide = _isInput ^ other.IsInput;
        var isSameType = typeof(T) == typeof(T2);

        return isDifferentSide && (PinsConverter.HasConvertator<T, T2>() || isSameType);
    }

    public void SetValueFrom<T2>(IPin other)
    {
    	  Value = other.RetrieveValue<T>();
    }

    public T2 RetrieveValue<T2>()
    {
        var convertator = PinsConverter.GetConvertator<T, T2>();
        if (convertator == null)
        {
            throw new InvalidOperationException();
        }

        return convertator.Invoke(Value);
    }

    public void Update(long ticks)
    {
        if (!_isInput)
        {
            _needUpdate = false;
            return;
        }

        if (_linkedPins.Count < 1)
        {
            return;
        }

        var otherPin = _linkedPins[0];
        otherPin.RequestUpdate(ticks);

        /*if (otherPin is Pin<T> other)
        {
            Value = other.Value;
            return;
        }*/

        Value = otherPin.RetrieveValue<T>();

        foreach (var pin in _receivers)
        {
        	  pin.SetValueFrom<T>(this);
        }
    }

    public void MarkDirty()
    {
        _needUpdate = true;
    }

    public void RequestUpdate(long ticks)
    {
        if (!_needUpdate)
        {
            return;
        }

        _needUpdate = false;
        Update(ticks);
    }

    public override string ToString()
    {
        return $"{GetType().Name}: {Value}";
    }

    public abstract T GetDefaultValue();
    public abstract Pin<T> Clone(bool isInput);
}
