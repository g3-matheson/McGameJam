using UnityEngine;
using UnityEngine.UI;

public class EndScreen : MonoBehaviour
{
    public Button restartButton;

    public Button mainMenuButton;

    void Awake()
    {
        restartButton = gameObject.transform.Find("RestartButton").GetComponent<Button>();
        mainMenuButton = gameObject.transform.Find("MainMenuButton").GetComponent<Button>();
        restartButton.onClick.AddListener(OnRestartButtonClicked);
        mainMenuButton.onClick.AddListener(OnMainMenuButtonClicked);
    }

    void OnRestartButtonClicked()
    {
        GameManager.Instance.bWin = false;
        GameManager.Instance.ReloadScene();
    }

    void OnMainMenuButtonClicked()
    {
        GameManager.Instance.bWin = false;
        UnityEngine.SceneManagement.SceneManager.LoadScene("Main Menu");
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
