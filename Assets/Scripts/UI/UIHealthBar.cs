using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Planetarity.UI
{
    public class UIHealthBar : MonoBehaviour
    {
        private Dictionary<Damageable, HealthBar> healthBars = new Dictionary<Damageable, HealthBar>();

        private void Start()
        {
            Camera.main.GetComponent<CameraController>().OnCameraZoomChanged += HandleCameraZoomChanged;
        }

        public void SubscribeOnDamageable(Damageable damageable)
        {
            damageable.OnHealthAdded += AddHealthBar;
            damageable.OnHealthRemoved += RemoveHealthBar;
        }

        private void HandleCameraZoomChanged(float zoom)
        {
            foreach (var damageable in healthBars.Keys)
            {
                healthBars[damageable].SetZoom(zoom);
            }
        }

        private void AddHealthBar(Damageable damageable)
        {
            if (!healthBars.ContainsKey(damageable))
            {
                var healthBar = Instantiate(GameAssets.i.pfHealthBar, transform);
                healthBars.Add(damageable, healthBar);
                healthBar.SetDamageable(damageable);
            }
        }

        private void RemoveHealthBar(Damageable damageable)
        {
            if (healthBars.ContainsKey(damageable))
            {
                Destroy(healthBars[damageable].gameObject);
                healthBars.Remove(damageable);
            }
        }
    }
}
