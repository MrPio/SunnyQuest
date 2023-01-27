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
    private TextMeshProUGUI _coinCounter;

    private AudioSource _audioSource;
    private readonly InventoryManager _inventoryManager = InventoryManager.getInstance;

    private void Start()
    {
        _audioSource = CamManager.mainCam.GetComponent<AudioSource>();
        _coinCounter = GameObject.FindWithTag("CoinCounter").GetComponent<TextMeshProUGUI>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Pacman"))
        {
            _audioSource.PlayOneShot(AudioClips[Random.Range(0, AudioClips.Count)]);
            Destroy(gameObject);
            _inventoryManager.Coins += 1;
            _coinCounter.text = _inventoryManager.Coins.ToString();
        }
    }
}