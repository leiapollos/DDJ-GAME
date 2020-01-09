using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LivingEntity : MonoBehaviour, IDamageable
{
    [HideInInspector]
    public AudioManager audioManager;

    public float startingHealth;
    protected float health;
    public bool dead;

    public event System.Action OnDeath;

    public Image healthbar;
    private Image healthbarBackground;

    public GameObject gameController;

    protected virtual void Start()
    {
        health = startingHealth;
        gameController = GameObject.FindGameObjectWithTag("GameController");
        this.audioManager = GameObject.FindObjectOfType<AudioManager>();
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

    public void AddHealth(int ammount)
    {
       // Debug.Log(health);
        health =  health + ammount > startingHealth? startingHealth : health + ammount;
        healthbar.fillAmount = health / startingHealth;
        //Debug.Log(health);
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
            audioManager.Play("ZombieDeath");
        }
        else if (this.gameObject.tag == "Civilian")
        {
            gameController.GetComponent<GameController>().UpdateScore(-10);
        }

        var animator = gameObject.GetComponent<Animator>();
        animator.SetTrigger("Death");

        //GameObject.Destroy(gameObject);

    }
}