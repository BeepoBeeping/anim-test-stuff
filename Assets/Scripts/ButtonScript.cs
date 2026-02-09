using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonScript : MonoBehaviour

{


    public void PlayButton()
    {
        SceneManager.LoadScene("Main");
    }

    public void QuitButton()
    {
        Application.Quit();
    }

}
