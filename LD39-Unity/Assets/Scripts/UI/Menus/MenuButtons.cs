using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour {

    public void StartGame()
    {
        SceneManager.LoadScene("Main", LoadSceneMode.Single);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
