using System;
using UnityEngine;

namespace Planetarity
{
    /// <summary>
    /// Behaviour responsible for dealing damage to a planet
    /// </summary>
    [RequireComponent(typeof(Planet))]
    public class Damageable : MonoBehaviour
    {
        public event Action<Damageable> OnHealthAdded = delegate { };
        public event Action<Damageable> OnHealthRemoved = delegate { };
        public event Action<float> OnHealthPercentChaged = delegate { };

        public float health = 0;
        public bool isDead = false;
        private float maxHealth;
        private Planet planet;

        private void Start()
        {
            planet = GetComponent<Planet>();
            if (health > 0)
            {
                ShowHealthBar();
                if (health != maxHealth)
                {
                    OnHealthPercentChaged(health / maxHealth);
                }
            }
        }
        private void Update()
        {
            if (isDead)
            {
                Explode();
            }
        }

        public void SetHealth(float maxHealth, float health)
        {
            this.maxHealth = maxHealth;
            this.health = health;
            ShowHealthBar();
        }

        public void ReceiveDamage(float damage)
        {
            health -= damage;
            if (health <= 0)
            {
                health = 0;
                isDead = true;
                HideHealthBar();
            }
            OnHealthPercentChaged(health / maxHealth);
        }

        public void ShowHealthBar()
        {
            OnHealthAdded(this);
        }

        public void HideHealthBar()
        {
            OnHealthRemoved(this);
        }

        private void Explode()
        {
            Explosion.Create(transform.position, planet.size);
            SoundManager.ExplodePlanet();
            gameObject.SetActive(false);
        }
    }
}