using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed;

    private Transform model;

    private Boolean isMoving;
    private float startTime;
    private Vector3 origin;
    private Vector3 destination;

	// Use this for initialization
	void Start ()
	{
	    isMoving = false;
	    origin = transform.position;
        destination = transform.position;

	    model = transform.GetChild(0);
	}
	
	// Update is called once per frame
	void Update ()
	{
	    if (!isMoving)
	    {
	        startTime = Time.time;
            origin = transform.position;
            Vector3 direction = Vector3.zero;

            if (Input.GetKey(KeyCode.W))
	        {
	            isMoving = true;
	            direction =  Vector3.forward;
            }
	        else if (Input.GetKey(KeyCode.A))
	        {
	            isMoving = true;
	            direction =  Vector3.left;
            }
	        else if (Input.GetKey(KeyCode.S))
	        {
	            isMoving = true;
	            direction = Vector3.back;
            }
	        else if (Input.GetKey(KeyCode.D))
	        {
	            isMoving = true;
	            direction = Vector3.right;
            }
            //check whether the position is not blocked and if it is, don't move
	        destination = transform.position + direction;
        }

	    if (isMoving)
	    {
	        Move();
	    }
    }

    private void Move()
    {
        float distanceCovered = (Time.time - startTime) * speed;
        transform.position = Vector3.Lerp(origin, destination, distanceCovered);
        //rotate model in moving direction to "roll over the edge"
        isMoving = !(transform.position == destination);

    }
}
