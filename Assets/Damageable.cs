using Assets.Scripts.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Damageable : MonoBehaviour
{
    public UnityEvent<int, Vector2> damageableHit;
    public UnityEvent<int, int> healthChanged;
    Animator animator;

    [SerializeField]
    private int maxHealth = 100;

    public int MaxHealth
    {
        get
        {
            return maxHealth;
        }
        set
        {
            maxHealth = value;
        }
    }

    [SerializeField]
    private int health = 100;


    public int Health
    {
        get
        {
            return health;
        }
        set
        {
            health = value;
            healthChanged?.Invoke(health, MaxHealth);
            // If the health drops below 0, the warrior will not alive anymore
            if (health <= 0)
            {
                IsAlive = false;
            }
        }
    }

    [SerializeField]
    private bool isAlive = true;

    [SerializeField]
    private bool isInvincible = false;

    private float timeSinceHit = 0;
    public float invincibilityTime = 0.25f;

    public bool IsAlive
    {
        get
        {
            return isAlive;
        }
        set
        {
            isAlive = value;
            animator.SetBool(AnimationStrings.isAlive, value);
            Debug.Log("IsAlive: " + value);
        }
    }

    // The velocity shouldn't be change when this is true but needs to be respected by the other physics components like the PlayerController
    public bool LockVelocity
    {
        get
        {
            return animator.GetBool(AnimationStrings.lockVelocity);
        }
        set
        {
            animator.SetBool(AnimationStrings.lockVelocity, value);
        }
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void Update()
    {
        if(isInvincible)
        {
            if(timeSinceHit > invincibilityTime)
            {
                // removing the invincibility
                isInvincible = false;
                timeSinceHit = 0;
            }
            timeSinceHit += Time.deltaTime;
        }
    }

    // Returns when the damageble took damage or not
    public bool Hit(int damage, Vector2 knock)
    {
        if(IsAlive && !isInvincible)
        {
            Health -= damage;
            isInvincible = true;

            // Notify the other subscribed components that the damageable was hit to handle the knock
            animator.SetTrigger(AnimationStrings.hitTrigger);
            LockVelocity = true;
            damageableHit?.Invoke(damage, knock);
            CharacterEvents.characterDamaged.Invoke(gameObject, damage);

            return true;
        }

        // Unable to be hit
        return false;
    }

    // returns if the character can be heal or not
    public bool Heal(int healthRestore)
    {
        // Check if the character is alive and doesn't have a full health then it can be heal
        if(IsAlive && Health < MaxHealth)
        {
            int maxHeal = Mathf.Max(MaxHealth - Health, 0);
            int actualHeal = Mathf.Min(maxHeal, healthRestore);
            Health += actualHeal;

            CharacterEvents.characterHealed(gameObject, actualHeal);
            return true;
        }

        // otherwise not
        return false;
    }
}
