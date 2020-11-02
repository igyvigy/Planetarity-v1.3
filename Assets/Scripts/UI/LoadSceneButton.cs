using UnityEngine;
using UnityEngine.SceneManagement;

namespace Planetarity.UI
{
public class LoadSceneButton : MonoBehaviour
    {
        public string SceneName;

        public void LoadTargetScene()
        {
            SceneManager.LoadSceneAsync(SceneName);
        }
    }
}
