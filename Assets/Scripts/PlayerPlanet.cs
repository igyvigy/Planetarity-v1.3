using UnityEngine;

namespace Planetarity
{
    [RequireComponent(typeof(ArtileryCommander))]
    public class PlayerPlanet : MonoBehaviour
    {
        private ArtileryCommander artileryCommander;
        
        private void Start()
        {
            artileryCommander = GetComponent<ArtileryCommander>();
            artileryCommander.SetWeapon("Wasp");
        }

        private void Update()
        {
            if (Input.GetAxis("Horizontal") > 0)
            {
                artileryCommander.MoveArtileryClockwise();
            }
            if (Input.GetAxis("Horizontal") < 0)
            {
                artileryCommander.MoveArtileryCounterClockwise();
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