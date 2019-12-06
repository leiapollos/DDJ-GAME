using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour
{
    public LayerMask collisionMask;
    float speed = 10;
    float damage = 1;

    public float lifetime = 3;
    float skinWidth = .1f;

    void Start()
    {
        Destroy(gameObject, lifetime);

        Collider[] initialCollisions = Physics.OverlapSphere(transform.position, .1f, collisionMask);
        if (initialCollisions.Length > 0)
        {
            OnHitObject(initialCollisions[0]);
        }
    }

    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }

    void Update()
    {
        float moveDistance = speed * Time.deltaTime;
        CheckCollisions(moveDistance);
        Vector3 translation = Vector3.forward * moveDistance;
        transform.Translate(translation);
    }


    void CheckCollisions(float moveDistance)
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, moveDistance + skinWidth, collisionMask, QueryTriggerInteraction.Collide))
        {
            OnHitObject(hit);
        }
    }

    void OnHitObject(RaycastHit hit)
    {
        IDamageable damageableObject = hit.collider.GetComponent<IDamageable>();
        if (damageableObject != null)
        {
            damageableObject.TakeHit(damage, hit);
            if (hit.collider.tag == "Enemy")
            {
                hit.collider.GetComponent<Enemy2>().TriggerStagger();
            }
        }
        GameObject.Destroy(gameObject);
    }

    void OnHitObject(Collider c)
    {
        IDamageable damageableObject = c.GetComponent<IDamageable>();
        if (damageableObject != null)
        {
            damageableObject.TakeDamage(damage);
        }
        GameObject.Destroy(gameObject);
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.name);
        if(collision.gameObject.tag == "Subway")
        {
            Physics.IgnoreCollision(collision.gameObject.GetComponent<Collider>(), GetComponent<Collider>());
        }
    }
}