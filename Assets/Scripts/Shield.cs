using UnityEngine;

namespace Planetarity
{
    public class Shield : MonoBehaviour
    {
        public bool isDead = false;
        private float maxHealth;
        public float health;

        private void Update()
        {
            if (isDead)
            {
                gameObject.SetActive(false);
            }
        }

        public void SetMaxHealth(float health)
        {
            this.maxHealth = health;
            this.health = maxHealth;
        }
        public void ReceiveDamage(float damage)
        {
            health -= damage;
            if (health <= 0)
            {
                health = 0;
                isDead = true;
            }
        }
    }
}