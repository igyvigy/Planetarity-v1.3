using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public class RotationSystem : SystemBase
{
    protected override void OnUpdate()
    {
        float deltaTime = Time.DeltaTime;

        Entities.
            WithAll<AsteroidTag>().
            ForEach((ref Rotation rotation, in RotateSpeed rotateSpeed) =>
            {
                quaternion normalizedRotation = math.normalize(rotation.Value);
                quaternion angleToRotate = quaternion.AxisAngle(math.up(), rotateSpeed.Value * deltaTime);

                rotation.Value = math.mul(normalizedRotation, angleToRotate);

            }).ScheduleParallel();
    }
}