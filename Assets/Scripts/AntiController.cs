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

    public Transform NoFloorPoint;
    public float maxRaycastAngle;
    public float _raycastAngle; 

    public Transform WallJumpPoint;
    public float wallJumpDistance;

    public bool Jumping;
    private float _jumpingSpeed;

    [Header("Grounded")]
    public bool Grounded;
    public float GroundedSize;
    public Transform GroundCheckPoint;


    private Vector2 _pos;

    void Start()
    {
        _speed = 0;
    }

    void Update()
    {
        GroundCheck();
        Move();
        Jump();
    }

    private void GroundCheck()
    {
        Grounded = Physics2D.OverlapCircle(GroundCheckPoint.position, GroundedSize);
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

        if (!Grounded) return;            

        //_raycastAngle = (maxRaycastAngle * _speed) / MaxSpeed;        

        /* NoFloorCheck */
        float angle = _raycastAngle;
        Vector2 direction = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad)).normalized;
        RaycastHit2D raycast = Physics2D.Raycast(NoFloorPoint.position, direction);
		if(raycast)
		{
    		Debug.DrawLine(NoFloorPoint.position, (Vector2) NoFloorPoint.position + direction * raycast.distance, Color.red);
		} else {
	    	Debug.DrawLine(NoFloorPoint.position, (Vector2) NoFloorPoint.position + direction * 1000, Color.white);
            Jumping = true;
            _jumpingSpeed = jumpAcceleration;
        }

        /* WallCheck */
        RaycastHit2D raycastWall = Physics2D.Raycast(NoFloorPoint.position, Vector2.right, wallJumpDistance);
		if(raycastWall)
		{
    		Debug.DrawLine(WallJumpPoint.position, (Vector2) WallJumpPoint.position + Vector2.right * raycastWall.distance, Color.red);
            Jumping = true;
            _jumpingSpeed = jumpAcceleration;
		} else {
	    	Debug.DrawLine(WallJumpPoint.position, (Vector2) WallJumpPoint.position + Vector2.right * wallJumpDistance, Color.white);
        }

    }

}
