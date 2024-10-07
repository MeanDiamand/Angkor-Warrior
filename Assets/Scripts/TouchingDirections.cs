using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Using the collider to check the directions if the object is currently on the ground, touch the wall or ceiling
public class TouchingDirections : MonoBehaviour
{
    public ContactFilter2D contactFilter;
    public float groundDist = 0.05f;
    public float wallDist = 0.2f;
    public float ceilingDist = 0.05f;
    CapsuleCollider2D touchCollider;
    Animator animator;

    RaycastHit2D[] groundHits = new RaycastHit2D[5];
    RaycastHit2D[] wallHits = new RaycastHit2D[5];
    RaycastHit2D[] ceilingHits = new RaycastHit2D[5];

    private Vector2 wallCheckDirection => gameObject.transform.localScale.x > 0 ? Vector2.right : Vector2.left;

    [SerializeField]
    private bool _isGrounded;

    public bool IsGrounded
    { 
        get { return _isGrounded; } 
        private set
        {
            _isGrounded = value;
            animator.SetBool(AnimationStrings.isGrounded, value);
        }
    }

    [SerializeField]
    private bool _isOnWall;

    public bool IsOnWall
    {
        get { return _isOnWall; }
        private set
        {
            _isOnWall = value;
            animator.SetBool(AnimationStrings.isOnWall, value);
        }
    }

    [SerializeField]
    private bool _isOnCeiling;

    public bool IsOnCeiling
    {
        get { return _isOnCeiling; }
        private set
        {
            _isOnCeiling = value;
            animator.SetBool(AnimationStrings.isOnCeiling, value);
        }
    }

    private void Awake()
    {
        touchCollider = GetComponent<CapsuleCollider2D>();
        animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        IsGrounded = touchCollider.Cast(Vector2.down, contactFilter, groundHits, groundDist) > 0;
        IsOnWall = touchCollider.Cast(wallCheckDirection, contactFilter, wallHits, wallDist) > 0;
        IsOnCeiling = touchCollider.Cast(Vector2.up, contactFilter, ceilingHits, ceilingDist) > 0;
    }
}
