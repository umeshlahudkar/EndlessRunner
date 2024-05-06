using UnityEngine;
using UnityEngine.UI;

namespace RG.UI
{
    public class HealthDisplay : MonoBehaviour
    {
        [SerializeField] private Image[] healthImages;

        public void UpdateDisplay(int remainingHealth)
        {
            for (int i = 1; i <= healthImages.Length; i++)
            {
                if (i <= remainingHealth)
                {
                    healthImages[i - 1].gameObject.SetActive(true);
                }
                else
                {
                    healthImages[i - 1].gameObject.SetActive(false);
                }
            }
        }
    }
}
