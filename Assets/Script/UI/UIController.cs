using UnityEngine;
using UnityEngine.UI;
using RG.Utils;
using RG.Gameplay;

namespace RG.UI
{
    public class UIController : Singleton<UIController>
    {
        [Header("Main Menu")]
        [SerializeField] private GameObject mainMenuScreen;

        [Header("Gameplay Screens")]
        [SerializeField] private GameObject gameplayCanvas;
        [SerializeField] private GameObject tapToRunScreen;
        [SerializeField] private GameObject pauseScreen;
        [SerializeField] private GameObject gameOverScreen;

        [Header("Gameplay UI")]
        [SerializeField] private CoinDisplay coinDisplay;
        [SerializeField] private TimeDisplay timeDisplay;
        [SerializeField] private DistanceDisplay distanceDisplay;
        [SerializeField] private HealthDisplay healthDisplay;
        [SerializeField] private Button pauseButton;

        private void Start()
        {
            mainMenuScreen.SetActive(true);

            ToggleGameplayUI(false);
            tapToRunScreen.SetActive(false);
            pauseScreen.SetActive(false);
            gameOverScreen.SetActive(false);
            gameplayCanvas.SetActive(false);
        }

        public void OnPlayButtonClick()
        {
            mainMenuScreen.SetActive(false);

            gameplayCanvas.SetActive(true);
            tapToRunScreen.SetActive(true);

            GameManager.Instance.PrepareLevel();
        }

        public void OnQuitButtonClick()
        {
            Application.Quit();
        }

        public void OnTapToStartClick()
        {
            ToggleGameplayUI(true);

            tapToRunScreen.SetActive(false);
            GameManager.Instance.StartGame();
        }

        public void OnPauseButtonClick()
        {
            pauseScreen.SetActive(true);
            GameManager.Instance.State = GameState.Paused;
            Time.timeScale = 0;
        }

        public void OnResumeButtonClick()
        {
            pauseScreen.SetActive(false);
            GameManager.Instance.State = GameState.Playing;
            Time.timeScale = 1;
        }

        public void OnHomeButtonClick()
        {
            Time.timeScale = 1;
            GameManager.Instance.ResetGame();

            mainMenuScreen.SetActive(true);

            ToggleGameplayUI(false);
            tapToRunScreen.SetActive(false);
            pauseScreen.SetActive(false);
            gameOverScreen.SetActive(false);
            gameplayCanvas.SetActive(false);
        }

        public void OnRestartButtonClick()
        {
            Time.timeScale = 1;
            GameManager.Instance.ResetGame();

            pauseScreen.SetActive(false);
            gameOverScreen.SetActive(false);
            ToggleGameplayUI(false);

            OnTapToStartClick();
        }

        public void UpdateHealthDisplay(int remainingHealth)
        {
            healthDisplay.UpdateDisplay(remainingHealth);
        }

        public void OpenGameOverScreen()
        {
            gameOverScreen.SetActive(true);
            ToggleGameplayUI(false);
        }

        private void ToggleGameplayUI(bool status)
        {
            pauseButton.gameObject.SetActive(status);
            healthDisplay.gameObject.SetActive(status);
            coinDisplay.gameObject.SetActive(status);
            timeDisplay.gameObject.SetActive(status);
            distanceDisplay.gameObject.SetActive(status);
        }
    }
}