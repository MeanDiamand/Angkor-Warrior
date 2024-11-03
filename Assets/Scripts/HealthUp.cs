using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthUp : MonoBehaviour
{
    public int healthRestore = 20;
    public Vector3 rotationSpeed = new Vector3(0, 180, 0);
    AudioManager audioManager;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Damageable character for detecting with it walked to the pickup zone
        Damageable damageable = collision.GetComponent<Damageable>();

        // if it is a damageable character in this pickup collider zone then it will get heal and the healing item will be destroyed
        if(damageable)
        {
            bool wasHealed = damageable.Heal(healthRestore);

            // Check if the character is healed or not
            if (wasHealed)
            {
                audioManager.PlaySFX(audioManager.eat);
                Destroy(gameObject);
            }
        }
    }

    private void Update()
    {
        transform.eulerAngles += rotationSpeed * Time.deltaTime;
    }
}
