using Timer = System.Timers.Timer;
using Logires.Interfaces;

namespace Logires;

public class Ticker
{
	private readonly Timer _timer;
	private readonly List<ITickable> _listeners = new List<ITickable>();
	private long _ticks = 0;

	public Ticker(int fps = 60)
	{
		_timer = new Timer((1.0 / fps) * 1000);
		_timer.Elapsed += (s, e) => Tick();
		_timer.AutoReset = true;
	}

	public void Start()
	{
		_timer.Start();
	}

	public void Pause()
	{
		_timer.Stop();
	}

	public void AddListener(ITickable listener)
	{
		_listeners.Add(listener);
	}
	
	public void RemoveListener(ITickable listener)
	{
		_listeners.Remove(listener);
	}
	
	public void Tick()
	{
		foreach (var listener in _listeners)
		{
			listener.MarkDirty();
		}
		
		foreach (var listener in _listeners)
		{
			listener.Tick(_ticks);
		}

		Console.WriteLine();
		_ticks++;
	}
}
