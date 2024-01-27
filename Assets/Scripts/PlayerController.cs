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
	private CharacterController controller;
	private Camera cam;
	private InputSystemReader _inputReader;
	private Vector2 _directInputMove;
	private Vector2 _directInputAim;

	// Use this for initialization
	void Start () 
	{
		controller = GetComponent<CharacterController> ();
		cam = Camera.main;
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
		//if (moveAttackCursorBack) return;
		_directInputMove = move;
	}
	
	
	// Update is called once per frame
	void Update () 
	{
		Move();

		// if (Input.GetButtonDown("Shoot")) {
		// 	//gun.Shoot();
		// } 
		// else if (Input.GetButton("Shoot")) {
		// 	//gun.ShootContinous();
		// }
	}
	
	// void ControlAim()
	// {
	// 	Vector3 mousePos = Input.mousePosition;
	// 	mousePos = cam.ScreenToWorldPoint(new Vector3(mousePos.x,mousePos.y,cam.transform.position.y-transform.position.y));
	// 	targetRotation = Quaternion.LookRotation(mousePos - new Vector3(transform.position.x,0,transform.position.z));
 //                            
	// 	transform.eulerAngles = Vector3.up * Mathf.MoveTowardsAngle(transform.eulerAngles.y,targetRotation.eulerAngles.y,rotationSpeed * Time.deltaTime);
	//
	// 	Vector3 input = new Vector3(_directInputAim.x,0,_directInputAim.y);
	// 	Vector3 motion = input;
	// 	motion *= (Mathf.Abs(input.x) == 1 && Mathf.Abs(input.z) == 1) ? .7f : 1;
	// 	motion *= (Input.GetButton("Run")) ? runSpeed : walkSpeed;
	// 	motion += Vector3.up * -8;
	// 	
	// 	controller.Move(motion * Time.deltaTime);
	// }

	void Move()
	{
		Vector3 input = new Vector3(_directInputMove.x,0,_directInputMove.y);
		
		if(input != Vector3.zero) 
		{
			targetRotation = Quaternion.LookRotation(input);
			transform.eulerAngles = Vector3.up * Mathf.MoveTowardsAngle(transform.eulerAngles.y,targetRotation.eulerAngles.y,rotationSpeed * Time.deltaTime);
		}
		
		Vector3 move = input;
		move *= (Mathf.Abs(input.x) == 1 && Mathf.Abs(input.z) == 1) ? .7f : 1;
		move *= walkSpeed;
		move += Vector3.up * -8;
		
		controller.Move(move * Time.deltaTime);
	}
}
