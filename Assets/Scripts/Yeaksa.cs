using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirections))]
public class Yeaksa : MonoBehaviour
{
    public float walkSpeed = 3f;
    Rigidbody2D rb;
    public enum WalkingDirection { Right, Left }
    private WalkingDirection walkDirection;
    private Vector2 walkDirectionVector = Vector2.right;
    TouchingDirections touchingDirections;
    public DetectionZone detectAttackZone;
    Animator animator;
    public float walkStopRate = 0.05f;

    public WalkingDirection WalkDirection
    {
        get { return walkDirection; }
        set
        {
            if(walkDirection != value)
            {
                //Changing the direction
                gameObject.transform.localScale = new Vector2(gameObject.transform.localScale.x * -1, gameObject.transform.localScale.y);
                if(value == WalkingDirection.Right )
                {
                    walkDirectionVector = Vector2.right;
                }
                else if(value == WalkingDirection.Left )
                {
                    walkDirectionVector = Vector2.left;
                }
            }
            walkDirection = value;
        }
    }

    public bool hasTarget = false;

    public bool HasTarget 
    { 
        get { return hasTarget; } 
        private set
        {
            hasTarget = value;
            animator.SetBool(AnimationStrings.hasTarget, value);
        }
    }

    public bool CanMove
    {
        get
        {
            return animator.GetBool(AnimationStrings.canMove);
        }
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        touchingDirections = GetComponent<TouchingDirections>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        HasTarget = detectAttackZone.detectedColliders.Count > 0;
    }

    private void FixedUpdate()
    {
        if(touchingDirections.IsGrounded && touchingDirections.IsOnWall)
        {
            FlipDirection();
        }
        if(CanMove)
        {
            rb.velocity = new Vector2(walkSpeed * walkDirectionVector.x, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(Mathf.Lerp(rb.velocity.x, 0, walkStopRate), rb.velocity.y);
        }
    }

    private void FlipDirection()
    {
        if(WalkDirection == WalkingDirection.Right)
        {
            WalkDirection = WalkingDirection.Left;
        }
        else if(WalkDirection == WalkingDirection.Left)
        {
            WalkDirection = WalkingDirection.Right;
        }
        else
        {
            Debug.LogError("Walking Direction is not currently set to left or right");
        }
    }
}
