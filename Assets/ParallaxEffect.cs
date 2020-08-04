using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    // parallax effect multiplier
    public Vector2 movementMultiplier = new Vector2(0.25f, 0.25f);

    // camera component variables
    private Vector3 prevCamPos;
    private Transform camTransform;

    // sprite variables
    Sprite sprite;
    Texture2D texture;
    private float texUnitSize;

    private void Start()
    {
        // get camera components
        camTransform = Camera.main.transform;
        prevCamPos = camTransform.position;

        // get sprite components
        sprite = GetComponent<SpriteRenderer>().sprite;
        texture = sprite.texture;
        texUnitSize = texture.width / sprite.pixelsPerUnit;
    }

    private void Update()
    {
        // get how far the camera has moved since the last frame
        Vector3 movement = camTransform.position - prevCamPos;

        // move the background with the multiplier
        transform.position += new Vector3(movement.x * movementMultiplier.x, movement.y * movementMultiplier.y);

        // set the cameras new previous position
        prevCamPos = camTransform.position;

        // make sure the background repeats
        if (Mathf.Abs(camTransform.position.x - transform.position.x) >= texUnitSize * 3)
        {
            float offsetPosition = (camTransform.position.x - transform.position.x) % texUnitSize;
            transform.position = new Vector3(camTransform.position.x + offsetPosition, transform.position.y, transform.position.z);
        }
    }
}
