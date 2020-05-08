using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyUIController : MonoBehaviour{
    [SerializeField] private Image healthBarFillImage = null;
    [SerializeField] private Text healthText = null;
    
    private EnemyController _enemyController;
    // Start is called before the first frame update
    void Start() {
        _enemyController = GetComponent<EnemyController>();
        if (_enemyController == null) {
            Debug.LogError("This GameObject also needs EnemyController class! Destroying myself.");
            Destroy(gameObject);
        }

        _enemyController.OnHealthChanged += OnHealthStatusChanged;
        healthText.text = $"{_enemyController.CurrentHealth}/{_enemyController.MaxHealth}";
    }

    private void OnHealthStatusChanged() {
        // ReSharper disable once PossibleLossOfFraction
        float fillAmount = (float)_enemyController.CurrentHealth / _enemyController.MaxHealth;
        Debug.Log(fillAmount);
        healthBarFillImage.fillAmount = fillAmount;
        healthText.text = $"{_enemyController.CurrentHealth}/{_enemyController.MaxHealth}";
    }
}
