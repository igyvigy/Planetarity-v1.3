using System;
using UnityEngine;


namespace Planetarity
{

    /// <summary>
    /// Behaviour responsible for controlling over an artilery. Aiming, shooting, etc.
    /// </summary>
    [RequireComponent(typeof(Planet))]
    public class ArtileryCommander : MonoBehaviour
    {
        public event Action<ArtileryCommander> OnProgressAdded = delegate { };
        public event Action<ArtileryCommander> OnProgressRemoved = delegate { };
        public event Action<float> OnProgressPercentChaged = delegate { };
        public float currentProgress { get; private set; }
        public bool isHoldingFire { get; private set; } = false;

        private Planet planet;
        private Artilery artilery;
        private string rocketName;
        private RocketSO rocketType;
        private float currentMinProgress;
        private float currentMaxProgress;
        private bool isOnCooldown = false;
        private void Start()
        {
            planet = GetComponent<Planet>();
            SpawnArtilery();
        }

        private void Update() {
            if (isHoldingFire)
            {
                SetProgress(currentProgress + Time.deltaTime * rocketType.forceGain);
            }
            if (isOnCooldown)
            {
                SetProgress(currentProgress - Time.deltaTime);
                if (currentProgress == 0)
                {
                    isOnCooldown = false;
                    HideProgress();
                }
            }
        }

        private void SpawnArtilery()
        {
            Transform artileryTransform = Instantiate(GameAssets.i.pfArtilery, new Vector3(transform.position.x, transform.position.y + planet.size, transform.position.z), Quaternion.identity);
            artileryTransform.SetParent(transform);
            artilery = artileryTransform.GetComponent<Artilery>();
        }

        public void ShowProgress()
        {
            OnProgressAdded(this);
        }

        public void HideProgress()
        {
            OnProgressRemoved(this);
        }

        public void SetProgress(float value)
        {
            currentProgress = value;
            currentProgress = Mathf.Clamp(currentProgress, currentMinProgress, currentMaxProgress);
            float progressPercent = currentProgress / currentMaxProgress;
            OnProgressPercentChaged(progressPercent);
        }

        public void MoveArtileryClockwise()
        {
            artilery.transform.RotateAround(transform.position, Vector3.forward, -Constants.k_ArtileryMoveSpeed * Time.deltaTime);
        }

        public void MoveArtileryCounterClockwise()
        {
            artilery.transform.RotateAround(transform.position, Vector3.forward, Constants.k_ArtileryMoveSpeed * Time.deltaTime);
        }

        public void SetWeapon(string name)
        {
            this.rocketName = name;
            this.rocketType = Utilities.GetRocketTypeByName(name);
        }

        public void HoldFire()
        {
            if (!CanFire()) return;
            isHoldingFire = true;
            currentMinProgress = rocketType.minForce;
            currentMaxProgress = rocketType.maxForce;
            ShowProgress();
            SetProgress(0);
        }

        public void ReleaseFire()
        {
            if (!isHoldingFire) return;
            isHoldingFire = false;
            Fire(currentProgress);
            isOnCooldown = true;
            currentMinProgress = 0;
            currentMaxProgress = rocketType.cooldown;
            SetProgress(currentMaxProgress);
        }

        public bool CanFire()
        {
            return !isOnCooldown;
        }

        public void Fire(float force)
        {
            if (!CanFire()) return;
            var gun = artilery.transform.Find("Gun");
            var firePoint = gun.Find("FirePoint");
            Rocket.Create(rocketName, firePoint.position, gun.rotation, force);
        }
    }
}