using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperRifleV1 : MonoBehaviour
{
    /* 
     [Header("UI")]
     Public Text ClipAmmo;
     Public Text MaxAmmo;
     */

    [Header("Misc Elements")]
    //public Animator Anim;
    public GameObject Bullet;
    public Transform Shootpoint;
    //public Transform Player;
    public CamController CC;
    public Camera Cam;
    //public GameObject Scope;
    private Vector3 cursorPos;
    public LayerMask IgnoreRaycast;

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
    public float AdsSpeed;

    [Space]
    public bool CantShoot = true;


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
        pauseMenu = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<PauseMenu>();

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
            //reload
            return;
        }

        if(Input.GetKey(KeyCode.R) && ClipSize < MaxClipSize)
        {
            //reload
        }

        if (Input.GetMouseButtonDown(1))
        {
            isAiming = true;
        }
        else if (Input.GetMouseButtonUp(1))
        {
            isAiming = false;
        }

        if (isAiming)
        {
            Cam.fieldOfView = Mathf.Lerp(Cam.fieldOfView, FovAds, AdsSpeed * Time.deltaTime);
        }
        else if (!isAiming)
        {
            Cam.fieldOfView = Mathf.Lerp(Cam.fieldOfView, FovBase, AdsSpeed * Time.deltaTime);
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
        GameObject bullet = Instantiate(Bullet, Shootpoint.position, Shootpoint.rotation);
                
    }
}
