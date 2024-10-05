using Unity.Entities;
using UnityEngine;

namespace DefaultNamespace
{
    public class SpawnerAuthoring : MonoBehaviour
    {
        public GameObject starPrefab;
        private class SpawnerAuthoringBaker : Baker<SpawnerAuthoring>
        {
            public override void Bake(SpawnerAuthoring authoring)
            {
                var entity = GetEntity(authoring, TransformUsageFlags.None);
                var spawner = new Spawner
                {
                    StarPrefab = GetEntity(authoring.starPrefab, TransformUsageFlags.Dynamic)
                };
                AddComponent(entity, spawner);
            }
        }
    }
}

struct Spawner : IComponentData
{
    public Entity StarPrefab;
}