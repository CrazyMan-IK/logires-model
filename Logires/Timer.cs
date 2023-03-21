namespace Logires;

public class Timer
{
    public event Action? Ticked = null;

    private float _duration = 0;
    private float _time = 0;
    private bool _isPaused = false;

    public Timer(float duration)
    {
        Duration = duration;
    }

    public float Duration
    {
        get => _duration;
        set
        {
            if (value <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(value));
            }

            if (_duration != value)
            {
                _duration = value;
            }
        }
    }
    public float TickedTime => _time;
    public float TimeLeft => _duration - _time;
    public float TimeLeftPercentage => _time / _duration;
    public bool IsPaused => _isPaused;

    public void Start()
    {
        _isPaused = false;
    }

    public void Pause()
    {
        _isPaused = true;
    }

    public void Reset()
    {
        _time = 0;
    }

    public void Stop()
    {
        Pause();
        Reset();
    }

    public void Update(float deltaTime)
    {
        if (_isPaused)
        {
            return;
        }

        _time += deltaTime;

        while (_time >= _duration)
        {
            _time -= _duration;

            Ticked?.Invoke();
        }
    }
}
