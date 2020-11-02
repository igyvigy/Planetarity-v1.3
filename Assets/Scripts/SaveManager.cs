using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;

namespace Planetarity
{
    [Serializable]
    public class SaveManager : MonoBehaviour
    {
        public static SaveManager i;
        private static string FILE_PATH;

        public RectTransform loadButton;
        public TextMeshProUGUI bestScoreLabelMenu;
        public TextMeshProUGUI bestScoreLabelGame;
        public TextMeshProUGUI bestScoreLabelWinGame;
        public GameData gameData = new GameData();

        private bool hasSavedGame {
            get {
                bool res = false;
                if (gameData != null)
                {
                    res = gameData.didSave;
                }
                return res;
            }
        }

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
            FILE_PATH = Path.Combine(Application.persistentDataPath, "data.igy");
        }

        void Start()
        {
            var gameData = LoadGameData();
            if (gameData != null)
            {
                this.gameData = gameData;
            }
            loadButton.gameObject.SetActive(hasSavedGame);
            UpdateBestScoreLabels();
        }

        public void LoadGame()
        {
            Settings.i.shouldLoadGameState = true;
            SceneManager.LoadSceneAsync("GameScene");
        }

        public void SaveGame(GameState state)
        {
            gameData.didSave = true;
            gameData.state = state;
            SaveGamedata(gameData);
        }

        public void SaveBestScore(int score)
        {
            if (score > gameData.bestScore)
            {
                gameData.bestScore = score;
                UpdateBestScoreLabels();
                SaveGamedata(gameData);
            }
        }

        private void UpdateBestScoreLabels()
        {
            if (bestScoreLabelMenu != null) bestScoreLabelMenu.text = gameData.bestScore.ToString();
            if (bestScoreLabelGame != null) bestScoreLabelGame.text = gameData.bestScore.ToString();
            if (bestScoreLabelWinGame != null) bestScoreLabelWinGame.text = gameData.bestScore.ToString();
        }

        private void SaveGamedata(GameData gameData)
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream stream = new FileStream(FILE_PATH, FileMode.OpenOrCreate);
            bf.Serialize(stream, gameData);
            stream.Close();
        }

        private GameData LoadGameData()
        {
            if (File.Exists(FILE_PATH))
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream stream = new FileStream(FILE_PATH, FileMode.Open);
                GameData data = bf.Deserialize(stream) as GameData;
                gameData = data;
                stream.Close();
                return data;
            }
            else
            {
                return null;
            }
        }
    }


    [Serializable]
    public struct SavedPlanet
    {
        [SerializeField] public string name;
        [SerializeField] public string weaponName;
        [SerializeField] public float[] pos;
        [SerializeField] public float health;
        [SerializeField] public float colorR;
        [SerializeField] public float colorG;
        [SerializeField] public float colorB;
        [SerializeField] public float size;
        [SerializeField] public float density;
        [SerializeField] public float mass;
        [SerializeField] public float orbitSpeed;
    }


    [Serializable]
    public struct GameState
    {
        [SerializeField] public int score;
        [SerializeField] public int kills;
        [SerializeField] public string playerName;
        [SerializeField] public string playerWeaponName;
        [SerializeField] public int numberOfPlanets;
        [SerializeField] public SavedPlanet[] planets;
    }

    [Serializable]
    public class GameData
    {
        [SerializeField] public int bestScore = 0;
        [SerializeField] public bool didSave = false;
        [SerializeField] public GameState state;
    }

}