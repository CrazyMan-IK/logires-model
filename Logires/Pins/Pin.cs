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

namespace Logires.Pins;

public abstract class Pin
{
	private readonly List<Pin> _linkedPins = new List<Pin>();
	private readonly bool _isInput = true;
	private bool _needUpdate = false;

	public Pin(bool isInput)
	{
		_isInput = isInput;
	}

	//public abstract object Value { get; set; }
	public IReadOnlyList<Pin> LinkedPins => _linkedPins;

	//public abstract void SetValue(object value);
	//public abstract object GetValue();

	public abstract void RetrieveValue(Pin other);

	public void Connect(Pin pin)
	{
	  if (IsConnectedWith(pin))
	  {
	    return;
	  }
	
	  if (!_isInput)
	  {
	    pin.Connect(this);
	    return;
	  }
	
	  if (_linkedPins.Count > 0)
	  {
	    Disconnect(_linkedPins[0]);
	  }
	
	  _linkedPins.Add(pin);
	  pin._linkedPins.Add(this);
	}

	public void Disconnect(Pin pin)
	{
	  _linkedPins.Remove(pin);
	  pin.Disconnect(this);
	}
	
	public bool IsConnectedWith(Pin pin)
	{
	  return _linkedPins.Contains(pin);
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
	
	  RetrieveValue(otherPin);
	}
	
	public void MarkDirty()
	{
	  _needUpdate = true;
	}
	
	private void RequestUpdate(long ticks)
	{
	  if (!_needUpdate)
	  {
	    return;
	  }
	
	  _needUpdate = false;
	  //UpdateRequested?.Invoke(ticks);
	  if (_needUpdate)
	  {
	    _needUpdate = false;
	    Update(ticks);
	  }
	}
}

public abstract class TypedPin<T> : Pin
{
	public TypedPin(bool isInput) : base(isInput)
	{
		
	}

	public T Value { get; set; } = default;

	public override void RetrieveValue(Pin other)
	{
		if (other is TypedPin<T> pin)
		{
			Value = pin.Value;
		}
	}

	public override string ToString()
	{
		return $"{GetType().Name}: {Value}";
	}
}
