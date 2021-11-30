namespace Logires.Interfaces;

public interface ITickable
{
	void MarkDirty();
	void Tick(long ticks);
}
