using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 15f;
    public float lifetime = 3f;

    private Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.linearVelocity = -transform.right * speed;
        Destroy(gameObject, lifetime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }
}
