
using RG.Utils;

namespace RG.Gameplay
{
    public class CoinManager : Singleton<CoinManager>
    {
        private int currentCoins = 0;

        public int CurrentCoins
        {
            get { return currentCoins; }
        }

        public delegate void CoinUpdate(int coin);
        public static event CoinUpdate OnCoinUpdate;

        private void Start()
        {
            ResetCoins();
        }

        public void AddCoins(int amount = 1)
        {
            currentCoins += amount;
            OnCoinUpdate?.Invoke(currentCoins);
        }

        public void DeductCoins(int amount)
        {
            if (currentCoins >= amount)
            {
                currentCoins -= amount;
                OnCoinUpdate?.Invoke(currentCoins);
            }
        }

        public void ResetCoins()
        {
            currentCoins = 0;
            OnCoinUpdate?.Invoke(currentCoins);
        }
    }
}
