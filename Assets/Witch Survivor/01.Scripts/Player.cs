using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float speed = 3f;
    [SerializeField]
    private float coolTime = 1.35f;

    private Rigidbody2D rigid2D = null;
    private Animator animator = null;
    private SkillSlot[] skillSlots = null;

    public Vector2 moveDirection { get; private set; }

    private void Awake()
    {
        rigid2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        skillSlots = GetComponentsInChildren<SkillSlot>();
    }

    private void Start()
    {
        SetSkillSlot();
    }

    private void FixedUpdate()
    {
        if (rigid2D == null)
        {
            return;
        }

        if (animator == null)
        {
            return;
        }

        animator.SetFloat("velocity", moveDirection.magnitude);
    }

    private void LateUpdate()
    {
        rigid2D.MovePosition(rigid2D.position + moveDirection * speed * Time.fixedDeltaTime);

        if (moveDirection.x == 0) return;
        float xScale = moveDirection.x < 0 ? -1f : 1f;
        transform.localScale = new Vector3(xScale, 1f, 1f);
    }

    private void OnMove(InputValue value)
    {
        moveDirection = value.Get<Vector2>();
    }

    private void SetSkillSlot()
    {
        for (int i = 0; i < skillSlots.Length; i++)
        {
            skillSlots[i].Init(i);
        }
    }
}
