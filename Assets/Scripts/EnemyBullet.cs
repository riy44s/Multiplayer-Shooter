using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    private GameObject player;
    private Rigidbody rb;
    [SerializeField] private float force;
    private float timer;

    public static EnemyBullet instance;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            Vector3 direction = player.transform.position - transform.position;
            rb.velocity = direction.normalized * force;

            Quaternion rotation = Quaternion.LookRotation(direction);
            rb.MoveRotation(rotation);
        }
        else
        {
            Debug.LogError("Player not found!");
        }
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer > 10)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Health playerHealth = other.GetComponent<Health>();

            if (playerHealth != null)
            {
                playerHealth.TakeDamage(20);
            }

            Destroy(gameObject);
            Debug.Log("It worked");
        }
    }
}
