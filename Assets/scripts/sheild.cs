using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sheild : MonoBehaviour
{
    public float speed = 10;
    float horizontalInput;
    public float horizontalMultiplier = 3;
    public Transform player;
    public bool pause = false;
    void Start()
    {
        // Get the renderer component of the player
    }
    private void FixedUpdate()
    {
        if (pause) return;
        Vector3 moveForward = transform.forward * speed * Time.deltaTime;
        Vector3 moveHorizontal = transform.right * horizontalInput * Time.deltaTime * speed * horizontalMultiplier;
        transform.position += moveForward;
        if ((moveHorizontal + transform.position).x < 5 && (moveHorizontal + transform.position).x > -5)
        {
            transform.position += moveHorizontal;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0); // Get the first touch
            if (touch.phase == TouchPhase.Moved)
            {
                float swipeDelta = touch.deltaPosition.x;
                float movement = swipeDelta * speed * Time.deltaTime * 0.1f;
                Vector3 vector3 = transform.position + Vector3.right * movement;
                if (vector3.x <= 5 && vector3.x >= -5)
                    transform.Translate(Vector3.right * movement);
            }
        }
        horizontalInput = Input.GetAxis("Horizontal");
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pause = !pause;
        }
    }
}
