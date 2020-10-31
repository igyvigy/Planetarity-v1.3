using System;
using System.Collections.Generic;
using UnityEngine;

namespace Planetarity.UI
{
    public class UIProgressBar : MonoBehaviour
    {
        private Dictionary<ArtileryCommander, ProgressBar> progressBars = new Dictionary<ArtileryCommander, ProgressBar>();

        private void Start() {
            Camera.main.GetComponent<CameraController>().OnCameraZoomChanged += HandleCameraZoomChanged;
        }

        public void SubscribeOnArtileryCommander(ArtileryCommander commander)
        {
            commander.OnProgressAdded += AddProgressBar;
            commander.OnProgressRemoved += RemoveProgressBar;
        }

        private void HandleCameraZoomChanged(float zoom)
        {
            foreach (var commander in progressBars.Keys)
            {
                progressBars[commander].SetZoom(zoom);
            }
        }

        private void AddProgressBar(ArtileryCommander commander)
        {
            if (!progressBars.ContainsKey(commander))
            {
                var progressBar = Instantiate(GameAssets.i.pfProgressBar, transform);
                progressBars.Add(commander, progressBar);
                progressBar.SetArtileryCommander(commander);
            }
        }

        private void RemoveProgressBar(ArtileryCommander commander)
        {
            if (progressBars.ContainsKey(commander))
            {
                Destroy(progressBars[commander].gameObject);
                progressBars.Remove(commander);
            }
        }
    }
}
