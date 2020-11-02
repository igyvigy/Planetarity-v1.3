using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Planetarity
{

    /// <summary>
    /// Behaviour responsible for controlling over shield
    /// </summary>
    [RequireComponent(typeof(Planet))]
    public class ShieldCommander : MonoBehaviour
    {
        private Planet planet;
        private Shield shield;
        private void Start()
        {
            planet = GetComponent<Planet>();
            SpawnShield();
        }

        private void SpawnShield()
        {
            Transform shieldTransform = Instantiate(GameAssets.i.pfShield, new Vector3(transform.position.x, transform.position.y + planet.size * 1.7f, transform.position.z), Quaternion.identity);
            shieldTransform.RotateAround(transform.position, Vector3.forward, 180);
            shieldTransform.SetParent(transform);
            float shieldHealth = Constants.k_ShieldHealth;
            shield = shieldTransform.GetComponent<Shield>();
            shield.SetMaxHealth(shieldHealth);
        }

        public void MoveShieldClockwise()
        {
            if (!shield.gameObject.activeInHierarchy) return;
            shield.transform.RotateAround(transform.position, Vector3.forward, -Constants.k_ShieldMoveSpeed * Time.deltaTime);
        }

        public void MoveShieldCounterClockwise()
        {
            if (!shield.gameObject.activeInHierarchy) return;
            shield.transform.RotateAround(transform.position, Vector3.forward, Constants.k_ShieldMoveSpeed * Time.deltaTime);
        }
    }
}