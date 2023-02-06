using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    [SerializeField] private GameObject pauseObj;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!pauseObj.activeSelf)
            {
                pauseObj.SetActive(true);
                Time.timeScale = 0f;
            }
            else
            {
                pauseObj.SetActive(false);
                Time.timeScale = 1f;
            }
        }
    }

    public void ResumeButton()
    {
        pauseObj.SetActive(false);
        Time.timeScale = 1f;
    }

    public void MenuButton()
    {
        SceneManager.LoadScene("Menu");
        Time.timeScale = 1f;
    }
}
