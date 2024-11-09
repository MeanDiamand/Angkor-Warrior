using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirections), typeof(Damageable))]
public class PlayerController : MonoBehaviour
{
    public float walkSpeed = 5f;
    public float jumpImpulse = 10f;
    Vector2 moveInput;
    Rigidbody2D rb;
    TouchingDirections touchingDirections;
    Damageable damageable;

    AudioManager audioManager;

    [SerializeField]
    private float deathBoundaryY = -10f;

    public float CurrentMoveSpeed
    {
        get
        {
            if(CanMove)
            {
                if (IsMoving && !touchingDirections.IsOnWall)
                {
                    return walkSpeed;
                }
                else
                {
                    // Idle speed = 0
                    return 0;
                }
            }
            else
            {
                //Movement Lock
                return 0;
            }
        }
    }

    [SerializeField]
    private bool _isMoving = false;

    public bool IsMoving 
    {
        get { return _isMoving; }
        private set 
        {  
            _isMoving = value;
            animator.SetBool(AnimationStrings.isMoving, value);
        } 
    }

    public bool _isFacingRight = true;

    public bool IsFacingRight 
    {
        get { return _isFacingRight; }
        private set
        {
            // Reverse only if the value is new
            if (_isFacingRight != value)
            {
                // Reverse the localscale to make the character to face the other direction
                transform.localScale *= new Vector2(-1, 1);
            }
            _isFacingRight = value;
        } 
    }

    public bool CanMove
    {
        get
        {
            return animator.GetBool(AnimationStrings.canMove);
        }
    }

    public bool IsAlive
    {
        get
        {
            return animator.GetBool(AnimationStrings.isAlive);
        }
    }

    Animator animator;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        touchingDirections = GetComponent<TouchingDirections>();
        damageable = GetComponent<Damageable>();    
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    private void FixedUpdate()
    {
        if (!damageable.LockVelocity)
        {
            rb.velocity = new Vector2(moveInput.x * CurrentMoveSpeed, rb.velocity.y);
        }
        
        animator.SetFloat(AnimationStrings.yVelocity, rb.velocity.y);

        // Check if the player has fallen below the death boundary
        if (transform.position.y < deathBoundaryY)
        {
            damageable.Health = 0; // This will trigger the death logic in the Damageable class
            Destroy(gameObject);
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();

        if(IsAlive)
        {
            IsMoving = moveInput != Vector2.zero;

            SetFacingDirection(moveInput);
            audioManager.PlaySFX(audioManager.walking);
        }
        else
        {
            IsMoving = false;
        }
    }

    private void SetFacingDirection(Vector2 moveInput)
    {
        if(moveInput.x > 0 && !IsFacingRight)
        {
            IsFacingRight = true;
        }
        else if(moveInput.x < 0 && IsFacingRight)
        {
            IsFacingRight = false;
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        // Need to check if alive later
        if(context.started && touchingDirections.IsGrounded && CanMove)
        {
            animator.SetTrigger(AnimationStrings.jumpTrigger);
            rb.velocity = new Vector2(rb.velocity.x, jumpImpulse);
        }
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            audioManager.PlaySFX(audioManager.swordSwing);
            animator.SetTrigger(AnimationStrings.attackTrigger);
        }
    }

    public void OnHit(int damage, Vector2 knock)
    {
        rb.velocity = new Vector2(knock.x, rb.velocity.y + knock.y);
    }
}
