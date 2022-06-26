using System.Runtime.CompilerServices;
using SteveClient.Engine.ECS.Data;
using Svelto.ECS;

namespace SteveClient.Engine.ECS;

public static class EntityDbExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static OptionalRefEntityComponent<T1> QueryUniqueEntityOptional<T1>(this EntitiesDB entitiesDb, ExclusiveGroupStruct group)
        where T1 : unmanaged, IEntityComponent
    {
        EntityCollection<T1> ec = entitiesDb.QueryEntities<T1>(group);
        
        ec.Deconstruct(out var buffer, out int count);

        return count switch
        {
            0 => new OptionalRefEntityComponent<T1>(false),
            1 => new OptionalRefEntityComponent<T1>(() => ref buffer[0]),
            _ => throw new ECSException("Unique entities must be unique! '"
                .FastConcat(typeof(T1).ToString()).FastConcat("'"))
        };
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static OptionalRefEntityComponent<T1, T2> QueryUniqueEntityOptional<T1, T2>(this EntitiesDB entitiesDb, ExclusiveGroupStruct group)
        where T1 : unmanaged, IEntityComponent
        where T2 : unmanaged, IEntityComponent
    {
        EntityCollection<T1, T2> ec = entitiesDb.QueryEntities<T1, T2>(group);

        ec.Deconstruct(out var buffer1, out var buffer2, out int count);

        return count switch
        {
            0 => new OptionalRefEntityComponent<T1, T2>(false),
            1 => new OptionalRefEntityComponent<T1, T2>(() => ref buffer1[0], () => ref buffer2[0]),
            _ => throw new ECSException("Unique entities must be unique! '"
                .FastConcat(typeof(T1).ToString()).FastConcat("'"))
        };
    }
}