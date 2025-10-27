using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(moving))]
public class PlayerHandler : MonoBehaviour
{
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    private moving moveScript;

    void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        moveScript = GetComponent<moving>();
    }

    void Update()
    {
        // Nhấn phím H để thực hiện hành động "Hoe"
        if (Keyboard.current != null && Keyboard.current.hKey.wasPressedThisFrame)
        {
            TriggerHoeAction();
        }
    }

    public void UpdateMovementAnimation(Vector2 velocity, Vector2 movementInput)
    {
        if (animator == null || moveScript == null) return;

        Vector2 lastDir = moveScript.lastMoveDirection.normalized;
        float currentSpeed = movementInput.magnitude;

        // 1️⃣ Gửi tốc độ
        animator.SetFloat("speed", currentSpeed);

        // 2️⃣ Gửi hướng (nếu bạn có Blend Tree 4 hướng)
        animator.SetFloat("horizontal", Mathf.Abs(lastDir.x)); // LUÔN DƯƠNG để dùng cùng animation WalkRight
        animator.SetFloat("vertical", lastDir.y);

        // 3️⃣ Lật sprite nếu di chuyển sang trái
        UpdateSpriteDirection(lastDir.x);
    }

    public void TriggerHoeAction()
    {
        if (animator == null || moveScript == null) return;

        Vector2 lastDir = moveScript.lastMoveDirection.normalized;

        if (rb != null)
            rb.linearVelocity = Vector2.zero;

        animator.SetFloat("speed", 0f);

        float absX = Mathf.Abs(lastDir.x);
        float absY = Mathf.Abs(lastDir.y);

        if (absY >= absX)
        {
            if (lastDir.y > 0)
                animator.SetTrigger("HoeUp");
            else
                animator.SetTrigger("HoeDown");
        }
        else
        {
            animator.SetTrigger("HoeRight");
        }

        UpdateSpriteDirection(lastDir.x);
    }

    private void UpdateSpriteDirection(float lastMoveX)
    {
        if (spriteRenderer == null) return;

        // Lật sprite khi đi trái
        if (lastMoveX < 0)
            spriteRenderer.flipX = true;
        else if (lastMoveX > 0)
            spriteRenderer.flipX = false;
    }
}
