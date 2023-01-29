using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class NewLevelHitbox : MonoBehaviour
{
    private Timer _timer;

    private void Start()
    {
        _timer = GameObject.FindWithTag("Timer").GetComponent<Timer>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Pacman"))
        {
            Destroy(gameObject);
            _timer.NewLevel();
            InventoryManager.GetInstance.CollectPoints(typeof(NewLevelHitbox));
        }
    }
}
