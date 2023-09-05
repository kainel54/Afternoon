using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class Enemy : Poolable
{
    Rigidbody2D rigid = null;
    Rigidbody2D target = null;
    private Animator animator;
    [SerializeField]
    float speed = 1f;

    [SerializeField]
    private EnemyData enemyData;
    private float health;

    private Vector2 dirVector = Vector2.zero;
    private WaitForFixedUpdate wait;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();        
    }

    private void OnEnable()
    {
        if(target == null)
        {
            return;
        }
        target = GameManager.Instance.CurrentPlayer.GetComponent<Rigidbody2D>();

        health = enemyData.maxHealth;
        speed = enemyData.speed;
    }

    private void FixedUpdate()
    {
        dirVector = target.position - rigid.position;

        float xScale = dirVector.x < 0 ? -1f : 1f;
        transform.localScale = new Vector3(xScale, 1f, 1f);

        Vector2 nextPosition = dirVector.normalized * speed * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + nextPosition);
    }
    
    public void Damage(float damage)
    {
        health -= damage;
        StartCoroutine(Knockback(damage));

        if (health > 0f)
        {
            animator.SetTrigger("Hit");
        }
        else
        {
            Release();
        }
    }

    private IEnumerator Knockback(float knockback)
    {

        yield return wait;
    }

    private void DoKnockback(float distance = 2f)
    {
        Vector3 playerPosition = GameManager.Instance.CurrentPlayer.transform.position;
        Vector3 dirVector = transform.position - playerPosition;
        rigid.AddRelativeForce(dirVector.normalized * distance, ForceMode2D.Impulse);
    }
}
