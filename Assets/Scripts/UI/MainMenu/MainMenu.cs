using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    public void SingleSceneLoad(string singleSceneLoadName)
    {
        SceneManager.LoadScene(singleSceneLoadName, LoadSceneMode.Single);
    }

    public void AddititiveSceneLoad(string additiveSceneLoadName)
    {
        SceneManager.LoadSceneAsync(additiveSceneLoadName, LoadSceneMode.Additive);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
