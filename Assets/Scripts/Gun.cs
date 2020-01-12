using UnityEngine;
using System.Collections;

public class Gun : MonoBehaviour
{

    public Transform muzzle;
    public Projectile projectile;
    public float msBetweenShots;
    public float muzzleVelocity = 35;

    protected float nextShotTime;
    private AudioManager audioManager;
    public int number_of_bullets = 10;

    public void Start()
    {
        this.audioManager = GameObject.FindObjectOfType<AudioManager>();
    }

    public void Shoot()
    {
        if (Time.time > nextShotTime)
        {
            nextShotTime = Time.time + msBetweenShots / 1000;
            Projectile newProjectile = Instantiate(projectile, muzzle.position, muzzle.rotation) as Projectile;
            newProjectile.SetSpeed(muzzleVelocity);
        }
    }

    IEnumerator waitForShot()
    {
        yield return new WaitForSeconds(1f);
    }
}