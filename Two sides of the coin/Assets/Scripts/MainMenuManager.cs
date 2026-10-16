using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
   
    public string gameplaySceneName = "Test";


    
    public GameObject howToPlayModal;


    void Start()
    {
        if (howToPlayModal != null)
            howToPlayModal.SetActive(false);
    }

    
    public void PlayGame()
    {
        SceneManager.LoadScene(gameplaySceneName);
    }

    
    public void OpenHowToPlay()
    {
        if (howToPlayModal != null)
            howToPlayModal.SetActive(true);
    }

    
    public void CloseHowToPlay()
    {
        if (howToPlayModal != null)
            howToPlayModal.SetActive(false);
    }

    
    public void QuitGame()
    {
        Application.Quit();
        
        UnityEditor.EditorApplication.isPlaying = false;
       
    }
}