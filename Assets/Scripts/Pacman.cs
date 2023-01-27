using System;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class Pacman : MonoBehaviour
{
    // public LayerMask platform;
    public Rigidbody2D rb;
    public BoxCollider2D bc;
    public SpriteRenderer sr;
    public float moveSpeed = 5.0f;
    public float ladderSpeed = 8.0f;
    public float jumpForce = 10.0f;
    public int jumpHoldLimit = 3;
    [SerializeField] private float _hopRate = 1;
    [SerializeField] private float _hopStrenght = 10;
    [SerializeField] private Animator _animator;
    [SerializeField] private int _maxHealth = 3;
    [SerializeField] private int _health = 3;
    [SerializeField] private GameObject _gameOverScreen;
    [SerializeField] private float _velocityYMax = 8f;
    [SerializeField] private float _invicibilityAfterHit = 3f;

    private Vector2 input;
    private bool inputJump;
    private int jumpCount = 1;
    private Bounds bounds;
    private int coins = 0;
    private float lastHop = 0;
    private double _lastHit;

    private void Start()
    {
        bounds = bc.bounds;
    }

    private void Update()
    {
        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");
        inputJump = Input.GetKey(KeyCode.Space);
        /*if (!inputJump && jumpCount != 1)
        {
            jumpCount = 1;
        }*/
    }

    private void FixedUpdate()
    {
        // ON LADDER MOVEMENT
        if (math.abs(input.y) > 0.05 && rb.velocity.y < input.y * ladderSpeed && isOnLadder())
            rb.velocity = new Vector2(rb.velocity.x, input.y * ladderSpeed);

        // X MOVEMENT WITH HOPS
        if (input.sqrMagnitude > 0.05f)
        {
            sr.flipX = input.x < 0;
            var movement = new Vector3(input.x, 0) * (moveSpeed * Time.fixedDeltaTime);
            transform.position += movement;

            lastHop += Time.fixedDeltaTime;
            if (Math.Abs(rb.velocity.y) < 0.05 && lastHop >= _hopRate)
            {
                lastHop = 0;
                rb.AddForce(Vector2.up * _hopStrenght);
            }
        }

        // Y MOVEMENT, JUMP
        if (inputJump && jumpCount < jumpHoldLimit)
        {
            if (jumpCount == 1 && rb.velocity.y > 0.05)
                rb.velocity = new Vector2(rb.velocity.x, 0);
            ++jumpCount;
            rb.velocity += Vector2.up * jumpForce;
            // rb.AddForce(Vector2.up * jumpForce);
        }

        // PREVENT FROM GOING TOO HIGH
        if (transform.position.y > CamManager.camHeight / 1.2f && rb.velocity.y > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, -2);
        }

        // RESTORE JUMP IF ON GROUND
        if (jumpCount != 1)
        {
            foreach (var tilemap in MapManager.levelTilemaps)
            {
                var southCheck = tilemap.WorldToCell(transform.position) + Vector3Int.down;
                var southTile = tilemap.GetTile(southCheck);
                // if (southTile is RuleTile ruleTile && ruleTile.m_DefaultColliderType != Tile.ColliderType.None &&
                // Math.Abs(rb.velocity.y) < 0.1f)
                if ((math.abs(input.y) < 0.05 && Math.Abs(rb.velocity.y) < 0.1f && southTile != null) ||
                    (southTile is Tile && southTile.name == "Ladder"))
                {
                    jumpCount = 1;
                }
            }
        }

        // FALLING OUT OF THE MAP CAUSE GAME OVER
        if (transform.position.y < -CamManager.camHeight / 1.8f)
        {
            Hit(999);
        }

        // CLAMP -Y SPEED
        var currentSpeed = rb.velocity;
        rb.velocity = new Vector2(currentSpeed.x, math.max(-_velocityYMax, currentSpeed.y));
    }

    private bool isOnLadder()
    {
        foreach (var tilemap in MapManager.levelTilemaps)
        {
            var tile = tilemap.GetTile(tilemap.WorldToCell(transform.position));
            if (tile is Tile && tile.name == "Ladder")
            {
                return true;
            }
        }

        return false;
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
        var now = Time.timeSinceLevelLoadAsDouble;
        if (damage<999 && now - _lastHit < _invicibilityAfterHit)
            return;
        _lastHit = now;
        _animator.SetTrigger("Hit");
        _health -= damage;
        if (_health <= 0)
        {
            Instantiate(_gameOverScreen, GameObject.FindWithTag("Canvas").transform);
            Destroy(gameObject);
        }

        for (var i = 1; i < _maxHealth + 1; i++)
        {
            GameObject.Find("Heart" + i).GetComponent<SpriteRenderer>().color =
                new Color(1f, 1f, 1f, i <= _maxHealth - _health ? 0.35f : 1f);
        }
    }
}