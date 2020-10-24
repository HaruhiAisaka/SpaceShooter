using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Player Movement")]
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float padding = 1f;

    [Header("Projectile")]
    [SerializeField] float laserSpeed = 20f;
    [SerializeField] float RPM = 120f;
    [SerializeField] GameObject laserPrefab;
    [SerializeField] AudioClip fireSFX;
    [SerializeField] [Range(0,1)] float fireSFX_Volume;
    Coroutine firing;
    int SECONDS_PER_MINUTE = 60;
    float xMin;
    float xMax;
    float yMin;
    float yMax;

    [Header("Health and Death")]
    [SerializeField] int health = 100;
    [SerializeField] AudioClip deathSFX;
    [SerializeField] [Range(0,1)] float deathSoundVolume = 0.7f;
    [SerializeField] GameObject explosionParticles;
    [SerializeField] float timeOfExplosion = 1f;
    
    
   
    // Start is called before the first frame update
    void Start()
    {
        SetUpMoveBoundaries();
    }

    private void SetUpMoveBoundaries(){
        Camera gameCamera = Camera.main;
        xMin = gameCamera.ViewportToWorldPoint(new Vector3(0,0,0)).x + padding;
        xMax = gameCamera.ViewportToWorldPoint(new Vector3(1,0,0)).x - padding;
        yMin = gameCamera.ViewportToWorldPoint(new Vector3(0,0,0)).y + padding;
        yMax = gameCamera.ViewportToWorldPoint(new Vector3(0,1,0)).y - padding;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Fire();
    }

    private void Move(){
        var deltaX = Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeed;
        var deltaY = Input.GetAxis("Vertical") * Time.deltaTime * moveSpeed;
        var newXPos = Mathf.Clamp(transform.position.x + deltaX, xMin,    xMax);
        var newYPos = Mathf.Clamp(transform.position.y + deltaY, yMin, yMax);
        transform.position = new Vector2(newXPos, newYPos);
    }
    
    private void Fire(){
        if (Input.GetButtonDown("Fire1")){
            firing = StartCoroutine(FireContinuously());
        }

        if (Input.GetButtonUp("Fire1")){
            StopCoroutine(firing);
        }
    }

    private IEnumerator FireContinuously(){
        while(true){
            GameObject laser = Instantiate(
            laserPrefab,
            transform.position,
            Quaternion.identity) as GameObject;
            laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0,   laserSpeed);
            AudioSource.PlayClipAtPoint(fireSFX,Camera.main.transform.position, fireSFX_Volume);
            yield return new WaitForSeconds(SECONDS_PER_MINUTE/RPM);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        DamageDealer damageDealer = other.gameObject.GetComponent<DamageDealer>();
        if (!damageDealer) return;
        ProccessHit(damageDealer);
        damageDealer.Hit();
        
    }

    private void ProccessHit(DamageDealer damageDealer){
        health -= damageDealer.GetDamage();
        if(health <= 0) {
            DestroyThis();
            
        }
    }

     private void DestroyThis(){
        FindObjectOfType<LevelLoading>().LoadGameOver();
        Destroy(gameObject);
        GameObject explosion = Instantiate(
                explosionParticles,
                transform.position,
                Quaternion.identity) as GameObject;
        Destroy(explosion, timeOfExplosion);
        AudioSource.PlayClipAtPoint(deathSFX, Camera.main.transform.position, deathSoundVolume);
    }
    
    public int GetHealth(){
        return health;
    }
}
