﻿using UnityEngine;
namespace Planetarity
{
    [RequireComponent(typeof(Damageable))]
    [RequireComponent(typeof(ArtileryCommander))]
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(SphereCollider))]
    public class Planet : MonoBehaviour
    {
        [Header("stats")]
        public string planetName;
        public Color color;
        public float size;
        public float density;
        public float mass;
        public float orbitSpeed;

        private Rigidbody rb;
        private void Start() {
            rb = GetComponent<Rigidbody>();
            rb.useGravity = false;
            rb.isKinematic = true;
        }

        private void Update()
        {
            transform.Rotate(transform.forward * -orbitSpeed * Time.deltaTime);
            transform.RotateAround(Vector3.zero, Vector3.forward, orbitSpeed * Time.deltaTime);
        }

        public void Configure(string name, Color color, float size, float density, float orbitSpeed, float? mass = null)
        {
            planetName = name;
            this.color = color;
            this.size = size;
            this.density = density;
            this.orbitSpeed = orbitSpeed;
            if (mass != null)
            {
                this.mass = mass.Value;
            }
            else
            {
                // calculate planet's mass by size and density
                float volume = (4 / 3) * Mathf.PI * (Mathf.Pow((size / 2), 3));
                this.mass = density * volume;
            }
        }

        public float GetGravity(Vector3 pos, float mass)
        {
            float distance = Vector3.Distance(pos, transform.position);
            var graviForce = Constants.k_GraviConst * (this.mass * mass) / Mathf.Pow(distance, 2);
            return -graviForce;
        }
    }
}