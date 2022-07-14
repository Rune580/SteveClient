using Svelto.ECS;

namespace SteveClient.Engine.ECS.Data;

public class OptionalRefEntityComponent<T1>
    where T1 : unmanaged, IEntityComponent
{
    private readonly GetRefT1Delegate? _getRefT1;

    public OptionalRefEntityComponent(bool hasValue)
    {
        HasValue = hasValue;
    }

    public OptionalRefEntityComponent(GetRefT1Delegate getRefT1)
    {
        _getRefT1 = getRefT1;
        HasValue = _getRefT1 is not null;
    }

    public ref T1 Get1()
    {
        if (!HasValue)
            throw new NullReferenceException();

        return ref _getRefT1!();
    }

    public bool HasValue { get; }

    public delegate ref T1 GetRefT1Delegate();
}

public class OptionalRefEntityComponent<T1, T2>
    where T1 : unmanaged, IEntityComponent
    where T2 : unmanaged, IEntityComponent
{
    private readonly GetRefT1Delegate? _getRefT1;
    private readonly GetRefT2Delegate? _getRefT2;

    public OptionalRefEntityComponent(bool hasValue)
    {
        HasValue = hasValue;
    }

    public OptionalRefEntityComponent(GetRefT1Delegate getRefT1,
        GetRefT2Delegate getRefT2)
    {
        _getRefT1 = getRefT1;
        _getRefT2 = getRefT2;
        HasValue = _getRefT1 is not null
            && _getRefT2 is not null;
    }

    public ref T1 Get1()
    {
        if (!HasValue)
            throw new NullReferenceException();

        return ref _getRefT1!();
    }

    public ref T2 Get2()
    {
        if (!HasValue)
            throw new NullReferenceException();

        return ref _getRefT2!();
    }

    public bool HasValue { get; }

    public delegate ref T1 GetRefT1Delegate();
    public delegate ref T2 GetRefT2Delegate();
}