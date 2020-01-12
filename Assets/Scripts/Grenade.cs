using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : Projectile
{
    public GameObject explosionPrefab;
    protected GameObject MakedObject;
    void Explode()
    {
        MakedObject = Instantiate(explosionPrefab, this.transform.position, Quaternion.identity) as GameObject;
    }

    public override void OnHitObject(RaycastHit hit)
    {
        Debug.Log(hit.collider.gameObject.name);
        IDamageable damageableObject = hit.collider.GetComponent<IDamageable>();
        if (damageableObject != null && hit.collider.tag != "Player")
        {
            damageableObject.TakeHit(10, hit);

            if (hit.collider.tag == "Enemy")
            {
                var enemy = hit.collider.GetComponent<Enemy2>();
                enemy.TriggerStagger();
                if (this.name.Contains("LisboaVivaCard")) enemy.audioSource.Play();
                Explode();
            }

        }
        GameObject.Destroy(gameObject);
    }
}
