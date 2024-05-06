using RG.UI;
using RG.Utils;
using UnityEngine;

namespace RG.Gameplay
{
    public class GameManager : Singleton<GameManager>
    {
        private GameState gameState = GameState.Waiting;
        public LevelGenerator levelGenerator;
        [SerializeField] private PlayerController playerController;

        public GameState State
        {
            get { return gameState; }
            set { gameState = value; }
        }

        public void PrepareLevel()
        {
            levelGenerator.GenerateLevel();
            playerController.gameObject.SetActive(true);
        }

        public void StartGame()
        {
            gameState = GameState.Playing;
            playerController.Run();
        }

        public void SetGameOver()
        {
            gameState = GameState.GameOver;
            UIController.Instance.OpenGameOverScreen();
            playerController.Stop();
        }

        public void ResetLevelSpeed()
        {
            levelGenerator.ResetSpeed();
        }

        public void ResetGame()
        {
            gameState = GameState.Waiting;
            playerController.ResetPlayer();
            levelGenerator.ResetLevel();

            ScoreManager.Instance.ResetScore();
            CoinManager.Instance.ResetCoins();
        }
    }
}