using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Planetarity.UI
{
    public class ProgressBar : MonoBehaviour
    {
        [SerializeField] private Image foregroundImage;
        [SerializeField] private float updateSpeedSeconds = 0.5f;
        [SerializeField] private float positionOffset = 0f;

        private ArtileryCommander commander;
        private float planetSize;
        public void SetArtileryCommander(ArtileryCommander commander)
        {
            this.commander = commander;
            this.planetSize = commander.GetComponent<Planet>().size;
            this.commander.OnProgressPercentChaged += HandleProgressPercentChanged;
        }

        public void SetZoom(float cameraZoom)
        {
            var scale = cameraZoom + .2f;
            var rect = GetComponent<RectTransform>();
            rect.localScale = new Vector3(scale, scale, scale);
            var planetRect = Utilities.GetScreenRect(commander.gameObject);
            rect.sizeDelta = new Vector2(planetRect.width * 1.4f, rect.sizeDelta.y);
        }

        private void HandleProgressPercentChanged(float progress)
        {
            foregroundImage.fillAmount = progress;
        }

        private void LateUpdate()
        {
            transform.position = Camera.main.WorldToScreenPoint(commander.transform.position + Vector3.up * positionOffset);
        }

        private void OnDestroy()
        {
            commander.OnProgressPercentChaged -= HandleProgressPercentChanged;
        }
    }
}