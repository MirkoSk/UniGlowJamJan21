using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniGlow.Utility;
using UnityEngine.UI;
using DG.Tweening;

public class HealthBar : MonoBehaviour
{
    [SerializeField] List<Image> healthImages = new List<Image>();
    [SerializeField] float flashDuration = 0.3f;
    [SerializeField] int numberOfFlashes = 3;
    [SerializeField] Color disappearColor = new Color();

    int currentHealth;
    bool endInitiated;



    private void Start()
    {
        currentHealth = healthImages.Count;
    }

    private void OnEnable()
    {
        GameEvents.PlayerDamaged += UpdateHealthBar;
    }

    private void OnDisable()
    {
        GameEvents.PlayerDamaged -= UpdateHealthBar;
    }

    private void Update()
    {
        if (currentHealth <= 0 && !endInitiated)
        {
            GameEvents.EndGame();
            endInitiated = true;
        }
    }



    void UpdateHealthBar(Vector2 enemyPosition)
    {
        if (currentHealth <= 0)
        {
            Debug.LogError("Trying to set health below 0.");
            return;
        }

        healthImages[currentHealth - 1].DOColor(disappearColor, flashDuration).SetLoops(numberOfFlashes * 2 - 1, LoopType.Yoyo);
        currentHealth--;
    }
}
