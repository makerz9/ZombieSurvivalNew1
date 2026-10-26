using System.Collections;
using UnityEngine;


public class Gun : MonoBehaviour
{
    
    //값 형식
    public enum State //열거는 이 데이터들을 한번에 선언가능
    {
        Ready, //기본값 = 맨 위에
        Empty,
        Reloading,
    }

    public State state {get; private set;}

    public Transform fireTransform;

    public ParticleSystem muzzleFlashEffect;

    public ParticleSystem shellEjectEffect;

    private LineRenderer bulletLineRenderer;

    private AudioSource gunAudioPlayer;

    public GunData gunData;

    private float fireDistance = 50f;

    public int ammoRemain;

    public int magAmmo;
    
    public int maxMagAmmo=25;


    private float lastFireTime; //연발 간격

    public bool isReloading = false;

    public UIManager uIManager;



    private void Awake()
    {
        bulletLineRenderer = GetComponent<LineRenderer>();
        gunAudioPlayer = GetComponent<AudioSource>();

        bulletLineRenderer.positionCount =2;
        bulletLineRenderer.enabled = false;


    }

    private void OnEnable()
    {
        ammoRemain = gunData.startAmmoRemain;
        magAmmo = gunData.magCapacity;
        uIManager.SetAmmoText(magAmmo, ammoRemain);

        state = State.Ready;
        lastFireTime = 0f;
        magAmmo = maxMagAmmo;
    }

    public void Fire()
    {
        if(state == State.Ready && Time.time >= lastFireTime + gunData.timeBetFire)
        {
            lastFireTime = Time.time;
            Shoot();
        }   
    }

    public LayerMask targetLayer;

    private void Shoot()
    {
        Vector3 hitPosition;

        Ray ray = new Ray(fireTransform.position, fireTransform.forward);
        if(Physics.Raycast(ray, out RaycastHit hit, fireDistance, targetLayer))
        {
            hitPosition = hit.point;
            Debug.Log(hit.collider. name);
            IDamageable target = hit.collider.GetComponent<IDamageable>();
            if (target != null)
            {
                target.OnDamage(gunData.damage, hit.point, hit.normal);
            }
        }
        else
        {
            hitPosition = fireTransform.position + fireTransform.forward * fireDistance;

        }

        StartCoroutine(ShootEffect(hitPosition));

        magAmmo--;
        uIManager.SetAmmoText(magAmmo, ammoRemain);
        if(magAmmo <= 0)
        {
            state = State.Empty;
        }
    }

    private IEnumerator ShootEffect(Vector3 hitPosition)
    {
        muzzleFlashEffect.Play();
        shellEjectEffect.Play();
        gunAudioPlayer.PlayOneShot(gunData.shotClip);

        bulletLineRenderer.SetPosition(0, fireTransform.position);
        bulletLineRenderer.SetPosition(1, hitPosition);
        bulletLineRenderer.enabled = true;

        yield return new WaitForSeconds(1f);

        bulletLineRenderer.enabled = false;
        Debug.Log("2");


    }

    private void Update()
    {
        // if(Input.GetButton("Fire1"))
        // {
        //     Fire();
        // }


    }

    public bool Reload()
    {
        StartCoroutine(ReloadRoutine());

        return false;
    }

    private IEnumerator ReloadRoutine()
    {
        
        isReloading = true;
        state = State.Reloading;
        gunAudioPlayer.PlayOneShot(gunData.reloadClip);


        Debug.Log(ammoRemain);
        Debug.Log(magAmmo);
        yield return new WaitForSeconds(gunData.reloadTime);
        

        
        // 25 - 0
        // 25 - 10 = 15개 충전
        // 최대 25개

        if(ammoRemain > 0)
        {
            int refillAmmo = maxMagAmmo - magAmmo;
            //
            // 15 / 5 라면 리필이 10개
            //  ||| 10재장전 > 5(남은거)
            // / 
            if(refillAmmo >= ammoRemain) //재장전이 남은거보다 큼 그럼 재장전량 - 남은거
            {
                // 10개에 4개 남아있으면 4개만 되야함
                //
                //int tempAmmo = refillAmmo;
                refillAmmo = ammoRemain;

            }
            Debug.Log("재장전완료");
            magAmmo += refillAmmo;
            ammoRemain -= refillAmmo;
            uIManager.SetAmmoText(magAmmo, ammoRemain); 

        }

        
        state = State.Ready;

        yield return new WaitForSeconds(0.5f);
        isReloading = false;
    }


}
