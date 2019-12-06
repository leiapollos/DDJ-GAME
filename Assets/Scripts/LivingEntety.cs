using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LivingEntity : MonoBehaviour, IDamageable
{

    public float startingHealth;
    protected float health;
    public bool dead;

    public event System.Action OnDeath;

    public Image healthbar;

    public GameObject gameController;

    protected virtual void Start()
    {
        health = startingHealth;
        gameController = GameObject.FindGameObjectWithTag("GameController");
    }

    public void TakeHit(float damage, RaycastHit hit)
    {
        // Do some stuff here with hit var
        TakeDamage(damage);
    }

    public void TakeDamage(float damage)
    {
        health -= damage;


        healthbar.fillAmount = health / startingHealth;

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
        if (this.gameObject.tag == "Enemy")
        {
            gameController.GetComponent<GameController>().UpdateScore(1);
        }
        else if (this.gameObject.tag == "Civilian")
        {
            gameController.GetComponent<GameController>().UpdateScore(-5);
        }

        var animator = gameObject.GetComponent<Animator>();
        animator.SetTrigger("Death");

        //GameObject.Destroy(gameObject);

    }
}