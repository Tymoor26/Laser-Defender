using UnityEngine;
using System.Collections;

public class EnemyBehaviour : MonoBehaviour {
    public float health;
    public GameObject laserPrefab;
    public float projectileSpeed;
    public float shotsPerSecond;
    public int scoreValue = 150;
    public AudioClip fireSound;
    public AudioClip deathSound;

    private ScoreKeeper scoreKeeper;

    void Start()
    {
        scoreKeeper = GameObject.FindObjectOfType<ScoreKeeper>();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        Projectile missile = col.gameObject.GetComponent<Projectile>();
        if (missile)
        {
            health -= missile.GetDamage();     
            if (health <= 0) {
                scoreKeeper.Score(scoreValue);
                AudioSource.PlayClipAtPoint(deathSound, transform.position);
                Destroy(gameObject);
            }
            missile.Hit();
        }
    }

    void Update()
    {
        float probability = shotsPerSecond * Time.deltaTime;
        if(Random.value < probability) { Shoot(); }
    }

    void Shoot()
    {
        Vector3 laserSpawn = new Vector3(transform.position.x, transform.position.y - 0.3f, transform.position.z);
        GameObject laser = Instantiate(laserPrefab, laserSpawn, Quaternion.identity) as GameObject;
        laser.AddComponent<Rigidbody2D>();
        laser.GetComponent<Rigidbody2D>().velocity = new Vector3(0, -projectileSpeed, 0);
        AudioSource.PlayClipAtPoint(fireSound, transform.position);
    }
}
