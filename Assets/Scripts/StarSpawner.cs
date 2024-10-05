using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public partial struct StarSpawnSystem : ISystem
{

    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<Spawner>();
    }
    
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        state.Enabled = false;
        var prefab = SystemAPI.GetSingleton<Spawner>().StarPrefab;
        int numberOfStars = GaiaDataFetcher.nativePositions.Length;
        var instances = state.EntityManager.Instantiate(prefab, numberOfStars, Allocator.Temp);
        int index = 0;
        foreach (var entity in instances)
        {
            float x = GaiaDataFetcher.nativePositions[index][0];
            float y = GaiaDataFetcher.nativePositions[index][1];
            float z = GaiaDataFetcher.nativePositions[index][2];
            var transform = SystemAPI.GetComponentRW<LocalTransform>(entity);
            transform.ValueRW.Position = new float3(x, y, z);
            index++;
        }
    }
    
}