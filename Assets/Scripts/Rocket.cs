using UnityEngine;

namespace Planetarity
{
    public class Rocket : MonoBehaviour
    {
        public static Rocket Create(string name, Vector3 position, Quaternion rotation, float force, string owner, float size)
        {
            Transform rocketTransform = Instantiate(GameAssets.i.pfRocket, position, rotation);
            rocketTransform.localScale = new Vector3(size, size, size);
            var rocket = rocketTransform.GetComponent<Rocket>();
            rocket.Configure(Utilities.GetRocketTypeByName(name), force, owner);
            return rocket;
        }
        public RocketSO type;
        public string owner;
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

        private void FixedUpdate()
        {
            rb.AddForce(transform.up * force, ForceMode.Impulse);
            rb.AddForce(transform.up * type.thrust);
            ApplySunGravity();
            ApplyPlanetsGravity();
        }

        public void Configure(RocketSO type, float force, string owner)
        {
            this.type = type;
            this.force = force;
            this.owner = owner;
            this.disappearTimer = type.timeToLive;
        }

        private void Explode()
        {
            Explosion.Create(transform.position, 0.2f * type.size);
            SoundManager.ExplodeRocket(type.name);
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
            foreach (Planet planet in GameManager.i.GetAlivePlanets())
            {
                if (planet.gameObject.name == owner) continue;
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
                string playerName = GameManager.i.playerName;
                if (owner == playerName && other.gameObject.name != playerName) GameManager.i.IncreasePlayerScore(type.damage, other.transform);
                damageable.ReceiveDamage(type.damage);
                if (owner == playerName && other.gameObject.name != playerName && damageable.isDead) GameManager.i.IncreasePlayerKills();
                Explode();
            }
            else if (other.gameObject.GetComponent<Shield>() != null)
            {
                Explode();
            }
            else
            {
                Debug.LogFormat("rocket did hit {0}", other.gameObject.name);
            }
        }
    }
}