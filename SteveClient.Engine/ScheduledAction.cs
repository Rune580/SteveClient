namespace SteveClient.Engine;

public class ScheduledAction
{
    private readonly Action _action;
    private readonly bool _enforceFrequency;
    private ulong _frequency;
    private ulong _lastTick;
    private ulong _remainingDelta;

    public ScheduledAction(Action action, ulong frequency, bool enforceFrequency)
    {
        _action = action;
        _frequency = frequency;
        _enforceFrequency = enforceFrequency;
        _lastTick = 0UL;
        _remainingDelta = 0UL;
    }

    public void UpdateFrequency(ulong frequency)
    {
        _frequency = frequency;
    }

    public void Tick(ulong elapsedTicks)
    {
        _remainingDelta += elapsedTicks;

        if (_enforceFrequency)
        {
            while (_remainingDelta >= _frequency)
            {
                _remainingDelta -= _frequency;
                
                _action();
            }
        }
        else if (_remainingDelta >= _frequency)
        {
            var i = _remainingDelta / _frequency;

            _remainingDelta -= _frequency * i;

            _action();
        }

        _lastTick = elapsedTicks;
    }

    public float NormalizedDelta => (float)_remainingDelta / _frequency;
}