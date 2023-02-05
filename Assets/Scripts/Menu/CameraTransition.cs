using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraTransition : MonoBehaviour
{
    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void LoadGameScene()
    {
        SceneManager.LoadScene("GameScene");
    }
}
