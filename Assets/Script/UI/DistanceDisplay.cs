using UnityEngine;
using TMPro;
using RG.Gameplay;

namespace RG.UI
{
    public class DistanceDisplay : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI distanceText;

        private void OnEnable()
        {
            ScoreManager.OnTravelDistanceUpdate += UpdateDisplay;
            UpdateDisplay(ScoreManager.Instance.TotalDistanceTraveled);
        }

        private void UpdateDisplay(float distance)
        {
            int kilometers = Mathf.FloorToInt(distance / 1000f);
            int remainingMeters = Mathf.FloorToInt(distance % 1000);

            remainingMeters /= 10;

            distanceText.text = string.Format("{0}.{1:D2} km", kilometers, remainingMeters);
        }

        private void OnDisable()
        {
            ScoreManager.OnTravelDistanceUpdate -= UpdateDisplay;
        }
    }
}
