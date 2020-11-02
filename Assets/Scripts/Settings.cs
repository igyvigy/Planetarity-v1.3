using UnityEngine;

namespace Planetarity
{
    public class Settings : MonoBehaviour
    {
        public static Settings i;

        private void Awake()
        {
            if (i == null)
            {
                i = this;
            }
        }
        [HideInInspector] public bool shouldLoadGameState = false; 
        public int minOpponentsCount = 1;
        public int maxOpponentsCount = 10;
    }
}