using UnityEngine;

public enum GunFireMode
{
    Auto,
    Single
}

public class Gun : MonoBehaviour
{ 
    public float rpm;
    public GunFireMode gunFireMode;
    public Transform shootPoint;
    public Transform cartridgeEjectionPoint;
    public LineRenderer effect;
    public AudioSource shootSound;
    
    private float _secondsBetweenShots;
    private float _nextPossibleShootTime;
    
    void Start() 
    {
        _secondsBetweenShots = 60 / rpm;
        if(GetComponent<LineRenderer> ()) {

            effect = GetComponent<LineRenderer>();
        }
    }

    public void Shoot() 
    {
        if (CanShoot ()) 
        {
            
            _nextPossibleShootTime = Time.time + _secondsBetweenShots;

            GetComponent<AudioSource>().Play();

            // if(tracer)
            // {
            //     StartCoroutine("RenderTracer",ray.direction * shotDistance);
            // }
            //
            // Rigidbody newShell = Instantiate(shell,shellEjectionPoint.position,Quaternion.identity) as Rigidbody;
            // newShell.AddForce(shellEjectionPoint.forward * Random.Range (200f,250f) + spawn.forward * Random.Range(-15f,15));
        }

    }
    
    public void ShootContinous()
    {
        if (gunFireMode == GunFireMode.Auto) 
        {
            Shoot();
        }
    }

    private bool CanShoot()
    {
        bool canShoot = true;

        if(Time.time < _nextPossibleShootTime)
        {
            canShoot = false;
        }

        return canShoot;
    }
}
