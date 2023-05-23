using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace NX
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField]
        Sprite health_sprite;
        [SerializeField]
        Sprite noHealth_sprite;

        [SerializeField]
        Transform horizontalLayout;

        int maxHealth;

        Image[] healthImages;

        public void SetMaxHealth(int _maxHealth)
        {
            maxHealth = _maxHealth;

            healthImages = new Image[maxHealth];

            for (int i = 0; i < maxHealth; i++)
            {
                GameObject newImage = new GameObject("HealthImage_" + i);
                newImage.transform.SetParent(horizontalLayout);

                healthImages[i] = newImage.AddComponent<Image>();

                newImage.GetComponent<Image>().color = new Color(0, 0, 0, 0);
            }

            SetCurrentHealth(maxHealth);
        }

        public void SetCurrentHealth(int currentHealth)
        {
            for (int i = 0; i < healthImages.Length; i++)
            {
                if (i + 1 <= currentHealth)
                    healthImages[i].sprite = health_sprite;
                else 
                    healthImages[i].sprite = noHealth_sprite;
            }
        }
    }
}
