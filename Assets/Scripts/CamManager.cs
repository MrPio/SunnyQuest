using DefaultNamespace;
using Unity.Mathematics;
using UnityEngine;

public class CamManager : MonoBehaviour
{
    public static Camera mainCam;
    public static AudioSource AudioSource;
    public static float camHeight;
    public static float camWidth;

    public Transform targetToFollow;
    public float smoothSpeed = 0.125f;
    [SerializeField] private Transform _hills;
    [SerializeField] private float _parallaxFactor = 0.5f;
    private readonly InventoryManager _inventoryManager = InventoryManager.GetInstance;

    private float _hillsWidth;
    private float _hillOffset;

    void Awake()
    {
        mainCam = Camera.main;
        AudioSource = mainCam.GetComponent<AudioSource>();
        camHeight = mainCam.orthographicSize * 2;
        camWidth = camHeight * mainCam.aspect;
        _hillsWidth = _hills.GetComponent<SpriteRenderer>().sprite.bounds.size.x;
        _hills.position = new Vector2(transform.position.x - camWidth / 2f, _hills.position.y);
    }

    private void FixedUpdate()
    {
        // CAM MOVEMENT
        var levelSize = _inventoryManager.LevelsSize[_inventoryManager.CurrentLevel - 1];
        var targetPos = targetToFollow.position;
        var newPos = new Vector2(
            x: math.max(_inventoryManager.VisitedLevels, targetPos.x),
            y: math.max(0, math.min(levelSize.y - camHeight, targetPos.y))
        );
        var smoothPos = Vector2.Lerp(transform.position, newPos, smoothSpeed);
        transform.position = new Vector3(smoothPos.x, smoothPos.y, -10);

        // HILLS MOVEMENT
        _hills.position = new Vector2(smoothPos.x * _parallaxFactor - camWidth / 2 + _hillOffset, _hills.position.y);
        if (transform.position.x - _hills.transform.position.x - camWidth / 2 >= _hillsWidth / 2f + 1f)
        {
            _hillOffset += _hillsWidth / 2;
            _hills.position = new Vector2(_hills.position.x + _hillsWidth / 2f, _hills.position.y);
        }
        else if (_hills.transform.position.x >= transform.position.x - camWidth / 2)
        {
            _hillOffset -= _hillsWidth / 2;
            _hills.position = new Vector2(_hills.position.x - _hillsWidth / 2f, _hills.position.y);
        }
    }
}