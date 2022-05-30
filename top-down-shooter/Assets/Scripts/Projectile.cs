using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public LayerMask collisionMask;
    float speed = 10;
    float damage = 1;

    float lifetime = 2;
    // this is to compensate for situations where the enemy and projectile are moving towards eachother
    // in such cases the projectile could end up inside the enemy inbetween frames and thus not register as a hit again
    // this will extend the length of the projectile slightly and make this much much less likley to happen. 
    float skinWidth = .1f;  

    private void Start()
    {
        Destroy(gameObject, lifetime);

        // this part is to check if the bullet that is shot start inside the enemy incase they are super close
        // since the raycast will not detect a collision if this is the case. 
        Collider[] initialCollisions = Physics.OverlapSphere(transform.position, .1f, collisionMask);
        if (initialCollisions.Length > 0)
        {
            onHitObject(initialCollisions[0]);
        }
    }

    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }
    // Update is called once per frame
    void Update()
    {
        float moveDistance = speed * Time.deltaTime;
        CheckCollisions(moveDistance);
        transform.Translate(Vector3.forward * moveDistance);
    }
    /**
     * Using raycast to check collisions so that it does not break on high velocity bullets
     * since using other methods we could end up in situations where the bullet has gone through the
     * enemy between frames and thus we wont register the hit. 
     */
    private void CheckCollisions(float moveDistance)
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, moveDistance + skinWidth, collisionMask, QueryTriggerInteraction.Collide))
        {
            OnHitObject(hit);
        }
    }

    private void OnHitObject(RaycastHit hit)
    {
        IDamageable damageableObject = hit.collider.GetComponent<IDamageable>();
        if(damageableObject != null)
        {
            damageableObject.TakeHit(damage, hit);
        }
        //print(hit.collider.gameObject.name);
        GameObject.Destroy(gameObject);
    }

    private void onHitObject(Collider c)
    {
        IDamageable damageableObject = c.GetComponent<IDamageable>();
        if (damageableObject != null)
        {
            damageableObject.TakeDamage(damage);
        }
        //print(hit.collider.gameObject.name);
        GameObject.Destroy(gameObject);
    }
}
