namespace SteveClient.Engine.ECS.Data;

public static class FilterAndApplyDelegates
{
    public delegate bool FilterPredicateDelegate<T1>(ref T1 item);

    public delegate void FilterApplyDelegate<T1>(ref T1 item);
}