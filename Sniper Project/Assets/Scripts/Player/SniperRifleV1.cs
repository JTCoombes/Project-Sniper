using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

public class SniperRifleV1 : MonoBehaviour
{
    /* 
     [Header("UI")]
     Public Text ClipAmmo;
     Public Text MaxAmmo;
     */

    [Header("Misc Elements")]
    public Animator Anim;
    public GameObject Bullet;
    public Transform Shootpoint;
    //public Transform Player;
    public CamController CC;
    public Camera Cam;
    public Camera ScopeCam;
    //public GameObject Scope;
    private Vector3 cursorPos;
    public LayerMask IgnoreRaycast;
    public Volume volume;

    [Space]
    public float shotSpeed;
    public float GravityForce;
    public float BulletLife;

    [Header("UI")]
    public PauseMenu pauseMenu; 

    [Space]

    [Header("Properties")]
    public float Damage;
    public float HeadShot_Damage;
    public float BodyShot_Damage;
    public float LimbShot_Damage;
    //public float shotforce = 10f;
    public float FireRate = 15f;
    //public float Range;
    private float NextTimeToFire = 0f;

    [Space]

    [Header("Ammo")]
    public int ClipSize;
    public int MaxClipSize;
    public int TotalAmmo;
    private int ammoused;
    private int RemainingAmmo;
    public bool IsReloading = false;
    public bool IsEmpty;

    [Header("Effects")]
    public ParticleSystem gunshot;
    //shothit
    //bullethole
    //bloodsplat

    [Header("ADS")]
    public bool isAiming;
    public float FovBase = 60f;
    public float FovAds = 30f;
    public float ScopeBase = 25f;
    public float scopeZoom;
    public float AdsSpeed;

    [Space]
    public bool CantShoot = true;
    public bool CanZoom;


    private void OnEnable()
    {
        CantShoot = false;
        IsReloading = false;
        isAiming = false;
        IsEmpty = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        //anim
        //hitsfx
        pauseMenu = GameObject.Find("Menu Manager").GetComponent<PauseMenu>();

        if (volume.profile.TryGet(out DepthOfField depthOfField))
        {
            depthOfField.active = false;
        }
        //clipammo
        //maxammo
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0) && Time.time >= NextTimeToFire && isAiming)
        {
            NextTimeToFire = Time.time + 1f / FireRate;
            //anim
            Shoot();
            //clip ammo
        }

        if (Input.GetKey(KeyCode.E) && isAiming)
        {
            CanZoom = true;
        }

        if (Input.GetKey(KeyCode.Q) && isAiming)
        {
            CanZoom = false;
        }

        if(TotalAmmo <= 0 && ClipSize <= 0)
        {
            IsReloading = false;
            Cam.fieldOfView = FovBase;
            isAiming = false;
            CantShoot = true;
            return;
        }

        if (IsReloading)
        {
            return;
        }

        if(ClipSize <= 0)
        {
            ClipSize = 0;
            //anim
            StartCoroutine(Reload());
            return;
        }

        if(Input.GetKey(KeyCode.R) && ClipSize < MaxClipSize)
        {
            //reload
        }

        if (Input.GetMouseButtonDown(1))
        {
            Anim.SetBool("Scope", true);
            isAiming = true;
          
        }
        else if (Input.GetMouseButtonUp(1))
        {
            Anim.SetBool("Scope", false);
            isAiming = false;
            
        }

        if (isAiming)
        {
            Cam.fieldOfView = Mathf.Lerp(Cam.fieldOfView, FovAds, AdsSpeed * Time.deltaTime);
        }
        else if (!isAiming)
        {
            Cam.fieldOfView = Mathf.Lerp(Cam.fieldOfView, FovBase, AdsSpeed * Time.deltaTime);
            CanZoom = false;
        }

        if (CanZoom)
        {
            ScopeCam.fieldOfView = scopeZoom;
        }
        else if (!CanZoom)
        {
            ScopeCam.fieldOfView = ScopeBase;
        }

        if (pauseMenu.IsPaused)
        {
            CantShoot = true;
        }
        else if (!pauseMenu.IsPaused)
        {
            CantShoot = false;
        }
    }

    public void Shoot()
    {
        ClipSize--;
        ammoused++;

        //bullet
        GameObject BulletOBJ = Instantiate(Bullet, Shootpoint.position, Shootpoint.rotation);
        Bullet BulletScript = BulletOBJ.GetComponent<Bullet>();

        if (BulletScript)
        {
            BulletScript.Initilize(Shootpoint, shotSpeed, GravityForce);
        }
        Destroy(BulletOBJ, BulletLife);
                
    }

    public IEnumerator Reload()
    {
        isAiming = false;
        CanZoom = false;
        CantShoot = true;
        IsReloading = true;
        RemainingAmmo = 0;
        ammoused = 0;

        yield return new WaitForSeconds(1f);

        ClipSize = MaxClipSize;

        yield return new WaitForSeconds(.5f);

        CantShoot = false;
        IsReloading = false;
    }

    public void Activate()
    {
        if (volume.profile.TryGet(out DepthOfField depthOfField))
        {
            depthOfField.active = true;
        }
    }

    public void Deactivate()
    {
        if (volume.profile.TryGet(out DepthOfField depthOfField))
        {
            depthOfField.active = false;
        }
    }
}
