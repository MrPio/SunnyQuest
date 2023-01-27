using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Cloud : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private SpriteRenderer _sr;
    [SerializeField] private List<Sprite> _clouds;
    [SerializeField] private float _moveSpeed;

    private float _elapsed = 0f;

    private void Start()
    {
        var heightFactor = (transform.position.y + CamManager.camHeight * 0.15f) / (CamManager.camHeight * 0.7f);
        var speed = _moveSpeed * (2f - heightFactor*0.75f);
        _sr.sprite = _clouds[Random.Range(0, _clouds.Count)];
        _rb.velocity = Vector2.left * speed;
    }

    private void FixedUpdate()
    {
        _elapsed += Time.fixedDeltaTime;
        if (_elapsed > 1)
        {
            _elapsed = 0;
            if (transform.position.x + _sr.sprite.bounds.size.x <
                CamManager.mainCam.transform.position.x - CamManager.camWidth / 2f)
                Destroy(gameObject);
        }
    }
}