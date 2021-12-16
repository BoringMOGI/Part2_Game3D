using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] Animator anim;
    [SerializeField] Damageable damageable;
    [SerializeField] new Collider collider;


    private void Update()
    {
        anim.SetBool("isAlive", damageable.hp > 0.0f);
    }

    public void OnDamaged()
    {
        anim.SetTrigger("onHit");
    }
    public void OnDead()
    {
        //collider.isTrigger = true;
        collider.enabled = false;

        Invoke("DestroyEnemy", 3.0f);
    }

    void DestroyEnemy()
    {
        Destroy(gameObject);
    }
}
