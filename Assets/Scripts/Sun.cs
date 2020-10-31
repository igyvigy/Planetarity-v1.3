using UnityEngine;

[RequireComponent(typeof(SphereCollider))]

public class Sun : MonoBehaviour
{
    public float GetGravity(Vector3 pos, float mass)
    {
        float distance = Vector3.Distance(pos, transform.position);
        var graviForce = Constants.k_GraviConst * (Constants.k_SunMass * mass) / Mathf.Pow(distance, 2);
        return -graviForce;
    }
}
