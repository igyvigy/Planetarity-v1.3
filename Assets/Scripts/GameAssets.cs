using Planetarity.UI;
using UnityEngine;

namespace Planetarity
{
    public class GameAssets : MonoBehaviour
    {
        public static GameAssets i;

        private void Awake()
        {
            if (i == null)
            {
                i = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public Sprite pointerSprite;
        public Transform pfArtilery;
        public Transform pfShield;
        public Transform pfRocket;
        public GameObject pfExplosion;
        public ProgressBar pfProgressBar;
        public HealthBar pfHealthBar;
        
    }
}