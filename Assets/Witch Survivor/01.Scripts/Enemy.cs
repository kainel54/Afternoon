using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Enemy : Poolable
{
    Rigidbody2D rigid = null;
    Rigidbody2D target = null;
    [SerializeField]
    float speed = 1f;

    private Vector2 dirVector = Vector2.zero;
    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();        
    }

    private void OnEnable()
    {
        target = GameManager.Instance.CurrentPlayer.GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        dirVector = target.position - rigid.position;

        float xScale = dirVector.x < 0 ? -1f : 1f;
        transform.localScale = new Vector3(xScale, 1f, 1f);

        Vector2 nextPosition = dirVector.normalized * speed * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + nextPosition);
    }
}
