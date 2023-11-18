using System;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class Pacman : MonoBehaviour
{
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
    [SerializeField] public int MaxHealth = 3;
    [SerializeField] public int Health = 3;
    [SerializeField] private GameObject _gameOverScreen;
    [SerializeField] private float _velocityYMax = 8f;
    [SerializeField] private float _invicibilityAfterHit = 3f;
    [SerializeField] private AudioClip jumpClip;
    [SerializeField] private List<AudioClip> hopClips;
    [SerializeField] private List<AudioClip> hitClips;
    [SerializeField] private List<AudioClip> callClips;

    public Vector2 input;
    public bool inputJump;
    private int jumpCount = 1;
    private Bounds bounds;
    private int coins = 0;
    private float lastHop = 0;
    private double _lastHit;
    private readonly InventoryManager _inventoryManager = InventoryManager.GetInstance;
    private Vector2 _backupPosition;
    private float _lastJump;
    private GameObject _pad, _jumpButton;

    private Dictionary<InventoryManager.Difficulty, float> _speedFactor =
        new()
        {
            { InventoryManager.Difficulty.Hard, 1.05f },
            { InventoryManager.Difficulty.Medium, 1.10f },
            { InventoryManager.Difficulty.Easy, 1.15f }
        };

    private Dictionary<InventoryManager.Difficulty, float> _jumpFactor =
        new()
        {
            { InventoryManager.Difficulty.Hard, 0.95f },
            { InventoryManager.Difficulty.Medium, 0.98f },
            { InventoryManager.Difficulty.Easy, 1.05f }
        };

    private void Start()
    {
        bounds = bc.bounds;
        _backupPosition = transform.position;
        if (Application.platform is RuntimePlatform.Android or RuntimePlatform.IPhonePlayer)
        {
            _pad = GameObject.FindWithTag("Pad");
            _jumpButton = GameObject.FindWithTag("JumpKey");
            print(_pad.transform.position);
        }
    }

    public void Initialize()
    {
        moveSpeed *= _speedFactor[_inventoryManager.GameDifficulty];
    }

    private void Update()
    {
        if (Application.platform != RuntimePlatform.Android && Application.platform != RuntimePlatform.IPhonePlayer)
        {
            input.x = Input.GetAxisRaw("Horizontal");
            input.y = Input.GetAxisRaw("Vertical");
            inputJump = Input.GetKey(KeyCode.Space);
        }

        else
            HandleTouches();

        if (Time.timeSinceLevelLoad - _inventoryManager.LastSpacebarMessageBox < 0.6f)
            inputJump = false;
    }

    private void HandleTouches()
    {
        for (var i = 0; i < Input.touchCount; i++)
        {
            var touch = Input.GetTouch(i);
            var touchPhase = touch.phase;
            if (touchPhase is TouchPhase.Began or TouchPhase.Ended or TouchPhase.Moved or TouchPhase.Canceled)
            {
                var down = touchPhase is TouchPhase.Began or TouchPhase.Moved ? 1 : 0;
                var position = CamManager.mainCam.ScreenToWorldPoint(touch.position);

                if (((Vector2)position - (Vector2)(_pad.transform.position)).sqrMagnitude < 18f)
                {
                    if (touchPhase == TouchPhase.Moved)
                        input = Vector2.zero;
                    var newPosition = (Vector2)position - (Vector2)(_pad.transform.position);
                    var sin = Vector2.Dot(Vector2.up, newPosition.normalized);
                    var cos = Vector2.Dot(Vector2.right, newPosition.normalized);
                    if (sin > 0.707f)
                        input.y = 1 * down;
                    else if (sin < -0.707f)
                        input.y = -1 * down;
                    else
                    {
                        if (cos > 0)
                            input.x = 1 * down;
                        else
                            input.x = -1 * down;
                    }
                }

                if (((Vector2)position - (Vector2)(_jumpButton.transform.position)).sqrMagnitude < 18f)
                {
                    inputJump = down == 1;
                }
            }
        }
    }

    private void FixedUpdate()
    {
        if (_inventoryManager.GameStarted)
            return;
        // SWITCHING COMMAND HANDLER IF THERE IS A MESSAGEBOX
        if (_inventoryManager.IsThereMessageBox)
        {
            _inventoryManager.HandleMessageBox(input.x, inputJump);
            return;
        }

        // ON LADDER MOVEMENT
        if (math.abs(input.y) > 0.05 && rb.velocity.y < input.y * ladderSpeed && isOnTile("Ladder"))
            rb.velocity = new Vector2(rb.velocity.x, input.y * ladderSpeed);

        // X MOVEMENT WITH HOPS
        if (input.sqrMagnitude > 0.05f)
        {
            sr.flipX = input.x < 0;
            var movement = new Vector3(input.x, 0) * (moveSpeed * Time.fixedDeltaTime);
            if ((transform.position + movement).x > _inventoryManager.VisitedLevels - CamManager.camWidth / 2f)
                transform.position += movement;

            lastHop += Time.fixedDeltaTime;
            if (Math.Abs(rb.velocity.y) < 0.05 && lastHop >= _hopRate)
            {
                lastHop = 0;
                rb.AddForce(Vector2.up * _hopStrenght + Vector2.right * rb.velocity);
                CamManager.AudioSource.PlayOneShot(hopClips[Random.Range(0, hopClips.Count)]);
            }
        }

        // VISITED LEVELS
        if ((int)_inventoryManager.VisitedLevels != (int)_inventoryManager.SpawnedLevelsTotalSize &&
            transform.position.x + CamManager.camWidth / 2 >= _inventoryManager.SpawnedLevelsTotalSize)
        {
            _inventoryManager.VisitedLevels = _inventoryManager.SpawnedLevelsTotalSize;
            ++_inventoryManager.CurrentLevel;
            GameObject.FindWithTag("LevelProgress").GetComponent<TextMeshProUGUI>().text =
                $"{_inventoryManager.CurrentLevel}/" +
                $"{_inventoryManager.LevelsSize.Count}";
        }

        // Y MOVEMENT, JUMP
        if (inputJump && jumpCount < jumpHoldLimit)
        {
            if (jumpCount == 1 && rb.velocity.y > 0.05)
                rb.velocity = new Vector2(rb.velocity.x, 0);
            if (jumpCount == 1 && Time.timeSinceLevelLoad - _lastJump > 0.4f)
            {
                CamManager.AudioSource.PlayOneShot(jumpClip);
                _lastJump = Time.timeSinceLevelLoad;
            }

            ++jumpCount;
            if (rb.velocity.y < jumpForce * jumpHoldLimit)
                rb.velocity += Vector2.up * (jumpForce * _jumpFactor[_inventoryManager.GameDifficulty]);
        }

        // PREVENT FROM GOING TOO HIGH
        if (transform.position.y > _inventoryManager.LevelsSize[_inventoryManager.CurrentLevel - 1].y -
            CamManager.camHeight / 2f + 2.2f && rb.velocity.y > 0)
        {
            // print("Too high!");  
            rb.velocity = new Vector2(rb.velocity.x, -2);
        }

        // RESTORE JUMP IF ON GROUND
        if (jumpCount != 1)
        {
            foreach (var tilemap in MapManager.levelTilemaps)
            {
                var southCheck = tilemap.WorldToCell(transform.position) + Vector3Int.down;
                var southTile = tilemap.GetTile(southCheck);

                // IF STANDING STILL ON TILE
                if (math.abs(input.y) < 0.05 && Math.Abs(rb.velocity.y) < 0.1f && southTile != null)
                {
                    jumpCount = 1;
                    _backupPosition = transform.position;
                }
                // IF ON LADDER
                else if (southTile is Tile && southTile.name == "Ladder")
                    jumpCount = 1;
            }
        }

        // FALLING OUT OF THE MAP TELEPORT THE PACMAN BACK
        if (transform.position.y < -CamManager.camHeight / 1.8f || isOnTile("Sea"))
        {
            Hit();
            print("*** damage out of map ***");

            transform.position = _backupPosition;
        }

        // CLAMP -Y SPEED
        var currentSpeed = rb.velocity;
        rb.velocity = new Vector2(currentSpeed.x, math.max(-_velocityYMax, currentSpeed.y));
    }

    private bool isOnTile(string tileName)
    {
        return MapManager.levelTilemaps.Select(tilemap => tilemap.GetTile(tilemap.WorldToCell(transform.position)))
            .Any(tile => tile is Tile or AnimatedTile && tile.name == tileName);
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
        if (damage < 999 && now - _lastHit < _invicibilityAfterHit)
            return;
        CamManager.AudioSource.PlayOneShot(hitClips[Random.Range(0, hitClips.Count)]);
        _lastHit = now;
        _animator.SetTrigger("Hit");
        Health -= damage;
        if (Health <= 0)
        {
            var gameOver = Instantiate(_gameOverScreen, GameObject.FindWithTag("Canvas").transform);
            gameOver.transform.Find("Points").GetComponent<TextMeshProUGUI>().text =
                $"You gained:" + Environment.NewLine + $"{_inventoryManager.Points} points!";
            Destroy(gameObject);
            if (Random.Range(0, 1f) <= 0.33f)
                GameObject.FindWithTag("AdMobManager").GetComponent<AdMobManager>().Show();
        }

        UpdateHeartsIcons();
    }

    public void Win()
    {
        var gameOver = Instantiate(_gameOverScreen, GameObject.FindWithTag("Canvas").transform);
        gameOver.transform.Find("Points").GetComponent<TextMeshProUGUI>().text =
            $"You gained:" + Environment.NewLine + $"{_inventoryManager.Points} points!";
        gameOver.transform.Find("Title").GetComponent<TextMeshProUGUI>().text =
            $"You Win!!!";
    }

    public void UpdateHeartsIcons()
    {
        for (var i = 1; i < MaxHealth + 1; i++)
        {
            GameObject.Find("Heart" + i).GetComponent<SpriteRenderer>().color =
                new Color(1f, 1f, 1f, i <= MaxHealth - Health ? 0.35f : 1f);
        }
    }
}