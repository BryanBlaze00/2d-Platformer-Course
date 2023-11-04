using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    [SerializeField] int coinValue = 100;
    [SerializeField] AudioClip coinSFX;

    bool collected = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && !collected)
        {
            collected = true;
            AudioSource.PlayClipAtPoint(coinSFX, Camera.main.transform.position);
            FindObjectOfType<GameController>().ProcessGainPoints(coinValue);
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }
}
