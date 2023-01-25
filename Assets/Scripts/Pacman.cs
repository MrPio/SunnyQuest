using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Pacman : MonoBehaviour
{
    public LayerMask platform;
    public Rigidbody2D rb;
    public BoxCollider2D bc;
    public SpriteRenderer sr;
    public float moveSpeed = 5.0f;
    public float jumpForce = 10.0f;
    public int jumpHoldLimit = 3;

    private Vector2 input;
    private bool inputJump;
    private int jumpCount = 1;
    private Bounds bounds;
    private int coins = 0;

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
        // rb.MovePosition(rb.position + input * (moveSpeed * Time.fixedDeltaTime));
        if (Math.Abs(input.x) > 0.05)
        {
            sr.flipX = input.x < 0;
            transform.position += (Vector3)input * (moveSpeed * Time.fixedDeltaTime);
        }

        if (inputJump && jumpCount < jumpHoldLimit)
        {
            ++jumpCount;
            rb.velocity += Vector2.up * jumpForce;
            // rb.AddForce(Vector2.up * jumpForce);
        }

        if (jumpCount != 1)
        {
            foreach (var tilemap in MapManager.levelTilemaps)
            {
                var southCheck = tilemap.WorldToCell(transform.position) + Vector3Int.down;
                var southTile = tilemap.GetTile(southCheck);
                // if (southTile is RuleTile ruleTile && ruleTile.m_DefaultColliderType != Tile.ColliderType.None &&
                // Math.Abs(rb.velocity.y) < 0.1f)
                if (Math.Abs(rb.velocity.y) < 0.1f&&southTile != null)
                {
                    print("onGround!");
                    jumpCount = 1;
                }
            }
        }
    }

}