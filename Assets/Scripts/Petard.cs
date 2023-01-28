using UnityEngine;

public class Petard : MonoBehaviour
{
    [SerializeField] private new Rigidbody2D rigidbody2D;
    [SerializeField] private float maxTorque;
    [SerializeField] private float maxLateralForce;
    [SerializeField] private GameObject explosion;

    private void Start()
    {
        rigidbody2D.AddTorque(Random.Range(maxTorque / 3f, maxTorque) * (Random.Range(0, 2) * 2 - 1));
        rigidbody2D.AddForce(Vector2.right * Random.Range(-maxLateralForce, maxLateralForce));
    }

    private void Explode()
    {
        Instantiate(explosion, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}