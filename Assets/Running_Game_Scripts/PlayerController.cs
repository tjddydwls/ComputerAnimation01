using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;

    [Header("Jump Settings")]
    public float jumpHeight = 2f;
    public float jumpDuration = 0.5f;

    private SpriteRenderer spriteRenderer;
    private bool isJumping = false;
    private float jumpTimer = 0f;
    private Vector3 startPosition;
    private bool isMovingRight = false;

    [Header("Animation Controller")]
    public RuntimeAnimatorController idleController;
    public RuntimeAnimatorController jumpController;
    public RuntimeAnimatorController runController;

    private Animator animator;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        animator.runtimeAnimatorController = idleController;
    }

    void Update()
    {
        Vector2 moveDirection = Vector2.zero;

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            moveDirection.x -= 1f;
            isMovingRight = false;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            moveDirection.x += 1f;
            isMovingRight = true;
        }

        if (moveDirection.x > 0f)
        {
            spriteRenderer.flipX = false;
        }
        else if (moveDirection.x < 0f)
        {
            spriteRenderer.flipX = true;
        }

        if (!isJumping)
        {
           if (moveDirection.x != 0) 
            {
                animator.runtimeAnimatorController = runController;
            }
            else
            {
                animator.runtimeAnimatorController = idleController;
            } 
        }

        moveDirection = moveDirection.normalized;
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.Space) && !isJumping)
        {
            StartJump();
        }

        if (isJumping)
        {
            UpdateJump();
        }
        isMovingRight = false;
    }

    void StartJump()
    {
        isJumping = true;
        jumpTimer = 0f;
        startPosition = transform.position;

        animator.runtimeAnimatorController = jumpController;
    }

    void UpdateJump()
    {
        jumpTimer += Time.deltaTime;
        float progress = jumpTimer / jumpDuration;

        if (progress >= 1f)
        {
            transform.position = new Vector3(transform.position.x, startPosition.y, transform.position.z);
            isJumping = false;
            animator.runtimeAnimatorController = idleController;
        }
        else
        {
            float height = Mathf.Sin(progress * Mathf.PI) * jumpHeight;
            transform.position = new Vector3(transform.position.x, startPosition.y + height, transform.position.z);
        }
    }
}
