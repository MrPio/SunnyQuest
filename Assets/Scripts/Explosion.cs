using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] private AudioClip explosion;
    private void Start()
    {
        CamManager.AudioSource.PlayOneShot(explosion);
    }

    private void Destroy()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Pacman"))
        {
            col.gameObject.GetComponent<Pacman>().Hit();
            print("*** damage explosion ***");

        }
        else if (col.gameObject.CompareTag("Donkey"))
            col.gameObject.GetComponent<Donkey>().Hit();
    }
}