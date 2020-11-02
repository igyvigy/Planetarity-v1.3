using UnityEngine;

namespace Planetarity
{
    [RequireComponent(typeof(ArtileryCommander))]
    public class PlayerPlanet : MonoBehaviour
    {
        private ArtileryCommander artileryCommander;
        private ShieldCommander shieldCommander;
        private void Start()
        {
            artileryCommander = GetComponent<ArtileryCommander>();
            shieldCommander = GetComponent<ShieldCommander>();
        }

        private void Update()
        {
            if (GameManager.i.isPaused) return;
            
            if (Input.GetAxis("Horizontal") > 0)
            {
                artileryCommander.MoveArtileryClockwise();
            }
            if (Input.GetAxis("Horizontal") < 0)
            {
                artileryCommander.MoveArtileryCounterClockwise();
            }
            if (Input.GetAxis("Vertical") > 0)
            {
                shieldCommander.MoveShieldClockwise();
            }
            if (Input.GetAxis("Vertical") < 0)
            {
                shieldCommander.MoveShieldCounterClockwise();
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                artileryCommander.HoldFire();
            }
            if (Input.GetKeyUp(KeyCode.Space))
            {
                artileryCommander.ReleaseFire();
            }
        }
    }
}