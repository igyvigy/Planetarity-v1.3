using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Planetarity.UI
{
    public class ProgressBar : MonoBehaviour
    {
        [SerializeField] private Image foregroundImage;
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
            var scale = cameraZoom + 0.2f;
            var rect = GetComponent<RectTransform>();
            // rect.localScale = new Vector3(1, scale, 1);
            var planetRect = Utilities.GetScreenRect(commander.gameObject);
            var rectSide = planetRect.width * 2.6f;
            rect.sizeDelta = new Vector2(rectSide, rectSide);
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