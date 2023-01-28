using DefaultNamespace;
using UnityEngine;

public class CloudManager : MonoBehaviour
{
    [SerializeField] private GameObject _cloud;
    [SerializeField] private float _cloudsRate = 1f;
    [SerializeField] private Rigidbody2D _target;
    private float _nextCloud, _elapsed;
    private readonly InventoryManager _inventoryManager = InventoryManager.GetInstance;


    private void Start()
    {
        _nextCloud = Random.Range(1 / _cloudsRate * 0.8f, 1 / _cloudsRate * 1.25f);
    }

    private void FixedUpdate()
    {
        _elapsed += Time.fixedDeltaTime;
        if (_elapsed >= _nextCloud)
        {
            _elapsed = 0;
            _nextCloud = Random.Range(1 / _cloudsRate * 0.8f, 1 / _cloudsRate * 1.25f);

            if (_target.velocity.x < 0.05 && Random.Range(0, 3) != 0)
            {
                return;
            }

            var levelSize = _inventoryManager.LevelsSize[_inventoryManager.CurrentLevel];

            var cloud = Instantiate(_cloud, transform);
            var xPos = CamManager.mainCam.transform.position.x + CamManager.camWidth * 0.65f;
            var yPos = Random.Range(
                minInclusive: -CamManager.camHeight * 0.15f,
                maxInclusive: levelSize.y - CamManager.camHeight * 0.45f
            );
            cloud.transform.position = new Vector3(xPos, yPos);
        }
    }
}