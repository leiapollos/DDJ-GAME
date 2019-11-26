using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LivingEntity : MonoBehaviour, IDamageable
{

    public float startingHealth;
    protected float health;
    protected bool dead;

    public event System.Action OnDeath;

    public Image healthbar;


    protected virtual void Start()
    {
        health = startingHealth;
    }

    public void TakeHit(float damage, RaycastHit hit)
    {
        // Do some stuff here with hit var
        TakeDamage(damage);
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        
        if(this.gameObject.tag != "Person" )
        {
            Debug.Log(this.gameObject.tag);
            healthbar.fillAmount = health / startingHealth;

        }

        if (health <= 0 && !dead)
        {
            Die();
        }
    }

    protected void Die()
    {
        dead = true;
        if (OnDeath != null)
        {
            OnDeath();
        }
        GameObject.Destroy(gameObject);
    }
}