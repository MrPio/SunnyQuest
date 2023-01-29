using System;
using System.Collections.Generic;
using DefaultNamespace;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class Coin : MonoBehaviour
{
    [SerializeField] private List<AudioClip> AudioClips;
    [SerializeField] private int _coinValue;

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
            _inventoryManager.Coins += 1;
            _inventoryManager.CollectPoints(typeof(Coin));
            GameObject.FindWithTag("CoinCounter").GetComponent<TextMeshProUGUI>().text = _inventoryManager.Coins.ToString();
        }
    }
}