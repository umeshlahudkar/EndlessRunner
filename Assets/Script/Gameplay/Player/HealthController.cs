
using System.Collections;
using UnityEngine;
using RG.UI;

namespace RG.Gameplay
{
    public class HealthController : MonoBehaviour
    {
        [SerializeField] private int maxHealth = 3;
        private int currentHealth = 0;

        [SerializeField] private SkinnedMeshRenderer playerRenderer;
        [SerializeField] private Color flashColor = Color.red;
        [SerializeField] private float flashDuration = 0.2f;
        [SerializeField] private int numberOfFlashes = 5;

        private Color[] originalColors;


        private void Start()
        {
            ResetHealth();
            originalColors = new Color[playerRenderer.materials.Length];
            for (int i = 0; i < playerRenderer.materials.Length; i++)
            {
                originalColors[i] = playerRenderer.materials[i].color;
            }
        }

        public void TakeDamage()
        {
            currentHealth--;
            if (currentHealth <= 0)
            {
                currentHealth = 0;
                GameManager.Instance.SetGameOver();
            }

            StartCoroutine(FlashPlayer());
            UIController.Instance.UpdateHealthDisplay(currentHealth);
        }

        private IEnumerator FlashPlayer()
        {
            for (int i = 0; i < numberOfFlashes; i++)
            {
                SetMaterialColor(flashColor);
                yield return new WaitForSeconds(flashDuration / 2);

                SetMaterialColor(originalColors);
                yield return new WaitForSeconds(flashDuration / 2);
            }
        }

        private void SetMaterialColor(Color color)
        {
            for (int i = 0; i < playerRenderer.materials.Length; i++)
            {
                playerRenderer.materials[i].color = color;
            }
        }

        private void SetMaterialColor(Color[] colors)
        {
            for (int i = 0; i < playerRenderer.materials.Length; i++)
            {
                playerRenderer.materials[i].color = colors[i];
            }
        }

        public void ResetHealth()
        {
            currentHealth = maxHealth;
            UIController.Instance.UpdateHealthDisplay(currentHealth);
        }
    }

}