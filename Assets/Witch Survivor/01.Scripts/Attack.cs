using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Playables;

public class Attack : Poolable
{
    [SerializeField]
    private float atk = 3;
    [SerializeField]
    private float atkDelay = 0.5f;

    private void OnEnable()
    {
        Invoke("Release",atkDelay);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            enemy.Damage(atk);
        }
    }


}
