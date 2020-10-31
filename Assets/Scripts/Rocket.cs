using UnityEngine;

namespace Planetarity
{
    public class Rocket : MonoBehaviour
    {
        public static Rocket Create(string name, Vector3 position, Quaternion rotation, float force)
        {
            Transform rocketTransform = Instantiate(GameAssets.i.pfRocket, position, rotation);
            var rocket = rocketTransform.GetComponent<Rocket>();
            rocket.Configure(Utilities.GetRocketTypeByName(name), force);
            return rocket;
        }
        public RocketSO type;
        private float disappearTimer;
        private Rigidbody rb;
        private float timeElapsed;
        private float force;

        private void Start() {
            rb = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            disappearTimer -= Time.deltaTime;
            if (disappearTimer < 0)
            {
                Destroy(gameObject);
            }
        }

        private void FixedUpdate() {

            rb.AddForce(transform.up * force, ForceMode.Impulse); //apply launch impulse
            rb.AddForce(transform.up * type.thrust); // apply thrust
            ApplySunGravity();
            ApplyPlanetsGravity();
        }

        public void Configure(RocketSO type, float force)
        {
            this.type = type;
            this.force = force;
            this.disappearTimer = type.timeToLive;
        }

        private void Explode()
        {
            Explosion.Create(transform.position, 0.2f);
            Destroy(gameObject);
        }

        private void ApplySunGravity()
        {
            Sun sun = GameManager.i.sun;
            Vector3 direction = (transform.position - sun.transform.position).normalized;
            rb.AddForce(direction * sun.GetGravity(transform.position, type.mass));
        }

        private void ApplyPlanetsGravity()
        {
            foreach (Planet planet in GameManager.i.planets)
            {
                Vector3 direction = (transform.position - planet.transform.position).normalized;
                rb.AddForce(direction * planet.GetGravity(transform.position, type.mass));
            }
            
        }

        private void OnCollisionEnter(Collision other) {

            if (other.gameObject.GetComponent<Sun>() != null)
            {
                Destroy(gameObject);
            }
            else if (other.gameObject.GetComponent<Damageable>() != null)
            {
                var damageable = other.gameObject.GetComponent<Damageable>();
                damageable.ReceiveDamage(type.damage);
                Explode();
            }
        }
    }
}