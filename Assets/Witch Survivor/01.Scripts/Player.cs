using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private Rigidbody2D rigid2d;
    private Animator animator;
    public Vector2 moveDirection { get; private set; }
    
    [SerializeField] float speed = 3f;

    private void Awake()
    {
        rigid2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }
    private void FixedUpdate()
    {
        if (rigid2d == null)
        {
            return;
        }

        if (animator == null)
        {
            return;
        }
        animator.SetFloat("Speed",moveDirection.magnitude);

        float xScale = moveDirection.x < 0 ? -1f: 1f ;
        transform.localScale = new Vector3(xScale, 1f, 1f);

        if(moveDirection.magnitude > speed) { }

        rigid2d.MovePosition(rigid2d.position + moveDirection*speed * Time.fixedDeltaTime);
    }

    private void OnMove(InputValue value)
    {
        moveDirection = value.Get<Vector2>();
    }

    private void LateUpdate()
    {
        
    }
}
