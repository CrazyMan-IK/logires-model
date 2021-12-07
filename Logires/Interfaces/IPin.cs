namespace Logires.Interfaces;

public interface IPin
{
	IReadOnlyList<IPin> LinkedPins { get; }
	bool IsInput { get; }
	
	void Connect(IPin other);
	void Disconnect(IPin other);
	bool IsConnectedWith(IPin other);
	bool CanConnectTo<T2>(IPin other);

	dynamic? GetValue();
	void SetValueFrom(IPin other);
	T2 RetrieveValue<T2>();
	void Update(long ticks);
	void MarkDirty();
	void RequestUpdate(long ticks);
}

public interface IPin<T> : IPin
{
	T Value { get; set; }
}
