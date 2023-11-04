using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : MonoBehaviour
{
    [SerializeField] float moveSpeed = 1f;

    //Components
    Rigidbody2D monsterRB2D;
    Animator monsterAnimator;
    CapsuleCollider2D monsterFeetCollider;

    //Initialize Components
    private void Awake()
    {
        monsterRB2D = GetComponent<Rigidbody2D>();
        monsterAnimator = GetComponent<Animator>();
        monsterFeetCollider = GetComponent<CapsuleCollider2D>();
    }

    void Start()
    {
        transform.localScale = new Vector3(-1, 1, 1);
    }


    void Update()
    {
        monsterRB2D.velocity = new Vector2(moveSpeed, 0f);
    }

    void OnTriggerEnter2D(Collider2D incoming)
    {
        string tag = incoming.gameObject.tag;

        switch (tag)
        {
            case "Monster":
                moveSpeed = -moveSpeed;
                FlipSprite();
                break;
            case "Arrow":
                monsterAnimator.SetTrigger("Killed");
                break;
        }
    }

    void OnTriggerExit2D(Collider2D incoming)
    {
        string tag = incoming.gameObject.tag;

        switch (tag)
        {
            case "Ground":
                moveSpeed = -moveSpeed;
                FlipSprite();
                break;
        }
    }

    void FlipSprite()
    {
        transform.localScale = new Vector2((Mathf.Sign(monsterRB2D.velocity.x)), 1f);
    }
}
