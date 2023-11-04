using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [Header("Player Speed's")]
    [SerializeField] float movementSpeed = 3f;
    [SerializeField] float jumpSpeed = 3f;
    [SerializeField] float climbSpeed = 3f;
    [SerializeField] float bumperForce = 30f;

    [Header("Weapons")]
    [SerializeField] GameObject arrow;
    [SerializeField] Transform bow;


    // Components
    Vector2 playerMoveInput;
    Rigidbody2D playerRB2D;
    Animator playerAnimator;
    CapsuleCollider2D playerBodyCollider;
    BoxCollider2D playerFeetCollider;

    // Objects
    Chest chest;

    float gravityScaleAtStart;
    bool playerHasVerticalSpeed;
    bool playerHasHorizontalSpeed;
    bool isAlive = true;

    //Initiate Components
    private void Awake()
    {
        playerRB2D = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
        playerBodyCollider = GetComponent<CapsuleCollider2D>();
        playerFeetCollider = GetComponent<BoxCollider2D>();
        chest = GetComponent<Chest>();
        bow = GetComponent<Transform>();
    }

    void Start()
    {
        gravityScaleAtStart = playerRB2D.gravityScale;
    }


    void Update()
    {
        if (!isAlive) { return; }

        Running();
        FlipPlayerSprite();
        ClimbLadder();
        Falling();
        Die();
    }

    private void LateUpdate()
    {

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Bumper"))
        {
            playerRB2D.AddForce(Vector2.up * bumperForce, ForceMode2D.Impulse);
        }
    }

    //Get position of player when moving
    void OnMove(InputValue value)
    {
        if (!isAlive) { return; }
        playerMoveInput = value.Get<Vector2>();
    }

    void OnAction(InputValue value)
    {
        // Do stuff with action button
    }

    void OnFire(InputValue value)
    {
        if (!isAlive) { return; }

        if (value.isPressed)
        {
            Instantiate(arrow, arrow.transform.position, Quaternion.identity);
        }
    }

    void OnJump(InputValue value)
    {
        if (!isAlive) { return; }
        if (!playerFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground"))) { return; }

        if (value.isPressed)
        {
            playerRB2D.velocity += new Vector2(0f, jumpSpeed);
            playerAnimator.SetTrigger("jumped");
        }
    }

    void Falling()
    {
        if (!playerFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground")) &&
        !playerBodyCollider.IsTouchingLayers(LayerMask.GetMask("Climbing")))
        {
            playerAnimator.SetBool("isFalling", true);
            return;
        }

        playerAnimator.SetBool("isFalling", false);
    }

    void Running()
    {
        //Add speed to player
        Vector2 playerVelocity = new Vector2(playerMoveInput.x * movementSpeed, playerRB2D.velocity.y);
        playerRB2D.velocity = playerVelocity;

        // Running Animation Reference
        if (!playerAnimator.GetBool("isFalling") && !playerAnimator.GetBool("isClimbing"))
        {
            playerHasHorizontalSpeed = Mathf.Abs(playerRB2D.velocity.x) > Mathf.Epsilon;
            playerAnimator.SetBool("isRunning", playerHasHorizontalSpeed);
        }
    }

    void ClimbLadder()
    {
        if (!playerBodyCollider.IsTouchingLayers(LayerMask.GetMask("Climbing")))
        {
            playerAnimator.SetBool("isClimbing", false);
            playerRB2D.gravityScale = gravityScaleAtStart;
            return;
        }

        Vector2 climbVelocity = new Vector2(playerRB2D.velocity.x, playerMoveInput.y * climbSpeed);
        playerRB2D.velocity = climbVelocity;
        playerRB2D.gravityScale = 0;


        playerHasVerticalSpeed = Mathf.Abs(playerRB2D.velocity.y) > Mathf.Epsilon;
        playerAnimator.SetBool("isClimbing", playerHasVerticalSpeed);
    }

    //Flip the player sprite with center pivot
    void FlipPlayerSprite()
    {
        playerHasHorizontalSpeed = Mathf.Abs(playerRB2D.velocity.x) > Mathf.Epsilon;

        if (playerHasHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(playerRB2D.velocity.x), 1f);
        }
    }

    void Die()
    {
        if (playerRB2D.IsTouchingLayers(LayerMask.GetMask("Monsters", "Hazards")))
        {
            playerAnimator.SetTrigger("isDead");
            playerRB2D.velocity += new Vector2(0f, jumpSpeed);
            isAlive = false;
            FindObjectOfType<GameController>().ProcessPlayerDeath();
        }
    }
}
