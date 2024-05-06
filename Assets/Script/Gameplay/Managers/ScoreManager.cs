using UnityEngine;
using RG.Utils;

namespace RG.Gameplay
{
    public class ScoreManager : Singleton<ScoreManager>
    {
        private float totalDistanceTraveled;
        private float timeSinceLevelStart;

        public float TotalDistanceTraveled
        {
            get { return totalDistanceTraveled; }
        }

        public float TimeSinceLevelStart
        {
            get { return timeSinceLevelStart; }
        }

        public delegate void LevelTimeUpdate(float time);
        public static event LevelTimeUpdate OnLevelTimeUpdate;

        public delegate void TravelDistanceUpdate(float coin);
        public static event TravelDistanceUpdate OnTravelDistanceUpdate;

        private void Start()
        {
            ResetScore();
        }

        private void LateUpdate()
        {
            if (GameManager.Instance.State == GameState.Playing)
            {
                timeSinceLevelStart += Time.deltaTime;
                UpdateLevelTime();

                totalDistanceTraveled = GameManager.Instance.levelGenerator.TotalDistanceTraveled;
                UpdateDistanceTravel();
            }
        }

        private void UpdateDistanceTravel()
        {
            OnTravelDistanceUpdate?.Invoke(totalDistanceTraveled);
        }

        private void UpdateLevelTime()
        {
            OnLevelTimeUpdate?.Invoke(timeSinceLevelStart);
        }

        public void ResetScore()
        {
            totalDistanceTraveled = 0;
            timeSinceLevelStart = 0;
            OnLevelTimeUpdate?.Invoke(timeSinceLevelStart);
            OnTravelDistanceUpdate?.Invoke(totalDistanceTraveled);
        }
    }

}