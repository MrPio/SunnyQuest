using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using Unity.VisualScripting;
using UnityEngine;

public class Mercant : MonoBehaviour
{
    [SerializeField] private GameObject _messageBox;

    private Transform _canvas;
    private readonly InventoryManager _inventoryManager = InventoryManager.getInstance;


    private void Start()
    {
        _canvas = GameObject.FindWithTag("Canvas").transform;
    }

    void OnTriggerEnter2D()
    {
        _inventoryManager.MessageBox=Instantiate(_messageBox, _canvas);
        _inventoryManager.IsThereMessageBox = true;
        _inventoryManager.Shop = _inventoryManager.MessageBox.GetComponent<Shop>();
    }

    void OnTriggerExit2D()
    {
        Destroy(_inventoryManager.MessageBox);
        _inventoryManager.MessageBox = null;
        _inventoryManager.IsThereMessageBox = false;
    }
}