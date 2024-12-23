using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    public new Camera camera;
    public Transform Target;

    // Start position of the parallax game object
    Vector2 startPosition;

    // Start Z value of the parallax game object
    float startZ;

    // Distance of the camera has moved from the start position of the parallax game object
    Vector2 cameraMoveSinceStart => (Vector2)camera.transform.position - startPosition;

    // Only calculate zDistFromTarget if Target is not null
    float zDistFromTarget => Target != null ? transform.position.z - Target.transform.position.z : 0;

    // Use nearClipPlane when the object is in front of the target, use farClipPlane when it is behind
    float clippingPlane => (camera.transform.position.z + (zDistFromTarget > 0 ? camera.farClipPlane : camera.nearClipPlane));

    // The further the object is from the player, the faster the ParallaxEffect object will move, dragging its z value closer to the target to make it move slower
    float parallaxFactor => Target != null ? Mathf.Abs(zDistFromTarget) / clippingPlane : 0;

    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
        startZ = transform.position.z;
    }

    // Update is called once per frame
    void Update()
    {
        // Check if Target is still assigned
        if (Target != null)
        {
            // Move the parallax object with same distance times a multiplier when the target is moving
            Vector2 newPos = startPosition + cameraMoveSinceStart * parallaxFactor;

            // The XY position will change based on the target travel speed times the parallax factor, but Z
            transform.position = new Vector3(newPos.x, newPos.y, startZ);
        }
        else
        {
            enabled = false; // Disable this script if Target is null
        }
    }
}