namespace SteveClient.Engine;

public static class Egid
{
    private static uint _lastEgid = 0;

    public static uint NextId => _lastEgid += 1;
    
    // Static id's
    public static readonly uint Camera = NextId;
}