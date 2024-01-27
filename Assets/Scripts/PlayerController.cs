using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
   //Handling Variables
	public float rotationSpeed = 700;
	public float walkSpeed = 5;

	//System
	private Quaternion targetRotation;

	//Components
	//public Gun gun;
	private CharacterController _controller;
	private Camera _cam;
	private InputSystemReader _inputReader;
	private Vector2 _directInputMove;
	private Vector2 _directInputAim;

	// Use this for initialization
	void Start () 
	{
		_controller = GetComponent<CharacterController> ();
		_cam = Camera.main;
	}
	
	void OnEnable()
	{
		_inputReader ??= GameController.Instance.InputReader;
		if (_inputReader != null)
		{
			_inputReader.MoveEvent += OnMove;
			_inputReader.AimEvent += OnAim;
		}
	}
	
	void OnDisable()
	{
		_inputReader ??= GameController.Instance.InputReader;
		if (_inputReader != null)
		{
			_inputReader.MoveEvent -= OnMove;
			_inputReader.AimEvent -= OnAim;
		}
	}
	
	private void OnAim(Vector2 aim)
	{
		_directInputAim = aim;
	}

	private void OnMove(Vector2 move)
	{
		_directInputMove = move;
	}
	
	void Update () 
	{
		Move();
		Aim();
	}

	void Aim()
	{
		Vector3 input = new Vector3(_directInputAim.x,0,_directInputAim.y);
		if(input != Vector3.zero) 
		{
			targetRotation = Quaternion.LookRotation(input);
			transform.eulerAngles = Vector3.up * Mathf.MoveTowardsAngle(transform.eulerAngles.y,targetRotation.eulerAngles.y,rotationSpeed * Time.deltaTime);
		}
	}
	
	void Move()
	{
		
		Vector3 input = new Vector3(_directInputMove.x,0,_directInputMove.y);
		var forward = _cam.transform.forward;
		var right = _cam.transform.right;
		
		forward.y = 0f;
		right.y = 0f;
		forward.Normalize();
		right.Normalize();
		
		var desiredMoveDirection =  right * input.x + forward * input.z;
		
		Vector3 move = desiredMoveDirection;
		move *= (Mathf.Abs(input.x) == 1 && Mathf.Abs(input.z) == 1) ? .7f : 1;
		move *= walkSpeed;
		move += Vector3.up * -8;
		
		_controller.Move(move * Time.deltaTime);
	}
}
