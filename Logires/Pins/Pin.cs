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
	private readonly List<IPin> _linkedPins = new List<IPin>();
	private readonly bool _isInput = true;
	private bool _needUpdate = false;

	public Pin(bool isInput)
	{
		_isInput = isInput;
	}

	public T? Value { get; set; } = default;
	public IReadOnlyList<IPin> LinkedPins => _linkedPins;

	public void Connect(IPin pin)
	{
	  if (IsConnectedWith(pin))
	  {
	    return;
	  }
	
	  if (!_isInput)
	  {
	  	_linkedPins.Add(pin);
	    pin.Connect(this);
	    return;
	  }
	
	  if (_linkedPins.Count > 0)
	  {
	    Disconnect(_linkedPins[0]);
	  }
	
	  _linkedPins.Add(pin);
	  pin.Connect(this);
	}

	public void Disconnect(IPin pin)
	{
		if (!IsConnectedWith(pin))
		{
			return;
		}
	
	  _linkedPins.Remove(pin);
	  pin.Disconnect(this);
	}
	
	public bool IsConnectedWith(IPin pin)
	{
	  return _linkedPins.Contains(pin);
	}

	public T2 RetrieveValue<T2>(IPin other)
	{
		if (Value == null)
		{
			throw new InvalidOperationException();
		}
	
		return PinsConverter.GetConvertator<T, T2>().Invoke(Value);
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

	  if (otherPin is Pin<T> other)
	  {
	  	Value = other.Value;
	  	return;
	  }

	  Value = otherPin.RetrieveValue<T>(this);
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
}
