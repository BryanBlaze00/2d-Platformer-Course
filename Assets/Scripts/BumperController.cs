using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BumperController : MonoBehaviour
{
    Animator bumperAnimator;

    void Start()
    {
        bumperAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        bumperAnimator.SetTrigger("Bump");
    }

}
