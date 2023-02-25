using UnityEngine;
using UnityEngine.SceneManagement;
public class LevelSelect : MonoBehaviour
{
    [SerializeField] private string mainMenuScene;
    [SerializeField] private string tutorialScene;
    [SerializeField] private string noTutorialScene;

    public void LoadTutorial()
    {
        SceneManager.LoadScene(tutorialScene, LoadSceneMode.Single);
    }

    public void LoadNormal()
    {
        SceneManager.LoadScene(noTutorialScene, LoadSceneMode.Single);
    }

    public void BackButton()
    {
        SceneManager.UnloadSceneAsync("LevelSelect");
    }

}
