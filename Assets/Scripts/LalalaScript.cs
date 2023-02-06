using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LalalaScript : MonoBehaviour
{
    [SerializeField] private GameObject vicScreen;
    [SerializeField] private GameObject credits;
    
    public void RestartButton()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void MenuButton()
    {
        SceneManager.LoadScene("Menu");
    }

    public void OpenCredits()
    {
        credits.SetActive(true);
        vicScreen.SetActive(false);
        StartCoroutine(StartTimer(15f));
    }

    private IEnumerator StartTimer(float time)
    {
        yield return new WaitForSeconds(time);
        MenuButton();
    }
}
