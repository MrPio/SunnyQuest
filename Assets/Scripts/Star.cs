using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class Star : MonoBehaviour
{
    [SerializeField] private List<AudioClip> AudioClips;

    private AudioSource _audioSource;
    private readonly InventoryManager _inventoryManager = InventoryManager.GetInstance;

    private void Start()
    {
        _audioSource = CamManager.mainCam.GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Pacman"))
        {
            _audioSource.PlayOneShot(AudioClips[Random.Range(0, AudioClips.Count)]);
            Destroy(gameObject);
            Destroy(_inventoryManager.Gates[0]);
            _inventoryManager.CollectStar();
            GameObject.FindWithTag("Timer").GetComponent<Timer>().NewLevel();
            InventoryManager.GetInstance.CollectPoints(typeof(Star));
        }
    }
}