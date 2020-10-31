using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Planetarity.UI
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private Image foregroundImage;
        [SerializeField] private float updateSpeedSeconds = 0.5f;
        [SerializeField] private float positionOffset = 0f;

        private Damageable damageable;
        private float planetSize;
        public void SetDamageable(Damageable damageable)
        {
            this.damageable = damageable;
            this.planetSize = damageable.GetComponent<Planet>().size;
            this.damageable.OnHealthPercentChaged += HandleHealthPercentChanged;
        }

        public void SetZoom(float cameraZoom)
        {
            var scale = cameraZoom + .2f;
            var rect = GetComponent<RectTransform>();
            // rect.localScale = new Vector3(scale, scale, scale);
            var planetRect = Utilities.GetScreenRect(damageable.gameObject);
            var side = planetRect.width * 4;
            rect.sizeDelta = new Vector2(side, side);
        }

        private void HandleHealthPercentChanged(float health)
        {
            StartCoroutine(ChangeToPct(health));
        }

        private IEnumerator ChangeToPct(float pct)
        {
            float preChangePct = foregroundImage.fillAmount;
            float elapsed = 0f;

            while (elapsed < updateSpeedSeconds)
            {
                elapsed += Time.deltaTime;
                foregroundImage.fillAmount = Mathf.Lerp(preChangePct, pct, elapsed / updateSpeedSeconds);
                yield return null;
            }
            foregroundImage.fillAmount = pct;
        }

        private void LateUpdate()
        {
            transform.position = Camera.main.WorldToScreenPoint(damageable.transform.position + Vector3.up * positionOffset);
        }

        private void OnDestroy()
        {
            damageable.OnHealthPercentChaged -= HandleHealthPercentChanged;
        }
    }
}