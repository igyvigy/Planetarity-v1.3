using UnityEngine;

namespace Planetarity
{
    public class AIPlanet : MonoBehaviour
    {
        private enum AIState
        {
            EnemySearch, Aiming, HoldingFire, OnCoolDown
        }

        private ArtileryCommander artileryCommander;
        private ShieldCommander shieldCommander;
        private Damageable damageable;
        [SerializeField] private Damageable target;
        [SerializeField] private AIState state = AIState.EnemySearch;

        private const float aimQualifier = 99f;
        private float? recentAim;
        private float aim = 0;
        private bool aimCW = true;
        private float aimDiff
        {
            get
            {
                var preAim = recentAim == null ? 0 : recentAim.Value;
                return aim - preAim;
            }
        }

        private bool aimed = false;
        private Vector3? targetDirection;

        private void Start()
        {
            artileryCommander = GetComponent<ArtileryCommander>();
            damageable = GetComponent<Damageable>();
            shieldCommander = GetComponent<ShieldCommander>();
        }

        private void Update()
        {

            if (target != null)
            {
                Debug.DrawLine(transform.position, target.transform.position, Color.green);
            }
            Debug.DrawRay(transform.position, artileryCommander.aimDirection * 10, Color.red);
            if (targetDirection != null)
            {
                Debug.DrawRay(transform.position, targetDirection.Value * 10, Color.blue);
            }


            if (damageable.isDead) return;

            if (target != null && target.isDead)
            {
                target = null;
                state = AIState.EnemySearch;
            }

            AimIfNeeded();

            switch (state)
            {
                case AIState.EnemySearch:
                    var planets = GameManager.i.GetAlivePlanets();
                    if (planets.Length == 0) return;
                    Planet nearestPlanet = null;
                    foreach (var planet in planets)
                    {
                        if (planet.gameObject.name == this.gameObject.name)
                        {
                            continue;
                        }
                        var distance = Vector3.Distance(transform.position, planet.transform.position);
                        if (nearestPlanet == null)
                        {
                            nearestPlanet = planet;
                        }
                        else
                        {
                            var distanceToNearestPlanet = Vector3.Distance(transform.position, nearestPlanet.transform.position);
                            if (distance < distanceToNearestPlanet)
                            {
                                nearestPlanet = planet;
                            }
                        }
                    }
                    target = nearestPlanet.GetComponent<Damageable>();
                    state = AIState.Aiming;
                    break;

                case AIState.Aiming:
                    if (aimed) // aimed
                    {
                        state = AIState.HoldingFire;
                    }
                    break;
                case AIState.HoldingFire:
                    if (!artileryCommander.isHoldingFire)
                    {
                        artileryCommander.HoldFire();
                    }
                    else
                    {
                        if (artileryCommander.currentProgressPercent <= .1f) return;
                        else
                        {
                            artileryCommander.ReleaseFire();
                            state = AIState.OnCoolDown;
                        }
                    }
                    break;
                case AIState.OnCoolDown:

                    if (artileryCommander.isOnCooldown) return;
                    else
                    {
                        state = AIState.EnemySearch;
                    }

                    break;
            }
        }

        private void AimIfNeeded()
        {
            const float aimThreshold = 5f;
            if (target == null) return;
            targetDirection = (target.transform.position - transform.position).normalized;
            aim = Vector3.Dot(artileryCommander.aimDirection * 10, targetDirection.Value * 10);
            aimed = aim >= aimQualifier;
            if (aimed) return;

            if (recentAim != null)
            {
                if (aim > 0 && recentAim.Value > 0) // gun is facing to target
                {
                    if (recentAim.Value - aim >= aimThreshold) // bad aim direction. Toggle
                    {
                        aimCW = !aimCW;
                    }
                }
            }
            if (aimCW) artileryCommander.MoveArtileryClockwise();
            else artileryCommander.MoveArtileryCounterClockwise();

            if (Mathf.Abs(aimDiff) >= aimThreshold)
            {
                recentAim = Vector3.Dot(artileryCommander.aimDirection * 10, targetDirection.Value * 10);
            }
        }
    }
}

