using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Menu
{
    public class MenuManager : MonoBehaviour
    {
        [SerializeField] private GameObject startPanel;
        [SerializeField] private GameObject optionsPanel;

        private void Start()
        {
            startPanel.SetActive(true);
            optionsPanel.SetActive(false);
        }

        public void StartButton()
        {
            SceneManager.LoadScene("GameScene");
        }

        public void OptionsButton()
        {
            startPanel.SetActive(false);
            optionsPanel.SetActive(true);
        }

        public void BackButton()
        {
            startPanel.SetActive(true);
            optionsPanel.SetActive(false);
        }
        
        public void QuitButton()
        {
            Application.Quit();
        }
    }
}