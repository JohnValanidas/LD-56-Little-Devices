using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Health : MonoBehaviour
{
    public int maxHealth = 100;
    private int _currentHealth = 100;

    public event Action<int, int> OnHealthChanged;
    public event Action onUnitDestoryed;
    
    void Start()
    {
        _currentHealth = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        _currentHealth -= amount;
        OnHealthChanged?.Invoke(_currentHealth, maxHealth);
        
        if (_currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        onUnitDestoryed?.Invoke();
        Destroy(gameObject);
    }
}