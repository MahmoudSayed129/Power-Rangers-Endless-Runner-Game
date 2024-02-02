using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class multiplierIcon : MonoBehaviour
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
        //transform.position = player.position;
        horizontalInput = Input.GetAxis("Horizontal");
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pause = !pause;
        }
    }
}

