using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public int attackDamage = 10;
    public Vector2 knock = Vector2.zero;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // See if the target is valid to be hit
        Damageable damageable = collision.GetComponent<Damageable>();

        if (damageable != null)
        {
            //flip the knock
            Vector2 deliveredKnock = transform.parent.localScale.x > 0 ? knock : new Vector2(-knock.x, knock.y);

            // Hit the target
            bool Hitted = damageable.Hit(attackDamage, deliveredKnock);

            if (Hitted)
            {
                Debug.Log(collision.name + " hit for " + attackDamage);
            }
        }
    }
}
