namespace Logires.Interfaces;

public interface IPin
{
	List<IPin> LinkedPins { get; }

	void Connect(IPin other);
	void Disconnect(IPin other);
	bool IsConnectedWith(IPin pin)

	void Update(long ticks);
	void MarkDirty();
}

public interface IPin<T> : IPin
{
	T Value { get; set; }
}
