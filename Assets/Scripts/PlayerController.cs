using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	public float rotationSpeed = 700;
	public float walkSpeed = 5;
	public Animator animator;

	public TextMeshProUGUI uiMoney;
	
	public int playerMoney;
	
	private Quaternion targetRotation;

	private Gun[] _allGuns;
	internal Gun equippedGun;
	private CharacterController _controller;
	private Camera _cam;
	private InputSystemReader _inputReader;
	private Vector2 _directInputMove;
	private Vector2 _directInputAim;
	
	

	private bool _shooting;
	
	void Start () 
	{
		_controller = GetComponent<CharacterController> ();
		_cam = Camera.main;
		equippedGun = GetComponentInChildren<Gun>();
		_allGuns = GetComponentsInChildren<Gun>(true);
	}
	
	void OnEnable()
	{
		_inputReader ??= GameController.Instance.InputReader;
		if (_inputReader != null)
		{
			_inputReader.MoveEvent += OnMove;
			_inputReader.AimEvent += OnAim;
			_inputReader.ShootEvent += OnShoot;
			_inputReader.ShootSecondaryEvent += OnShoot;
			_inputReader.ShootCancelledEvent += OnShootStop;
			_inputReader.ShootSecondaryEvent -= OnShootSecondary;
			_inputReader.ShootSecondaryCancelledEvent += OnShootSecondary;
		}
	}
	
	void OnDisable()
	{
		_inputReader ??= GameController.Instance.InputReader;
		if (_inputReader != null)
		{
			_inputReader.MoveEvent -= OnMove;
			_inputReader.AimEvent -= OnAim;
			_inputReader.ShootEvent -= OnShoot;
			_inputReader.ShootSecondaryEvent -= OnShoot;
			_inputReader.ShootCancelledEvent -= OnShootStop;
			_inputReader.ShootSecondaryEvent -= OnShootSecondary;
			_inputReader.ShootSecondaryCancelledEvent -= OnShootSecondaryCancelled;
		}
	}

	private void OnShootSecondaryCancelled()
	{
		
	}

	private void OnShootSecondary()
	{
		
	}


	private void OnShootStop()
	{
		_shooting = false;
	}

	private void OnShoot()
	{
		if (equippedGun.gunFireMode == GunFireMode.Auto)
		{
			_shooting = true;
		}
		
		equippedGun.Shoot();
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
		uiMoney.text = $"Stoicism: {playerMoney}";
		Move();
		Aim();

		if (_directInputMove.magnitude > 0)
		{
			animator.Play("Run");
			animator.speed = _directInputMove.magnitude;
		}
		else
		{
			animator.speed = 1;
			animator.Play("Stand");
		}
		
		if (_shooting)
		{
			equippedGun.Shoot();
		}
	}

	public void AddMoney(int addMoney)
	{
		playerMoney += addMoney;
	}
	
	public void SetPlayerGun(string gunName)
	{
		foreach (var gun in _allGuns)
		{
			if (gun.name == gunName)
			{
				gun.gameObject.SetActive(true);
				equippedGun = gun;
			}
			else
			{
				gun.gameObject.SetActive(false);
			}
		}
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
