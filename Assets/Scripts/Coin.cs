using System;
using DefaultNamespace;
using UnityEngine;

public class Coin : MonoBehaviour
{
    private readonly InventoryManager _inventoryManager = InventoryManager.getInstance;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Pacman"))
        {
            Destroy(gameObject);
            _inventoryManager.coins += 1;
        }    }
}