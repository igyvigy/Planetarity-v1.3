using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;
using Unity.Mathematics;

public class MovementSystem : SystemBase
{
    protected override void OnUpdate()
    {
        float dt = Time.DeltaTime;

        Entities.
        WithAny<RocketTag>()
        .ForEach((ref Translation position, ref Rotation rotation, ref Speed speed) =>
        {
            float3x3 rot3x3 = new float3x3(rotation.Value);
            // c0 - right, c1 - up, c2 - forward
            float3 up = rot3x3.c1;
            float3 forward = rot3x3.c2;
            position.Value += up * speed.Value * dt;
        }).ScheduleParallel();
    }
}
