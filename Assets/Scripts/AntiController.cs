using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntiController : MonoBehaviour
{

    [Header("Movement")]
    public float MaxSpeed;
    public float Acceleration;
    public float Decceleration;
    private float _speed;
    public bool Moving;

    [Header("Jumping")]
    public float jumpAcceleration;
    public float jumpDecceleration;

    public float maxRaycastAngle;
    public float _raycastAngle; 

    public bool Jumping;
    private float _jumpingSpeed;

    private Vector2 _pos;

    void Start()
    {
        _speed = 0;
    }

    void Update()
    {
        Move();
        Jump();
    }

    private void Move() 
    {
        if (Moving && _speed < MaxSpeed)
        {
            _speed += Acceleration * Time.deltaTime;
        } else 
        {
            _speed -= Decceleration * Time.deltaTime;
        }

        _speed = Mathf.Clamp(_speed, 0, MaxSpeed);

        _pos = transform.position;
        _pos.x = transform.position.x + _speed * Time.deltaTime;
        transform.position = _pos;
    }

    private void Jump()
    {
        if (Jumping)
        {
            _pos = transform.position;
            _pos.y = transform.position.y + _jumpingSpeed * Time.deltaTime;
            transform.position = _pos;

            _jumpingSpeed -= jumpDecceleration * Time.deltaTime;
            _jumpingSpeed = Mathf.Clamp(_jumpingSpeed, 0, jumpAcceleration);
            
            if (_jumpingSpeed == 0)
            {
                Jumping = false;
            }
            return;
        }

        //_raycastAngle = (maxRaycastAngle * _speed) / MaxSpeed;        

        float angle = _raycastAngle;

        Vector2 direction = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad)).normalized;

        RaycastHit2D raycast = Physics2D.Raycast(transform.position, direction);

        Vector2 pos = transform.position;

		if(raycast)
		{
    		Debug.DrawLine(transform.position, pos + direction * raycast.distance, Color.red);
		} else {
	    	Debug.DrawLine(transform.position, pos + direction * 1000, Color.white);
            Jumping = true;
            _jumpingSpeed = jumpAcceleration;
        }
    }

}
