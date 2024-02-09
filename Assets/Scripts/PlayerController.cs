using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float rotationSpeed = 700;
    public float walkSpeed = 5;
    public Animator animator;
    public Texture2D cursorTexture;

    public GameObject uiMoney;

    public int playerMoney;

    private Quaternion targetRotation;

    private Gun[] _allGuns;
    internal Gun equippedGun;
    private CharacterController _controller;
    private Camera _cam;
    private InputSystemReader _inputReader;
    private Vector2 _directInputMove;
    private Vector2 _directInputAim;
    private LayerMask _mousePlaneLayer;


    private bool _shooting;
    private Vector3 _lastPos;

    [SerializeField] private GunUI gunUI;


    void Start()
    {
        _mousePlaneLayer = LayerMask.GetMask("Mouse");
        _controller = GetComponent<CharacterController>();
        _cam = Camera.main;
        equippedGun = GetComponentInChildren<Gun>();
        _allGuns = GetComponentsInChildren<Gun>(true);

        print(_allGuns);
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


    void Update()
    {
        uiMoney.GetComponent<TextMeshProUGUI>().text = $"Stoicism: {playerMoney}";
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

        if(equippedGun.GetCurrentAmmo() <= 0)
        {
            print("Out of Ammo!");
            SetPlayerGun("Pistol");
        }
    }

    public void AddMoney(int addMoney)
    {
        playerMoney += addMoney;
        uiMoney.GetComponent<UITextPop>().PopText();
    }

    public Gun GetPlayerGun()
    {
        return equippedGun;
    }

    public void SetPlayerGun(string gunName)
    {
        gunUI.SetGun(gunName);

        foreach(var gun in _allGuns)
        {
            gun.gameObject.SetActive(false);
            
            if (gun.name == gunName)
            {
                gun.gameObject.SetActive(true);
                equippedGun = gun;
            }
        }
    }

    void Aim()
    {
        var mousePos = Mouse.current.position.ReadValue();
        if (_directInputAim.magnitude <= 0 && !_lastPos.Equals(mousePos))
        {
            Cursor.SetCursor(cursorTexture, new Vector2(8, 8), CursorMode.ForceSoftware);
            Cursor.visible = true;
            Ray ray = _cam.ScreenPointToRay(mousePos);

            if (Physics.Raycast(ray, out var hit, float.MaxValue, _mousePlaneLayer))
            {
                var pos = new Vector3(hit.point.x, transform.position.y, hit.point.z);
                var dir = (pos - transform.position);
                targetRotation = Quaternion.LookRotation(dir);
                transform.eulerAngles = Vector3.up * Mathf.MoveTowardsAngle(transform.eulerAngles.y,
                    targetRotation.eulerAngles.y, rotationSpeed * Time.deltaTime);
            }
        }
        else if (_directInputAim.magnitude > 0)
        {
            Cursor.visible = false;
            Vector3 input = new Vector3(_directInputAim.x, 0, _directInputAim.y);
            if (input != Vector3.zero)
            {
                targetRotation = Quaternion.LookRotation(input);
                transform.eulerAngles = Vector3.up * Mathf.MoveTowardsAngle(transform.eulerAngles.y,
                    targetRotation.eulerAngles.y, rotationSpeed * Time.deltaTime);
            }
        }

        _lastPos = Mouse.current.position.ReadValue();
    }

    void Move()
    {
        Vector3 input = new Vector3(_directInputMove.x, 0, _directInputMove.y);
        var forward = _cam.transform.forward;
        var right = _cam.transform.right;

        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        var desiredMoveDirection = right * input.x + forward * input.z;

        Vector3 move = desiredMoveDirection;
        move *= (Mathf.Abs(input.x) == 1 && Mathf.Abs(input.z) == 1) ? .7f : 1;
        move *= walkSpeed;
        move += Vector3.up * -8;

        _controller.Move(move * Time.deltaTime);
    }
}