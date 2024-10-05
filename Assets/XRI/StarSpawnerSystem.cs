using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using System.IO;

public class StarSpawnerSystem : SystemBase
{
    protected override void OnCreate()
    {
        // Ensure the system runs only once at the start
        RequireSingletonForUpdate<StarSpawner>();
    }
    
    protected override async void OnUpdate()
    {
        var spawner = GetSingleton<StarSpawner>();
        var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

        // Fetch and filter data from Gaia API
        string apiUrl = spawner.apiUrl.ToString();
        NativeArray<float3> positions = await GaiaDataFetcher.FetchAndFilterData(apiUrl, item =>
        {
            // Example filter: only include stars with x > 0
            return (float)item["x"] > 0;
        });

        // Create a job to instantiate entities
        var job = new StarSpawnJob
        {
            EntityPrefab = spawner.EntityPrefab,
            Positions = positions,
            EntityManager = entityManager
        };

        Dependency = job.Schedule(positions.Length, 64, Dependency);
        Dependency.Complete();

        // Clean up
        positions.Dispose();
        EntityManager.DestroyEntity(GetSingletonEntity<StarSpawner>());
    }

    [BurstCompile]
    struct StarSpawnJob : IJobParallelFor
    {
        public Entity EntityPrefab;
        [DeallocateOnJobCompletion] public NativeArray<float3> Positions;
        public EntityManager EntityManager;

        public void Execute(int index)
        {
            var instance = EntityManager.Instantiate(EntityPrefab);
            EntityManager.SetComponentData(instance, new Translation { Value = Positions[index] });
        }
    }
}

public struct StarSpawner : IComponentData
{
    public Entity EntityPrefab;
    public FixedString128Bytes apiUrl;
}