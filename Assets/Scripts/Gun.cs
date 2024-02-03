using System.Collections;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.VFX;
using Random = UnityEngine.Random;

public enum GunFireMode
{
    Auto,
    Single
}

public class Gun : MonoBehaviour
{
    public float rpm;
    public float damage;
    public int numberOfBullets = 1;
    public int numberOfShells = 1;
    public GunFireMode gunFireMode;
    public Transform shootPoint;
    public Transform shellEjectionPoint;
    public float ejectForce;
    public Rigidbody shell;
    public Bullet bullet;
    public float accuracyPenalty;
    public float bulletSpeed;

    public Light muzzleLight;

    public VisualEffect shootEffect;
    public AudioSource shootSound;

    private float _secondsBetweenShots;
    private float _nextPossibleShootTime;
    private bool _isshootSoundNotNull;

    public int maxAmmo = 100;
    private int currentAmmo;

    void Start()
    {
        _isshootSoundNotNull = shootSound != null;
        _secondsBetweenShots = 60 / rpm;
    }

    private void OnEnable()
    {
        currentAmmo = maxAmmo;
    }

    private void OnValidate()
    {
        _secondsBetweenShots = 60 / rpm;
    }

    public int GetCurrentAmmo()
    {
        return currentAmmo;
    }

    public void Shoot()
    {
        if (!CanShoot()) return;
        
        _nextPossibleShootTime = Time.time + _secondsBetweenShots;

        if (_isshootSoundNotNull)
        {
            shootSound.PlayOneShot(shootSound.clip);
        }

        for (int i = 0; i < numberOfShells; i++)
        {
            Rigidbody shellCase = Instantiate(shell, shellEjectionPoint.position, Quaternion.identity);
            shellCase.AddForce(shellEjectionPoint.forward * Random.Range(ejectForce * 0.5f, ejectForce * 1.5f) +
                               shootPoint.forward * Random.Range(-ejectForce * 0.1f, ejectForce * 0.1f));
        }
        
        
        for (int i = 0; i < numberOfBullets; i++)
        {
            var direction = shootPoint.forward;
            direction = Quaternion.AngleAxis(Random.Range(0, accuracyPenalty), Vector3.up) * direction;
            direction = Quaternion.AngleAxis(Random.Range(0, accuracyPenalty), Vector3.right) * direction;
            direction = Quaternion.AngleAxis(Random.Range(0, accuracyPenalty), Vector3.down) * direction;
            direction = Quaternion.AngleAxis(Random.Range(0, accuracyPenalty), Vector3.left) * direction;
            
            var nextBullet = Instantiate(bullet, shootPoint.position,
                Quaternion.LookRotation(direction.normalized, shootPoint.transform.up));
            var rb = nextBullet.GetComponent<Rigidbody>();
            
            rb.AddForce((direction * bulletSpeed), ForceMode.VelocityChange);
            nextBullet.Damage = damage;
            nextBullet.dir = direction;
        }

        currentAmmo -= 1;
     
        shootEffect.Play();

        muzzleLight.enabled = true;
        StartCoroutine(LightOffDelay(0.1f));
        
        Start();
    }
    
    IEnumerator LightOffDelay(float delaySeconds)
    {
        yield return new WaitForSeconds(delaySeconds);
        muzzleLight.enabled = false;
    }

    private bool CanShoot()
    {
        return Time.time >= _nextPossibleShootTime && currentAmmo > 0;
    }
}