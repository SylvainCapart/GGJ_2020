using UnityEngine;
using UnityEngine.SceneManagement;

public class GeneralSceneMgt : MonoBehaviour
{
    public enum SceneIndex {MENU, GAME};

    public static GeneralSceneMgt instance;

    private void Awake()
    {
        //Screen.fullScreen = false;

        if (instance != null)
        {
            if (instance != this)
            {
                Destroy(this.gameObject);
            }
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
    }

    public void GoToScene(SceneIndex scene_index)
    {
        SceneManager.LoadScene((int)scene_index);
    }

    public void GoToMenu()
    {
        //if (System.Math.Abs(Time.timeScale - 1f) > EPSILON)
         //   Time.timeScale = 1f;

        SceneManager.LoadScene((int)SceneIndex.MENU);


    }

    public void GoToGameNormal()
    {

  //      if (System.Math.Abs(Time.timeScale - 1f) > EPSILON)
  //          Time.timeScale = 1f;

        SceneManager.LoadScene((int)SceneIndex.GAME);

    }

    public void QuitGame()
    {
        Application.Quit();
    }

}
