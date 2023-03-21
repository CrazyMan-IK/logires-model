//using Timer = Logires.Timer;
using Logires.Interfaces;

namespace Logires;

public class Ticker
{
	private readonly Timer _timer;
	private readonly List<ITickable> _listeners = new List<ITickable>();
	private long _ticks = 0;

	public Ticker(int fps = 60)
	{
		_timer = new Timer(1.0f / fps);
		_timer.Ticked += Tick;
	}

	public long Ticks => _ticks;

	public void Start()
	{
		_timer.Start();
	}

	public void Update(float deltaTime)
	{
		_timer.Update(deltaTime);
	}

	public void Pause()
	{
		_timer.Pause();
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

		_ticks++;
	}
}
