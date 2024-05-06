using UnityEngine;
using TMPro;
using RG.Gameplay;

namespace RG.UI
{
    public class TimeDisplay : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI timeText;

        private void OnEnable()
        {
            ScoreManager.OnLevelTimeUpdate += UpdateDisplay;
            UpdateDisplay(ScoreManager.Instance.TimeSinceLevelStart);
        }

        private void UpdateDisplay(float time)
        {
            int minutes = Mathf.FloorToInt(time / 60);
            int seconds = Mathf.FloorToInt(time % 60);

            timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }

        private void OnDisable()
        {
            ScoreManager.OnLevelTimeUpdate -= UpdateDisplay;
        }
    }
}
