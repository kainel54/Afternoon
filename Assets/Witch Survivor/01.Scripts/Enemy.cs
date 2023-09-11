using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class Enemy : Poolable
{
    private float speed = 1f;
    private Rigidbody2D target = null;
    private Rigidbody2D rigid = null;
    private Animator animator = null;

    [SerializeField]
    private EnemyData enemyData;
    private float health = 0f;

    private Vector2 dirVector = Vector2.zero;

    private WaitForFixedUpdate wait;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        target = GameManager.Instance.CurrentPlayer.
            GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        if (GameManager.Instance.CurrentPlayer == null)
        {
            return;
        }
        target = GameManager.Instance.CurrentPlayer.
            GetComponent<Rigidbody2D>();
        health = enemyData.maxHealth;
        speed = enemyData.speed;
    }

    private void FixedUpdate()
    {
        if (target == null)
        {
            return;
        }
        dirVector = target.position - rigid.position;

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Hit"))
        {
            return;
        }

        float xScale = dirVector.x < 0 ? -1f : 1f;
        transform.localScale = new Vector3(xScale, 1f, 1f);

        Vector2 nextPosition = dirVector.normalized * speed * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + nextPosition);
    }

    public void Damage(Attack atk)
    {
        health -= atk.weaponData.atkDamage;

        StartCoroutine(KnockBack(atk.weaponData.knockBack));

        if (health > 0f)
        {
            animator.SetTrigger("Hit");
        }
        else
        {
            Release();
        }
    }

    private IEnumerator KnockBack(float knockback)
    {
        yield return wait;
        Doknockback(knockback);
    }

    private void Doknockback(float distance = 2)
    {
        Vector3 playerPosition =
            GameManager.Instance.CurrentPlayer.transform.position;
        Vector3 dirVector = transform.position - playerPosition;
        rigid.AddRelativeForce(dirVector.normalized * distance,
            ForceMode2D.Impulse);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Attack"))
        {
            Attack atk = collision.GetComponent<Attack>();

            if (atk == null) return;

            Damage(atk);

        }
    }
}