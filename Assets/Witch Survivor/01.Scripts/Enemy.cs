using System.Collections;
using UnityEngine;

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

    SpriteRenderer[] spriteRenderers;

    private WaitForFixedUpdate wait;

    private bool isDead = false;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>(true);
    }

    private void Start()
    {
        target = GameManager.Instance.CurrentPlayer.GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        if (GameManager.Instance.CurrentPlayer == null)
        {
            return;
        }

        target = GameManager.Instance.CurrentPlayer.GetComponent<Rigidbody2D>();
        health = enemyData.maxHealth;
        speed = enemyData.speed;
    }
    private void FixedUpdate()
    {
        if (target == null)
        {
            return;
        }

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Hit"))
        {
            return;
        }
        if (isDead) return;

        dirVector = target.position - rigid.position;

        float xScale = dirVector.x < 0 ? -1f : 1f;
        transform.localScale = new Vector3(xScale, 1f, 1f);

        Vector2 nextPosition = dirVector.normalized * speed * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + nextPosition);
    }

    public void Damage(Attack atk)
    {
        health -= atk.currentSkill.atk;
        StartCoroutine(Knockback(atk.currentSkill.knockback));

        if (health > 0f)
        {
            animator.SetTrigger("Hit");
        }
        else
        {
            isDead = true;
            StartCoroutine(Dead());
        }

    }

    private IEnumerator Knockback(float knockback)
    {
        yield return wait;
        DoKnockback(knockback);
    }

    private void DoKnockback(float distance = 2f)
    {
        Vector3 playerPosition = GameManager.Instance.CurrentPlayer.transform.position;
        Vector3 dirVector = transform.position - playerPosition;
        rigid.AddRelativeForce(dirVector.normalized * distance, ForceMode2D.Impulse);
    }

    private IEnumerator Dead()
    {
        animator.Play("Dead");
        yield return new WaitForSeconds(0.5f);
        Release();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Attack"))
        {
            Attack atk = collision.GetComponent<Attack>();

            if (atk == null) return;

            Damage(atk);
        }
    }

    public override void Release()
    {
        transform.position = Vector3.zero;
        base.Release();
    }
}