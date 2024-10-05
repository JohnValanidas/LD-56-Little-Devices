using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBasedSpriteChanger : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;  // Reference to the SpriteRenderer
    public Color fullHealthColor = Color.white;  // Color at full health
    public Color lowHealthColor = Color.red;     // Color at low health

    private Health healthComponent; // Reference to the Health component

    void Start()
    {
        // Get the SpriteRenderer component attached to this object
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Find the Health component attached to the same object
        healthComponent = GetComponent<Health>();

        // Subscribe to the health change event
        if (healthComponent != null)
        {
            healthComponent.OnHealthChanged += UpdateSpriteColor;
        }
    }

    // Unsubscribe when the object is destroyed to avoid memory leaks
    void OnDestroy()
    {
        if (healthComponent != null)
        {
            healthComponent.OnHealthChanged -= UpdateSpriteColor;
        }
    }

    // Update the color of the sprite based on health change
    void UpdateSpriteColor(int currentHealth, int maxHealth)
    {
        // Calculate the health percentage (0 to 1)
        float healthPercentage = (float)currentHealth / maxHealth;

        // Interpolate between the full health and low health colors
        spriteRenderer.color = Color.Lerp(lowHealthColor, fullHealthColor, healthPercentage);
    }    
}
