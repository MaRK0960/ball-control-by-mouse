using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class BallControlbyMouse : MonoBehaviour
{
    public float MovePower = 100f;
    public float TopSpeed = 20.0f;

    private Vector3 move;           //vector starts from the current ball position and to the location of mouse click
    private float _InertiaAngle;    //angle between current move direction -velocity- and the applied force direction
    private Vector3 Velocity;       //Current move direction of ball

    private Rigidbody _Rigidbody;

    private void Awake()
    {
        _Rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (Input.GetMouseButton(0))                //when mouse is clicked get the direction
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit = new RaycastHit();

            if (Physics.Raycast(ray, out hit))
            {
                move = hit.point - transform.position;
                move.y = 0;
                Move(move.normalized);
            }
        }

        if (Velocity.sqrMagnitude > TopSpeed * TopSpeed)                 //maintain top speed
            Velocity = Velocity.normalized * TopSpeed;
    }

    public void Move(Vector3 moveDirection)                 //how to move the ball depending on angle between click and velocity vectors
    {

        Velocity = _Rigidbody.velocity.normalized;
        _InertiaAngle = Vector3.Angle(moveDirection, Velocity);

        if (_InertiaAngle > 115.0f)
        {

            _Rigidbody.velocity *= 0.01f;
        }
        else if (_InertiaAngle > 55.0f)
        {
            _Rigidbody.AddForce((-Velocity + moveDirection) * MovePower * 7, ForceMode.Acceleration);
            _Rigidbody.AddForce(-moveDirection * MovePower * 3, ForceMode.Acceleration);
        }
        else
        {
            _Rigidbody.AddForce((-Velocity + moveDirection) * MovePower * 7, ForceMode.Acceleration);
        }

        _Rigidbody.AddForce(moveDirection * MovePower, ForceMode.Acceleration);
    }
}