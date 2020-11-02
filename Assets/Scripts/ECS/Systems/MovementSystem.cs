using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;
using Unity.Mathematics;

namespace Planetarity.ECS
{
    public class MovementSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            float dt = Time.DeltaTime;
            float upperBounds = GameManager.i.UpperBounds;
            float lowerBounds = GameManager.i.LowerBounds;
            float leftBounds = GameManager.i.LeftBounds;
            float rightBounds = GameManager.i.RightBounds;
            Entities.WithAny<AsteroidTag>().ForEach((ref Translation position, ref Rotation rotation, ref Speed speed) =>
            {
                float initialZ = position.Value.z;
                float3x3 rot3x3 = new float3x3(rotation.Value);
                // c0 - right, c1 - up, c2 - forward
                float3 up = rot3x3.c1;
                float3 forward = rot3x3.c2;
                position.Value += up * speed.Value * dt;

                if (position.Value.y >= upperBounds)
                {
                    position.Value.y = lowerBounds;
                }
                if (position.Value.y <= lowerBounds)
                {
                    position.Value.y = upperBounds;
                }
                if (position.Value.x >= rightBounds)
                {
                    position.Value.x = leftBounds;
                }
                if (position.Value.x <= leftBounds)
                {
                    position.Value.x = rightBounds;
                }
                position.Value.z = initialZ;
            }).ScheduleParallel();
        }
    }
}
