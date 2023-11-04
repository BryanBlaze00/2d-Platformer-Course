using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueTurtle : MonoBehaviour
{
    [SerializeField] float moveSpeed = 1f;
    [SerializeField] int killValue = 50;

    //Components
    Rigidbody2D monsterRB2D;
    Animator monsterAnimator;

    //Initialize Components
    private void Awake()
    {
        monsterRB2D = GetComponent<Rigidbody2D>();
        monsterAnimator = GetComponent<Animator>();
    }

    void Start()
    {
        transform.localScale = new Vector3(-1, 1, 1);
    }


    void Update()
    {
        monsterRB2D.velocity = new Vector2(moveSpeed, 0f);
    }

    void OnTriggerExit2D(Collider2D incoming)
    {
        string tag = incoming.tag;

        switch (tag)
        {
            case "Ground":
                moveSpeed = -moveSpeed;
                FlipSprite();
                break;
            case "Arrow":
                FindObjectOfType<GameController>().ProcessGainPoints(killValue);
                monsterAnimator.SetTrigger("Killed");
                break;
        }

        // if (incoming.gameObject.CompareTag("Ground"))
        // {
        //     moveSpeed = -moveSpeed;
        //     FlipSprite();
        // }
    }

    void FlipSprite()
    {
        transform.localScale = new Vector2((Mathf.Sign(monsterRB2D.velocity.x)), 1f);
    }

    // public void Killed()
    // {
    //     monsterAnimator.SetTrigger("Killed");
    //     Invoke("DestroyMonster()", 2f);
    // }

    // void DestroyMonster()
    // {
    //     Destroy(this.gameObject, 1f);
    // }
}
