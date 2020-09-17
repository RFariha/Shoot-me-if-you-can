using System.Collections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {
	
	private Animator anim;
	private AudioSource _AudioSource;
	
	public float range = 100f;
	public int bulletsPerMag = 30;
	public int bulletsLeft=200;
	
	public int currentBullets;
		public int damage;
	
	public Transform shootPoint;
		public GameObject hitParticles;
		public GameObject bulletImpact;
		//public CollectablesManager ammobox;
	
	public ParticleSystem muzzleFlash;
	public AudioClip shootSound;
	public AudioClip reloadSound;
	
	public float fireRate = 0.1f;
	
	float fireTimer;
	
	private bool isReloading;
	
	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
		_AudioSource = GetComponent<AudioSource> ();
		
		currentBullets = bulletsPerMag;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButton ("Fire1")) {
			if (currentBullets > 0)
				Fire ();
			else if (bulletsLeft <= 0)
			return;
			else
				DoReload ();
		}
		
		if (Input.GetKeyDown (KeyCode.R)) {
			if (currentBullets < bulletsPerMag && bulletsLeft > 0)
			DoReload ();
		}
		
		if (fireTimer < fireRate)
			fireTimer += Time.deltaTime;
		
	//	AmmunitionManager.mag = currentBullets;
	//	AmmunitionManager.ammo = bulletsLeft;
	}
	
	void FixedUpdate()
	{
		AnimatorStateInfo info = anim.GetCurrentAnimatorStateInfo (0);
		
		isReloading = info.IsName ("Reload");
		if (info.IsName ("Fire"))
			anim.SetBool ("fire", false);
	}
	
	public void Fire()
	{
		if (fireTimer < fireRate || currentBullets <= 0||isReloading)
			return; 
		RaycastHit hit;
		if (Physics.Raycast (shootPoint.position, shootPoint.transform.forward, out hit, range)) {
			Debug.Log (hit.transform.name + " found!");
			if (hit.collider.tag == "enemy") {
				EnemyHealth enemyHealth = hit.collider.GetComponent <EnemyHealth> ();
				if (enemyHealth != null) {
				enemyHealth.TakeDamage (damage, hit.point);
				}
		}
	//	else if (hit.collider.tag == "collectable") {
	//		bulletsLeft += bulletsPerMag;
	//		Destroy (hit.collider.gameObject);
	//		ammobox.ammoboxcount--;
	//	}
		
		GameObject hitParticleEffect = Instantiate (hitParticles, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal));
		GameObject bulletHole = Instantiate (bulletImpact, hit.point, Quaternion.FromToRotation(Vector3.forward, hit.normal));
		
		Destroy (hitParticleEffect, 1f);
		Destroy (bulletHole, 2f);
		}
		anim.CrossFadeInFixedTime ("Fire", 0.1f);
		muzzleFlash.Play ();
		PlayShootSound ();
		
		
		currentBullets--;
		fireTimer = 0.0f;
	}
	
	public void Reload()
	{
		if (bulletsLeft <= 0)
			return;
		
		int bulletsToLoad = bulletsPerMag - currentBullets;
		int bulletsToDeduct = (bulletsLeft >= bulletsToLoad) ? bulletsToLoad : bulletsLeft;
		
		bulletsLeft -= bulletsToDeduct;
		currentBullets += bulletsToDeduct;
		
		PlayReloadSound ();
	}
	
	private void DoReload()
	{
		AnimatorStateInfo info = anim.GetCurrentAnimatorStateInfo (0);
		if (info.IsName ("Reload"))
			return;
		if (isReloading)
			return;
		Reload ();
		anim.CrossFadeInFixedTime ("Reload", 0.01f);
		anim.SetTrigger ("Reload"); 

	}
	
	private void PlayShootSound()
	{
		_AudioSource.clip = shootSound;
		_AudioSource.Play ();
	}

	private void PlayReloadSound()
	{
		_AudioSource.clip = reloadSound;
		_AudioSource.Play ();
	}
}
