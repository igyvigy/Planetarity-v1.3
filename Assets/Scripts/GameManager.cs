using System.Collections;
using System.Collections.Generic;
using Planetarity.UI;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

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
        public UIPointer uIPointer;
        public RectTransform pauseUI;
        public RectTransform gameOverUI;
        public RectTransform winGameUI;
        [SerializeField] private float upperBounds;
        [SerializeField] private float leftBounds;
        [SerializeField] private float rightBounds;
        [SerializeField] private float lowerBounds;

        public float UpperBounds => upperBounds;
        public float LowerBounds => lowerBounds;
        public float LeftBounds => leftBounds;
        public float RightBounds => rightBounds;

        public TextMeshProUGUI scoreLabel;
        public TextMeshProUGUI killsLabel;
        public TextMeshProUGUI playerNameLabel;
        public TextMeshProUGUI playerWeaponLabel;

        [Header("stats")]
        public bool isPaused = false;
        public bool gameIsOwer = false;
        public bool gameWon = false;

        public int planetsCount;
        public int score = 0;
        public int kills = 0;
        public string playerName;
        public string playerWeaponName;

        [HideInInspector] public Sun sun;
        private List<GameObject> planets = new List<GameObject>();
        private GameObject playerPlanet
        {
            get
            {
                return planets[playerPlanetIndex];
            }
        }
        private SphereGenerator sphereGenerator;
        private CameraController cameraController;
        private int playerPlanetIndex;

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
            cameraController = Camera.main.GetComponent<CameraController>();
        }

        private void Start()
        {
            CreateSun();
            if (Settings.i.shouldLoadGameState)
            {
                Settings.i.shouldLoadGameState = false;
                LoadGameState();
            }
            else
            {
                CreateNewPlanets();
                AssignPlayerToARandomPlanet();
                AssignAIToOtherPlanetsRandomly();
                DistributeWeaponsRandomly();
            }
            
            UpdateHUD();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape) && !gameIsOwer)
            {
                isPaused = !isPaused;
                pauseUI.gameObject.SetActive(isPaused);
            }
            if (playerPlanet.GetComponent<Damageable>().isDead && !gameIsOwer)
            {
                gameIsOwer = true;
                gameOverUI.gameObject.SetActive(true);
            }
            if (GetAliveEnemyPlanets().Length == 0 && !gameWon)
            {
                gameWon = true;
                isPaused = true;
                winGameUI.gameObject.SetActive(true);
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

        private void CreateNewPlanets()
        {
            planetsCount = Random.Range(Settings.i.minOpponentsCount, Settings.i.maxOpponentsCount) + 1;

            for (var i = 1; i <= planetsCount; i++)
            {
                float planetSize = Random.Range(Constants.k_MinPlanetSize, Constants.k_MaxPlanetSize);
                Color planetColor = GetUniqueRandomColorForAPlanet();

                GameObject planet = sphereGenerator.CreatePlanet(planetColor, planetSize);
                planet.name = "Planet " + i.ToString();

                Planet planetComp = planet.AddComponent<Planet>();
                float density = Constants.k_MaxPlanetDensity;
                float orbitSpeed = Random.Range(Constants.k_MinPlanetOrbitSpeed, Constants.k_MaxPlanetOrbitSpeed);
                planetComp.Configure(planet.name, planetColor, planetSize, density, orbitSpeed);

                planet.transform.position = new Vector3(14 * i, 0, 0); //set orbit
                planet.transform.RotateAround(Vector3.zero, Vector3.forward, Random.Range(0, 360)); //place on random point on the orbit

                ArtileryCommander commander = planet.GetComponent<ArtileryCommander>();
                uiProgressBar.SubscribeOnArtileryCommander(commander);

                Damageable damageable = planet.GetComponent<Damageable>();
                float planetHealth = planetSize * planetComp.mass;
                damageable.SetHealth(planetHealth, planetHealth);
                uiHealthBar.SubscribeOnDamageable(damageable);

                planets.Add(planet);
            }
        }

        private Color GetUniqueRandomColorForAPlanet()
        {
            Color randomColor;
            bool CheckIfColorTakenByAPlanet(Color color)
            {
                var taken = false;
                foreach (var planet in planets)
                    if (planet.GetComponent<Planet>().color == color)
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
        private void AssignPlayerToARandomPlanet()
        {
            playerPlanetIndex = Random.Range(0, planets.Count);
            GameObject playerPlanetGo = planets[playerPlanetIndex];
            playerPlanetGo.AddComponent<PlayerPlanet>();
            cameraController.SetTarget(playerPlanetGo.transform, false);
            playerPlanetGo.AddComponent<ShieldCommander>(); // player has shield
            playerName = playerPlanetGo.name;
            uIPointer.SetPlayer(playerPlanetGo.transform);
        }

        private void AssignAIToOtherPlanetsRandomly()
        {
            List<UIPointerTarget> pointerTargets = new List<UIPointerTarget>();
            foreach (var planet in planets)
                if (planet.GetComponent<PlayerPlanet>()) continue;
                else
                {
                    planet.AddComponent<AIPlanet>();
                    pointerTargets.Add(new UIPointerTarget(planet.transform, GameAssets.i.pointerSprite));
                }
            uIPointer.SetTargets(pointerTargets);
        }

        private void DistributeWeaponsRandomly()
        {
            foreach (var planet in planets)
            {
                string name = Utilities.GetRandomRocketName();
                if (planet.name == playerName) playerWeaponName = name;
                planet.GetComponent<ArtileryCommander>().SetWeapon(name);
            }
        }

        private void UpdateHUD()
        {
            scoreLabel.text = score.ToString();
            killsLabel.text = kills.ToString();
            playerNameLabel.text = playerName;
            playerWeaponLabel.text = playerWeaponName;
        }

        #endregion


        #region Public methods
        public Planet[] GetAliveEnemyPlanets()
        {
            List<Planet> alivePlanets = new List<Planet>();
            foreach (var planet in planets)
            {
                if (planet.GetComponent<PlayerPlanet>()) continue;
                if (planet.activeInHierarchy) alivePlanets.Add(planet.GetComponent<Planet>());
            }
            return alivePlanets.ToArray();
        }

        public Planet[] GetAlivePlanets()
        {
            List<Planet> alivePlanets = new List<Planet>();
            foreach (var planet in planets)
            {
                if (planet.activeInHierarchy) alivePlanets.Add(planet.GetComponent<Planet>());
            }
            return alivePlanets.ToArray();
        }

        public void IncreasePlayerScore(float damage, Transform target)
        {
            var distance = Vector3.Distance(playerPlanet.transform.position, target.position);
            score += (int)(damage * distance);
            UpdatePlayerScore(score);
        }

        public void IncreasePlayerKills()
        {
            kills += 1;
            killsLabel.text = kills.ToString();
            var scoreForKill = 3000000; //TODO: calculate better score for kill
            score += scoreForKill;
            UpdatePlayerScore(score);
        }

        public void RestartMatch()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        public void SaveGame()
        {
            List<SavedPlanet> savedPlanets = new List<SavedPlanet>();
            foreach (var planet in planets)
            {
                savedPlanets.Add(new SavedPlanet
                {
                    name = planet.name,
                    weaponName = planet.GetComponent<ArtileryCommander>().rocketName,
                    pos = new float[] { planet.transform.position.x, planet.transform.position.y, planet.transform.position.z },
                    health = planet.GetComponent<Damageable>().health,
                    colorR = planet.GetComponent<Planet>().color.r,
                    colorG = planet.GetComponent<Planet>().color.g,
                    colorB = planet.GetComponent<Planet>().color.b,
                    size = planet.GetComponent<Planet>().size,
                    density = planet.GetComponent<Planet>().density,
                    mass = planet.GetComponent<Planet>().mass,
                    orbitSpeed = planet.GetComponent<Planet>().orbitSpeed,
                });
            }

            var state = new GameState
            {
                score = this.score,
                kills = this.kills,
                playerName = this.playerName,
                playerWeaponName = this.playerWeaponName,
                numberOfPlanets = this.planetsCount,
                planets = savedPlanets.ToArray()
            };

            SaveManager.i.SaveGame(state);
        }

        #endregion

        private void UpdatePlayerScore(int score)
        {
            this.score = score;
            scoreLabel.text = score.ToString();
            SaveManager.i.SaveBestScore(this.score);
        }

        private void LoadGameState()
        {
            GameState state = SaveManager.i.gameData.state;
            this.score = state.score;
            this.kills = state.kills;
            this.playerName = state.playerName;
            this.playerWeaponName = state.playerWeaponName;
            this.planetsCount = state.numberOfPlanets;

            foreach (var savedPlanet in state.planets)
            {
                Color planetColor = new Color {
                    r = savedPlanet.colorR,
                    g = savedPlanet.colorG,
                    b = savedPlanet.colorB
                };

                GameObject planet = sphereGenerator.CreatePlanet(planetColor, savedPlanet.size);
                planet.name = savedPlanet.name;

                Planet planetComp = planet.AddComponent<Planet>();
                planetComp.Configure(planet.name, planetColor, savedPlanet.size, savedPlanet.density, savedPlanet.orbitSpeed, savedPlanet.mass);
                planet.transform.position = new Vector3(savedPlanet.pos[0], savedPlanet.pos[1], savedPlanet.pos[2]);

                ArtileryCommander commander = planet.GetComponent<ArtileryCommander>();
                commander.SetWeapon(savedPlanet.weaponName);
                uiProgressBar.SubscribeOnArtileryCommander(commander);

                Damageable damageable = planet.GetComponent<Damageable>();
                float maxHealth = savedPlanet.size * savedPlanet.mass;
                damageable.SetHealth(maxHealth, savedPlanet.health);
                if (savedPlanet.health == 0)
                {
                    damageable.isDead = true;
                    planet.SetActive(false);
                }
                uiHealthBar.SubscribeOnDamageable(damageable);

                planets.Add(planet);
            }

            List<UIPointerTarget> pointerTargets = new List<UIPointerTarget>();

            for (int index = 0; index < planets.Count; index++)
            {
                var planet = planets[index];
                if (planet.name == playerName)
                {
                    playerPlanetIndex = index;
                    planet.AddComponent<PlayerPlanet>();
                    cameraController.SetTarget(planet.transform, false);
                    planet.AddComponent<ShieldCommander>();
                    uIPointer.SetPlayer(planet.transform);
                }
                else
                {
                    planet.AddComponent<AIPlanet>();
                    pointerTargets.Add(new UIPointerTarget(planet.transform, GameAssets.i.pointerSprite));
                }
            }
            uIPointer.SetTargets(pointerTargets);

            UpdateHUD();
        }
    }
}
