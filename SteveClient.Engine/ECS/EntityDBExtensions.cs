using System.Runtime.CompilerServices;
using SteveClient.Engine.ECS.Data;
using Svelto.DataStructures;
using Svelto.ECS;
using static SteveClient.Engine.ECS.Data.FilterAndApplyDelegates;

namespace SteveClient.Engine.ECS;

public static class EntityDbExtensions
{
    #region QueryUniqueOptional

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static OptionalRefEntityComponent<T1> QueryUniqueEntityOptional<T1>(this EntitiesDB entitiesDb, ExclusiveGroupStruct group)
        where T1 : unmanaged, IEntityComponent
    {
        EntityCollection<T1> ec = entitiesDb.QueryEntities<T1>(group);
        
        ec.Deconstruct(out NB<T1> buffer, out int count);

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

    #endregion

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void FilterAndApply<T1>(this EntitiesDB entitiesDb, in LocalFasterReadOnlyList<ExclusiveGroupStruct> groups, FilterPredicateDelegate<T1> predicate, FilterApplyDelegate<T1> apply)
        where T1 : unmanaged, IEntityComponent
    {
        foreach (var ((buffer1, count), _) in entitiesDb.QueryEntities<T1>(groups))
        {
            for (int i = 0; i < count; i++)
            {
                if (!predicate.Invoke(ref buffer1[i]))
                    continue;
                
                apply.Invoke(ref buffer1[i]);
            }
        }
    }
}