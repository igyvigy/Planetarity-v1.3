using Unity.Entities;
using UnityEngine;
using Unity.Transforms;
using Unity.Collections;

namespace Planetarity.ECS
{
    public class ECSSpawner : MonoBehaviour
    {
        public int asteroidsToSpawn = 200;
        [SerializeField] GameObject[] asteroidPrefabs;

        private EntityManager entityManager;
        private World defaultWorld;
        private NativeArray<Entity> asteroidEntities;

        private void Start()
        {
            defaultWorld = World.DefaultGameObjectInjectionWorld;
            entityManager = defaultWorld.EntityManager;

            GameObjectConversionSettings settings = GameObjectConversionSettings.FromWorld(defaultWorld, null);
            asteroidEntities = new NativeArray<Entity>(asteroidPrefabs.Length, Allocator.Temp);
            for (var i = 0; i < asteroidEntities.Length; i++)
            {
                asteroidEntities[i] = GameObjectConversionUtility.ConvertGameObjectHierarchy(asteroidPrefabs[i], settings);
            }
            for (var i = 0; i < asteroidsToSpawn; i++)
            {
                SpawnRandomAsteroid(new Vector3(UnityEngine.Random.Range(-200, 200), UnityEngine.Random.Range(-200, 200), UnityEngine.Random.Range(-20, -50)));
            }
            asteroidEntities.Dispose();
        }

        public void SpawnRandomAsteroid(Vector3 position)
        {
            var index = UnityEngine.Random.Range(0, asteroidPrefabs.Length);
            var asteroidEntity = asteroidEntities[index];
            Entity asteroid = entityManager.Instantiate(asteroidEntity);
            entityManager.SetComponentData(asteroid, new Translation { Value = position });
            entityManager.SetComponentData(asteroid, new Rotation { Value = UnityEngine.Random.rotation });
            entityManager.AddComponentData(asteroid, new Speed { Value = UnityEngine.Random.Range(0.1f, 2f) });
            entityManager.AddComponentData(asteroid, new RotateSpeed { Value = UnityEngine.Random.Range(.1f, 1f) });
            entityManager.AddComponentData(asteroid, new AsteroidTag());
        }
    }
}