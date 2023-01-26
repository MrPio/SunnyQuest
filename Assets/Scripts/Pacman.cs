using System;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class Pacman : MonoBehaviour
{
    // public LayerMask platform;
    public Rigidbody2D rb;
    public BoxCollider2D bc;
    public SpriteRenderer sr;
    public float moveSpeed = 5.0f;
    public float jumpForce = 10.0f;
    public int jumpHoldLimit = 3;
    [SerializeField] private float _hopRate = 1;
    [SerializeField] private float _hopStrenght = 10;
    [SerializeField] private Animator _animator;
    [SerializeField] private int _maxHealth = 3;
    [SerializeField] private int _health = 3;
    [SerializeField] private GameObject _gameOverScreen;

    private Vector2 input;
    private bool inputJump;
    private int jumpCount = 1;
    private Bounds bounds;
    private int coins = 0;
    private float lastHop = 0;

    private void Start()
    {
        bounds = bc.bounds;
    }

    private void Update()
    {
        input.x = Input.GetAxisRaw("Horizontal");
        inputJump = Input.GetKey(KeyCode.Space);
        /*if (!inputJump && jumpCount != 1)
        {
            jumpCount = 1;
        }*/
    }

    private void FixedUpdate()
    {
        if (Math.Abs(input.x) > 0.05)
        {
            sr.flipX = input.x < 0;
            var movement = (Vector3)input * (moveSpeed * Time.fixedDeltaTime);
            transform.position += movement;

            lastHop += Time.fixedDeltaTime;
            if (Math.Abs(rb.velocity.y) < 0.05 && lastHop >= _hopRate)
            {
                lastHop = 0;
                rb.AddForce(Vector2.up * _hopStrenght);
            }
        }

        if (inputJump && jumpCount < jumpHoldLimit)
        {
            if (jumpCount == 1 && rb.velocity.y > 0.05)
                rb.velocity = new Vector2(rb.velocity.x, 0);
            ++jumpCount;
            rb.velocity += Vector2.up * jumpForce;
            // rb.AddForce(Vector2.up * jumpForce);
        }

        if (transform.position.y > CamManager.camHeight / 1.5f && rb.velocity.y > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, -2);
        }

        if (jumpCount != 1)
        {
            foreach (var tilemap in MapManager.levelTilemaps)
            {
                var southCheck = tilemap.WorldToCell(transform.position) + Vector3Int.down;
                var southTile = tilemap.GetTile(southCheck);
                // if (southTile is RuleTile ruleTile && ruleTile.m_DefaultColliderType != Tile.ColliderType.None &&
                // Math.Abs(rb.velocity.y) < 0.1f)
                if (Math.Abs(rb.velocity.y) < 0.1f && southTile != null)
                {
                    // print("onGround!");
                    jumpCount = 1;
                }
            }
        }

        if (transform.position.y < -CamManager.camHeight / 1.8f)
        {
            Hit(999);
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Ground"))
        {
            jumpCount = 1;
        }
    }

    public void Hit(int damage = 1)
    {
        _animator.SetTrigger("Hit");
        _health -= damage;
        if (_health <= 0)
        {
            Instantiate(_gameOverScreen, GameObject.FindWithTag("Canvas").transform);
            Destroy(gameObject);
        }

        for (var i = 1; i < _maxHealth + 1; i++)
        {
            GameObject.Find("Heart" + i).GetComponent<SpriteRenderer>().color = Color.white.WithAlpha(
                i <= _maxHealth - _health ? 0.35f : 1f);
        }
    }
}