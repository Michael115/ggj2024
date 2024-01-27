using System;
using UnityEngine;
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
    public GunFireMode gunFireMode;
    public Transform shootPoint;
    public Transform shellEjectionPoint;
    public float ejectForce;
    public Rigidbody shell;
    public Bullet bullet;
    public float accuracyPenalty;
    public float bulletSpeed;

    public VisualEffect shootEffect;
    public AudioSource shootSound;
    
    private float _secondsBetweenShots;
    private float _nextPossibleShootTime;
    private bool _isshootSoundNotNull;
    
    void Start()
    {
        _isshootSoundNotNull = shootSound != null;
        _secondsBetweenShots = 60 / rpm;
    }
    
    public void Shoot() 
    {
        if (CanShoot()) 
        {
            _nextPossibleShootTime = Time.time + _secondsBetweenShots;

            if (_isshootSoundNotNull)
            {
                shootSound.PlayOneShot(shootSound.clip);
            }
            
            Rigidbody shellCase = Instantiate(shell, shellEjectionPoint.position,Quaternion.identity);
            shellCase.AddForce(shellEjectionPoint.forward * Random.Range (ejectForce*0.5f, ejectForce*1.5f) + shootPoint.forward * Random.Range(-ejectForce*0.1f,ejectForce*0.1f));
            
            var direction = shootPoint.forward;
            direction = Quaternion.AngleAxis(Random.Range(0, accuracyPenalty), Vector3.up) * direction;
            direction = Quaternion.AngleAxis(Random.Range(0, accuracyPenalty), Vector3.right) * direction;
            direction = Quaternion.AngleAxis(Random.Range(0, accuracyPenalty), Vector3.down) * direction;
            direction = Quaternion.AngleAxis(Random.Range(0, accuracyPenalty), Vector3.left) * direction;
            
            var nextBullet = Instantiate(bullet, shootPoint.position, Quaternion.LookRotation(shootPoint.forward, shootPoint.transform.up));
            var rb = nextBullet.GetComponent<Rigidbody>();
            rb.AddForce((direction * bulletSpeed),  ForceMode.VelocityChange);
            nextBullet.damage = damage;

            shootEffect.Play();
        }
    }

    private bool CanShoot()
    {
        
        return !(Time.time < _nextPossibleShootTime);
    }
}
