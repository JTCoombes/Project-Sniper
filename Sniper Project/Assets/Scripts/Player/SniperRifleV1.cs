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
    public GameObject BulletObject;
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
    public WindManager WindManager;
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
    public float reloadtime;

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
    public float AimDuration;
    public float AimStartTime;

    [Header("Recoil")]
    public float HorizontalRecoil;
    public float VerticalRecoil;
    public float HorizontalRecoil_base, VerticalRecoil_Base;
    public float HorizontalRecoil_Steady, VerticalRecoil_Steady;
    public float RecoilResetSpeed;
    [Space]
    public float Duration;
    public float Magnitude;
    public Camerashake camerashake;

    [Space]
    public bool CantShoot = true;
    public bool CanZoom;
    public bool Steady;



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

        VerticalRecoil = VerticalRecoil_Base;
        HorizontalRecoil = HorizontalRecoil_base;
        CC.RecoilResetSpeed = RecoilResetSpeed;
        pauseMenu = GameObject.Find("Menu Manager").GetComponent<PauseMenu>();
        camerashake = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camerashake>();

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

        if (Input.GetKeyDown(KeyCode.LeftShift) && isAiming)
        {
            
            Steady = true;
            //SteadyAim();
           
        }
        else if(Input.GetKeyUp(KeyCode.LeftShift) && isAiming)
        {
            Steady = false;
            //ResetAim();
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
            Cam.fieldOfView = Mathf.Lerp(Cam.fieldOfView, FovBase, AdsSpeed * Time.deltaTime);
            return;
        }

        if(ClipSize <= 0)
        {
            ClipSize = 0;
            Anim.SetBool("Scope", false);
            StartCoroutine(Reload());
            isAiming = false;
            return;
        }

        if(Input.GetKey(KeyCode.R) && ClipSize < MaxClipSize)
        {
            Anim.SetBool("Scope", false);
            StartCoroutine(ManualReload());
            isAiming = false;
            return;
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
            VerticalRecoil = VerticalRecoil_Base;
            HorizontalRecoil = HorizontalRecoil_base;
        }
        else if (!isAiming && !pauseMenu.IsPaused)
        {
            Cam.fieldOfView = Mathf.Lerp(Cam.fieldOfView, FovBase, AdsSpeed * Time.deltaTime);
            CanZoom = false;
            Time.timeScale = 1.0f;
        }


        if (Steady)
        {
            VerticalRecoil = VerticalRecoil_Steady;
            HorizontalRecoil = HorizontalRecoil_Steady;

            if (AimDuration <= 0)
            {
                Steady = false;
                Time.timeScale = 1f;
                AimDuration = 0f;
                //recoil();
            }
            else
            {
                Time.timeScale = .25f;
                Time.fixedDeltaTime = Time.timeScale * 0.2f;
                AimDuration -= Time.fixedDeltaTime;
            }
        }
        else
        {
            VerticalRecoil = VerticalRecoil_Base;
            HorizontalRecoil = HorizontalRecoil_base;

            if (AimDuration >= AimStartTime)
            {
                
                AimDuration = AimStartTime;
            }
            else
            {
                AimDuration += Time.fixedDeltaTime;
                Time.timeScale = 1f;
            }
            
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

        //recoil();

        //GameObject BulletOBJ = Instantiate(Bullet, Shootpoint.position, Shootpoint.rotation);
        GameObject BulletOBJ = ObjectPoolingManager.instance.SpawnFromPool("Bullet", Shootpoint.position, Shootpoint.rotation);
        Bullet BulletScript = BulletOBJ.GetComponent<Bullet>();

        if (BulletScript)
        {
            BulletScript.Initilize(Shootpoint, shotSpeed, GravityForce, WindManager.getWind());
        }
        //Destroy(BulletOBJ, BulletLife);
        BulletScript.StartCoroutine(BulletScript.Despawn(BulletOBJ, BulletLife));
    }

    void Despawn()
    {
        
    }

    
    public IEnumerator Reload()
    {
        CanZoom = false;
        CantShoot = true;
        IsReloading = true;
        isAiming = false;


        yield return new WaitForSeconds(reloadtime);

        yield return new WaitForSeconds(0.5f);

        RemainingAmmo = TotalAmmo - ammoused;
        TotalAmmo -= MaxClipSize;
        ClipSize = MaxClipSize;
        if(TotalAmmo <= 0)
        {
            MaxClipSize = 0;
            TotalAmmo = 0;
            ClipSize = RemainingAmmo + ClipSize;
        }
        // ui elements
        yield return new WaitForSeconds(.5f);

        CantShoot = false;
        IsReloading = false;
        ammoused = 0; 
    }

    public IEnumerator ManualReload()
    {
        CantShoot = true;
        IsReloading = true;
        isAiming = false;
        CanZoom = false;

        yield return new WaitForSeconds((MaxClipSize - ClipSize) - reloadtime);

        //anim
        yield return new WaitForSeconds(.25f);

        RemainingAmmo = TotalAmmo - ammoused;
        ClipSize = MaxClipSize;
        TotalAmmo -= ammoused;
        if(TotalAmmo >= 1)
        {

        }
        else if (TotalAmmo <= 0)
        {
            TotalAmmo = 0;
            ClipSize = RemainingAmmo + ClipSize;
            MaxClipSize = ClipSize;
        }
        //ui elements
        yield return new WaitForSeconds(.5f);
        //anim
        IsReloading = false;
        CantShoot = false;
        ammoused = 0;
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
    /*
    void recoil()
    {
        StartCoroutine(camerashake.Shake(Duration,Magnitude));
        CC.Recoil(VerticalRecoil, HorizontalRecoil);
    }
    */

    void SteadyAim()
    {
        if(AimDuration <= 0)
        {
            Steady = false;

            AimDuration = AimStartTime;
        }
        else
        {
            AimDuration -= Time.fixedDeltaTime;
        }
    }

    void ResetAim()
    {
        if (AimDuration >= AimStartTime)
        {

            AimDuration = AimStartTime;
        }
        else
        {
            AimDuration += Time.fixedDeltaTime;
        }
    }
}
