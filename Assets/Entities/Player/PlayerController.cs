using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    public GameObject laserPrefab;
    public float speed;
    public float padding;
    public float projectileSpeed;
    public float firingRate;
    public float health;
    public AudioClip fireSound;
    public LevelManager levelManager;

    private ScoreKeeper scoreKeeper;

    float xmin;
    float xmax;



    // Use this for initialization
    void Start () {
        float distanceZ = transform.position.z - Camera.main.transform.position.z;
        Vector3 leftMost = Camera.main.ViewportToWorldPoint(new Vector3(0,0, distanceZ));
        Vector3 rightMost = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, distanceZ));
        xmin = leftMost.x + padding;
        xmax = rightMost.x - padding;
        scoreKeeper = GameObject.FindObjectOfType<ScoreKeeper>();
        levelManager = GameObject.FindObjectOfType<LevelManager>();
    }

    // Update is called once per frame
    void Update () {
        MoveShip();

        if (Input.GetKeyDown(KeyCode.Space))   { InvokeRepeating("Shoot", 0.00001f, firingRate); }
        if (Input.GetKeyUp(KeyCode.Space)) { CancelInvoke("Shoot"); }
	}

    void MoveShip()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            //transform.position += new Vector3(-speed * Time.deltaTime, 0, 0);
            transform.position += Vector3.left * speed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            //transform.position += new Vector3(speed * Time.deltaTime, 0, 0); 
            transform.position += Vector3.right * speed * Time.deltaTime;
        }

        float newX = Mathf.Clamp(transform.position.x, xmin, xmax);
        transform.position = new Vector3(newX, transform.position.y, transform.position.z);
    }

    void Shoot()
    {
        Vector3 laserSpawn = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
        GameObject laser = Instantiate(laserPrefab, laserSpawn, Quaternion.identity) as GameObject;
        laser.AddComponent<Rigidbody2D>();
        laser.GetComponent<Rigidbody2D>().velocity = new Vector3(0, projectileSpeed, 0);
        AudioSource.PlayClipAtPoint(fireSound, transform.position);
        
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        Projectile missile = col.gameObject.GetComponent<Projectile>();
        if (missile)
        {
            health -= missile.GetDamage();
            if (health <= 0)
            {
                Destroy(gameObject);
                levelManager.LoadLevel("End");
            }
            missile.Hit();
        }
    }
}
