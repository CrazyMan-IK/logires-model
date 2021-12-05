namespace Logires.Interfaces;

public interface IPin
{
	IReadOnlyList<IPin> LinkedPins { get; }
	
	void Connect(IPin other);
	void Disconnect(IPin other);
	bool IsConnectedWith(IPin pin);

	T2 RetrieveValue<T2>(IPin other);
	void Update(long ticks);
	void MarkDirty();
	void RequestUpdate(long ticks);
}

public interface IPin<T> : IPin
{
	T? Value { get; set; }
}
