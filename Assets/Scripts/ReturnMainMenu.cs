using UnityEngine;
using UnityEngine.SceneManagement;
public class ReturnMainMenu : MonoBehaviour
{
    public void Return()
    {
        SceneManager.LoadScene("Main Menu");
    }
}
