using UnityEngine;
using TMPro;
using RG.Gameplay;

namespace RG.UI
{
    public class CoinDisplay : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI coinText;

        private void OnEnable()
        {
            CoinManager.OnCoinUpdate += UpdateDisplay;
            UpdateDisplay(CoinManager.Instance.CurrentCoins);
        }

        private void UpdateDisplay(int currentCoin)
        {
            coinText.text = currentCoin.ToString();
        }

        private void OnDisable()
        {
            CoinManager.OnCoinUpdate -= UpdateDisplay;
        }
    }
}
