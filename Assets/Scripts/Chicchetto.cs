using System;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using Random = UnityEngine.Random;

public class Chicchetto : MonoBehaviour
{
    [SerializeField] private new Rigidbody2D rigidbody2D;
    [SerializeField] private GameObject petard;
    [SerializeField] public float speed = 4.5f;
    [SerializeField] private int maxDrops = 2;
    [SerializeField] private SpriteRenderer spriteRenderer;
    private GameObject _pacman;
    private int _drops;
    private float _dropDistance;
    private int _currentDropped;
    private readonly InventoryManager _inventoryManager = InventoryManager.GetInstance;

    private static Camera MainCam => CamManager.mainCam;

    private void Start()
    {
        _pacman = GameObject.FindWithTag("Pacman");
        var levelSize = _inventoryManager.LevelsSize[_inventoryManager.CurrentLevel];

        var xPos = MainCam.transform.position.x + CamManager.camWidth * 0.6f;
        var yPos = Random.Range(
            minInclusive: levelSize.y - CamManager.camHeight * 0.65f,
            maxInclusive: levelSize.y - CamManager.camHeight * 0.55f
        );
        transform.position = new Vector2(xPos, yPos);
        rigidbody2D.velocity = Vector2.left * speed;

        _drops = Random.Range(1, maxDrops + 1);
        _dropDistance = Random.Range(0.5f, 1.2f);
    }

    private void FixedUpdate()
    {
        // DESTROY WHEN OUT OF THE MAP
        if (transform.position.x + spriteRenderer.sprite.bounds.size.x <
            CamManager.mainCam.transform.position.x - CamManager.camWidth / 2f)
            Destroy(gameObject);

        // DROP PETARD(S) WHEN ON PACMAN
        if (_currentDropped == _drops)
            return;
        var distance = transform.position.x - _pacman.transform.position.x;
        if (distance < _dropDistance - 1f * _currentDropped)
            Drop();
    }

    private void Drop()
    {
        Instantiate(
            original: petard,
            position: transform.position + Vector3.down *
            (spriteRenderer.sprite.bounds.size.y / 2f + 0.5f),
            rotation: Quaternion.Euler(0, 0, Random.Range(0, 360))
        );
        ++_currentDropped;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Pacman"))
            _pacman.GetComponent<Pacman>().Hit();
    }
}