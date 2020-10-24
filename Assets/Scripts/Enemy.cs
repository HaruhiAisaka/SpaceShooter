using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Firing Attributes")]
    [SerializeField] float shotCounter;
    [SerializeField] float minTimeBetweenShots = 0.2f;
    [SerializeField] float maxTimeBetweenShots = 3f;
    [SerializeField] GameObject laserPrefab;
    [SerializeField] float laserSpeed = 20f;
    [SerializeField] AudioClip fireSFX;
    [SerializeField] [Range(0,1)] float fireSFX_Volume;
    
    [Header("Health and Death")]
    [SerializeField] float health = 100;
    [SerializeField] GameObject explosionParticles;
    [SerializeField] float timeOfExplosion = 1f;
    [SerializeField] AudioClip deathSFX;
    [SerializeField] [Range(0,1)] float deathSoundVolume = 0.7f;

    [Header("Score")]
    [SerializeField] int scoreValue;
    // Start is called before the first frame update
    void Start(){
        shotCounter = Random.Range(minTimeBetweenShots,maxTimeBetweenShots);
    }

    // Update is called once per frame
    void Update(){
        CountDownAndShoot();
    }

    private void CountDownAndShoot(){
        shotCounter -= Time.deltaTime;
        if (shotCounter<= 0){Fire();}
    }

    private void Fire(){
        GameObject laser = Instantiate(
            laserPrefab,
            transform.position,
            Quaternion.identity) as GameObject;
        laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0,-laserSpeed);
        AudioSource.PlayClipAtPoint(fireSFX,Camera.main.transform.position, fireSFX_Volume);
        shotCounter = Random.Range(minTimeBetweenShots,maxTimeBetweenShots);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag != "EnemyLaser"){
            DamageDealer damageDealer = other.gameObject.GetComponent<DamageDealer>();
            if (!damageDealer) return;
            ProccessHit(damageDealer);
            damageDealer.Hit();
        }
    }

    private void ProccessHit(DamageDealer damageDealer){
        health -= damageDealer.GetDamage();
        if(health <= 0) {
            DestroyThis();
        }
    }

    private void DestroyThis(){
        FindObjectOfType<GameSession>().AddToScore(scoreValue);
        Destroy(gameObject);
        GameObject explosion = Instantiate(
                explosionParticles,
                transform.position,
                Quaternion.identity) as GameObject;
        Destroy(explosion, timeOfExplosion);
        AudioSource.PlayClipAtPoint(deathSFX, Camera.main.transform.position, deathSoundVolume);
    }
}
