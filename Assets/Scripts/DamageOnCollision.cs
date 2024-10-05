using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOnCollision : MonoBehaviour
{
    public int damageAmount = 10;
    public string targetTag = "Target";
    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("OnCollisionEnter2D");
        if (collision.gameObject.CompareTag(targetTag))
        {
            Health targetHealth = collision.gameObject.GetComponent<Health>();
            
            if (targetHealth != null)
            {
                targetHealth.TakeDamage(damageAmount);
            }
            Destroy(gameObject);
        }
    }
}
