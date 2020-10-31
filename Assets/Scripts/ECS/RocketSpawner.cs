using Unity.Entities;
using UnityEngine;
using Unity.Transforms;


namespace Planetarity.ECS
{
    public class RocketSpawner : MonoBehaviour
    {
        [SerializeField] GameObject rocketPrefab;

        private EntityManager entityManager;
        private World defaultWorld;
        private EntityArchetype rocketArchetype;
        private Entity rocketEntity;

        private void Start()
        {
            defaultWorld = World.DefaultGameObjectInjectionWorld;
            entityManager = defaultWorld.EntityManager;

            GameObjectConversionSettings settings = GameObjectConversionSettings.FromWorld(defaultWorld, null);
            rocketEntity = GameObjectConversionUtility.ConvertGameObjectHierarchy(rocketPrefab, settings);
        }

        public void SpawnRocket(string name, Vector3 position, Quaternion rotation)
        {
            Entity rocket = entityManager.Instantiate(rocketEntity);
            RocketSO type = Utilities.GetRocketTypeByName(name);
            entityManager.SetComponentData(rocket, new Translation { Value = position });
            entityManager.SetComponentData(rocket, new Rotation { Value = rotation });
            entityManager.SetComponentData(rocket, new Speed { Value = type.thrust });
        }
    }
}