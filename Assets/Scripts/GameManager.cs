using System.Collections;
using System.Collections.Generic;
using Planetarity.UI;
using UnityEngine;

namespace Planetarity
{
    /// <summary>
    /// Behaviour responsible for controlling main game logic
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        public static GameManager i;

        [Header("config")]
        public Material sunMaterial;
        public RocketSO[] rocketTypes;
        public UIProgressBar uiProgressBar;
        public UIHealthBar uiHealthBar;

        [Header("stats")]
        public bool isPaused = false;
        public int planetsCount;
        public Sun sun;
        public List<Planet> planets = new List<Planet>();
        

        private SphereGenerator sphereGenerator;
        private CameraController cameraController;
        
        private int playerPlanetIndex;
        private Settings settings;

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
            sphereGenerator = GetComponent<SphereGenerator>();
            settings = GameObject.FindGameObjectWithTag(Constants.k_TagSettings).GetComponent<Settings>();
            cameraController = Camera.main.GetComponent<CameraController>();
        }

        private void Start()
        {
            CreateSun();
            CreatePlanets();
            AssignPlayerToAPlanet();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                isPaused = !isPaused;
            }
            Time.timeScale = isPaused ? 0 : 1;
        }

        #region PlanetsGeneration
        private void CreateSun()
        {
            GameObject sunGo = sphereGenerator.CreatePlanet(sunMaterial, Constants.k_SunSize);
            sunGo.name = Constants.k_SunName;

            // add sun light
            GameObject lightGameObject = new GameObject("Sun Light");
            Light lightComp = lightGameObject.AddComponent<Light>();
            lightComp.color = Constants.k_WhiteColor;
            lightComp.range = Constants.k_SunLightRange;
            lightComp.intensity = Constants.k_SunLightIntencity;
            lightGameObject.transform.parent = sunGo.transform;
            lightGameObject.transform.position = Vector3.zero;

            Sun sun = sunGo.AddComponent<Sun>();

            this.sun = sun;
        }

        private void CreatePlanets()
        {
            planetsCount = Random.Range(settings.minOpponentsCount, settings.maxOpponentsCount) + 1;

            for (var i = 1; i <= planetsCount; i++)
            {
                float planetSize = Random.Range(Constants.k_MinPlanetSize, Constants.k_MaxPlanetSize);
                Color planetColor = GetUniqueRandomColorForAPlanet();

                GameObject planet = sphereGenerator.CreatePlanet(planetColor, planetSize);
                planet.name = "Planet " + i.ToString();

                Planet planetComp = planet.AddComponent<Planet>();
                planetComp.Configure(planet.name, planetColor, planetSize);

                planet.transform.position = new Vector3(14 * i, 0, 0); //set orbit
                planet.transform.RotateAround(Vector3.zero, Vector3.forward, Random.Range(0, 360)); //place on random point on the orbit

                ArtileryCommander commander = planet.GetComponent<ArtileryCommander>();
                uiProgressBar.SubscribeOnArtileryCommander(commander);

                Damageable damageable = planet.GetComponent<Damageable>();
                uiHealthBar.SubscribeOnDamageable(damageable);

                planets.Add(planetComp);
            }
        }

        private Color GetUniqueRandomColorForAPlanet()
        {
            Color randomColor;
            bool CheckIfColorTakenByAPlanet(Color color)
            {
                var taken = false;
                foreach (var planet in planets)
                    if (planet.color == color)
                        taken = true;
                return taken;
            }
            do
                randomColor = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
            while (CheckIfColorTakenByAPlanet(randomColor));
            return randomColor;
        }

        #endregion


        #region Preparation
        private void AssignPlayerToAPlanet()
        {
            playerPlanetIndex = Random.Range(0, planets.Count);
            GameObject playerPlanetGo = planets[playerPlanetIndex].gameObject;
            playerPlanetGo.AddComponent<PlayerPlanet>();
            cameraController.SetTarget(playerPlanetGo.transform, false);
        }

        #endregion
    }

}
