using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField] float arrowSpeed = 1f;
    [SerializeField] float arrowArc = 1f;
    Rigidbody2D arrowRB;
    CapsuleCollider2D arrowCollider;

    float xSpeed;

    Vector2 startingPos;

    private void Awake()
    {
        arrowRB = GetComponent<Rigidbody2D>();
        arrowCollider = GetComponent<CapsuleCollider2D>();

        arrowCollider.enabled = true;
        arrowRB.isKinematic = false;
    }
    void Start()
    {
        if (this.name.Equals("Arrow(Clone)"))
        {
            xSpeed = GameObject.FindWithTag("Player").transform.localScale.x * arrowSpeed;
            transform.localScale = new Vector2(Mathf.Sign(xSpeed), 1f);
        }
        else
        {
            arrowCollider.enabled = false;
        }
    }

    void Update()
    {
        if (this.name.Equals("Arrow"))
        {
            arrowRB.isKinematic = true;
            return;
        }

        transform.localScale = new Vector3(.6f, .6f, .6f);
        arrowRB.velocity = new Vector2(xSpeed, arrowArc);
    }

    void OnTriggerEnter2D(Collider2D incoming)
    {
        string tag = incoming.gameObject.tag;
        switch (tag)
        {
            case "Monster":
                Destroy(incoming.gameObject, 2);
                break;
        }

        Destroy(gameObject, 2);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        Destroy(gameObject, 1);
    }
}

