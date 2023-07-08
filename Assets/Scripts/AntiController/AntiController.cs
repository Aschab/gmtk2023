using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntiController : MonoBehaviour
{
    [Header("RaycastPoints")]
    public Transform HeadPoint;
    public Transform MiddlePoint;
    public Transform FootPoint;
    public Transform ThirdEye;

    [Header("Movement")]
    public float MaxSpeed;
    public float Acceleration;
    public float Decceleration;
    private float _speed;
    public bool forward;
    public bool Moving;
    public float changeDistanceFrom;

    [Header("Jumping")]
    public float maxJumpHeight;
    public float jumpStart;
    public float jumpDecceleration;
    public float minJumpingTime;
    public float _jumpingTime;
    public float _jumpingHeight;

    public float maxRaycastAngle;
    public float _raycastAngle; 

    public float wallJumpDistance;

    public bool Jumping;
    public float _jumpingSpeed;

    [Header("Falling")]
    public float fallingAcceleration;
    public float maxFallingAcceleration;
    public bool Falling;
    public float _fallingSpeed;

    [Header("Grounded")]
    public bool Grounded;
    public float GroundedSize;
    public float minGroundTime;
    private float _groundTime;


    private Vector2 _pos;

    void Start()
    {
        _speed = 0;
        _jumpingSpeed = jumpStart;
        Moving = true;
        forward = true;
    }

    void Update()
    {
        DirectionCheck();
        GroundCheck();
        Move();
        Jump();
        Fall();
    }

    private void DirectionCheck()
    {
        float angle = forward ? 0 : 180;
        Vector2 direction = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad)).normalized;
        RaycastHit2D raycastDir = Physics2D.Raycast(ThirdEye.position, direction, changeDistanceFrom);
        RaycastHit2D raycastDir2 = Physics2D.Raycast(FootPoint.position, direction, changeDistanceFrom);

		if(raycastDir && raycastDir2)
		{
            forward = !forward;
    		Debug.DrawLine(ThirdEye.position, (Vector2) ThirdEye.position + direction * raycastDir.distance, Color.red);
    		Debug.DrawLine(FootPoint.position, (Vector2) FootPoint.position + direction * raycastDir.distance, Color.red);
		} else {
	    	Debug.DrawLine(ThirdEye.position, 
                (Vector2) ThirdEye.position + direction * changeDistanceFrom,
                 Color.white);
	    	Debug.DrawLine(FootPoint.position, 
                (Vector2) FootPoint.position + direction * changeDistanceFrom,
                 Color.white);
        }
    }

    private void GroundCheck()
    {
        Grounded = Physics2D.OverlapCircle(FootPoint.position, GroundedSize);
        if (Grounded)
        {
            _groundTime += Time.deltaTime;
        } else {
            _groundTime = 0;
        }
    }

    private bool StayGrounded()
    {
        return Grounded && _groundTime < minGroundTime;
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
        if (forward)
        {
            _pos.x = transform.position.x + _speed * Time.deltaTime;
        } else {
            _pos.x = transform.position.x - _speed * Time.deltaTime;
        }
        transform.position = _pos;
    }

    private void Fall()
    {
        Falling = false;
        if (Grounded || Jumping) {
            _fallingSpeed = 0f;
            return;
        }
        Falling = true;
        _fallingSpeed += fallingAcceleration * Time.deltaTime;
        _fallingSpeed = Mathf.Clamp(_fallingSpeed, 0, maxFallingAcceleration);
        
        _pos = transform.position;
        _pos.y = transform.position.y - _fallingSpeed * Time.deltaTime;
        transform.position = _pos;
    }

    private void Jump()
    {
        float angle = forward ? - 90 + _raycastAngle : - 90 - _raycastAngle;
        Vector2 direction = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad)).normalized;
        RaycastHit2D raycastFloor = Physics2D.Raycast(HeadPoint.position, direction);
        if(raycastFloor)
		{
    		Debug.DrawLine(HeadPoint.position, (Vector2) HeadPoint.position + direction * raycastFloor.distance, Color.red);
		} else {
	    	Debug.DrawLine(HeadPoint.position, (Vector2) HeadPoint.position + direction * 1000, Color.white);
        }

        Vector2 wallDirection = forward ? Vector2.right : Vector2.left;
        RaycastHit2D raycastWallFoot = Physics2D.Raycast(FootPoint.position, wallDirection, wallJumpDistance);
		if(raycastWallFoot)
		{
    		Debug.DrawLine(FootPoint.position, (Vector2) FootPoint.position + wallDirection * raycastWallFoot.distance, Color.red);
		} else {
	    	Debug.DrawLine(FootPoint.position, 
                (Vector2) FootPoint.position + wallDirection * wallJumpDistance,
                 Color.white);
        }

        RaycastHit2D raycastWallHead = Physics2D.Raycast(HeadPoint.position, wallDirection, wallJumpDistance);
		if(raycastWallHead)
		{
    		Debug.DrawLine(HeadPoint.position, (Vector2) HeadPoint.position + wallDirection * raycastWallHead.distance, Color.red);
		} else {
	    	Debug.DrawLine(HeadPoint.position, 
                (Vector2) HeadPoint.position + wallDirection * wallJumpDistance,
                 Color.white);
        }

        if (Jumping)
        {
            _jumpingSpeed -= jumpDecceleration * Time.deltaTime;

            _pos = transform.position;
            _pos.y = transform.position.y + _jumpingSpeed * Time.deltaTime;
            _jumpingHeight += _jumpingSpeed * Time.deltaTime;
            transform.position = _pos;
            
            _jumpingTime += Time.deltaTime;

            if (_jumpingTime < minJumpingTime) {
                return;
            }
            
            if (_jumpingHeight >= maxJumpHeight)
            {
                Jumping = false;
            }
            
            if (!Jumping) 
            {
                _jumpingTime = 0;
                _jumpingSpeed = jumpStart;
                _jumpingHeight = 0;
            }
            return;
        }

        if (!Grounded) return;            

        //_raycastAngle = (maxRaycastAngle * _speed) / MaxSpeed;        

		if(!StayGrounded() && (raycastWallFoot || raycastWallHead))
		{
            Jumping = true;
            _jumpingTime = 0;
            _jumpingSpeed = jumpStart;
            _jumpingHeight = 0;
		}

    }

}