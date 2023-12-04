using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int maxHealth;
    public int currentHealth;
    private Character _cc;
    private void Awake()
    {
        currentHealth = maxHealth;
        _cc = GetComponent<Character>();
    }

    public float CurrentHealthPercentage()
    {
        return (float)currentHealth/(float)maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        CheckHealth();
    }
    private void CheckHealth()
    {
        if(currentHealth <= 0)
        {
            _cc.SwitchStateTo(CharacterState.Dead);
        }
    }
    public void AddHealth(int health)
    {
        currentHealth += health;

        if(currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }
}
