using UnityEngine;
using UnityEditor;

public class LisboaVivaGun : Gun
{


    public new void Shoot()
    {
        if (Time.time > nextShotTime)
        {
            nextShotTime = Time.time + msBetweenShots / 1000;
            //I made the weapon point in the wrong direction so we have to rotate the bullets:
            Vector3 rot = muzzle.rotation.eulerAngles;
            rot = new Vector3(rot.x, rot.y, rot.z - 90);
            Projectile newProjectile = Instantiate(projectile, muzzle.position, Quaternion.Euler(rot)) as Projectile;
            newProjectile.SetSpeed(muzzleVelocity);
        }
    }
}
