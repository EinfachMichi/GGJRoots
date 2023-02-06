using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Menu
{
    public class MenuManager : MonoBehaviour
    {
        [SerializeField] private GameObject startPanel;
        [SerializeField] private Animator camAnimator;
        [SerializeField] private Texture2D cursor;

        private void Start()
        {
            startPanel.SetActive(true);
            Cursor.SetCursor(cursor, Vector2.zero, CursorMode.Auto);
        }

        public void StartButton()
        {
            startPanel.SetActive(false);
            camAnimator.SetTrigger("Transition");
        }

        private void LoadGameScene()
        {
            SceneManager.LoadScene("GameScene");
        }

        public void QuitButton()
        {
            Application.Quit();
        }
    }
}