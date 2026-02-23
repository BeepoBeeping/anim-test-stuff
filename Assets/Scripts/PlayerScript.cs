using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Animations;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.SceneManagement;


public enum States // used by all logic
{
    None,
    Idle,
    Walk,
    Jump,
    Dead,
    Dance,
};

public class PlayerScript : MonoBehaviour
{
    States state;

    public Animator anim;
    Rigidbody rb;
    public bool grounded;
    public FadeInScript fadeIn;
    public FadeOutScript fadeOut;

    public float waiting = 3f;
    public bool deathCooldown = true;
    public bool falling;
    InputAction moveAction;
    InputAction jumpAction;
    InputAction danceAction;
    InputAction menuAction;

    #region Start

    // Start is called before the first frame update
    void Start()
    {
        state = States.Idle;
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");
        danceAction = InputSystem.actions.FindAction("Dance");
        menuAction = InputSystem.actions.FindAction("Menu");
    }

    #endregion

    #region Update
    // Update is called once per frame
    void Update()
    {
        DoLogic();

 
        if (falling == true)
        {
            anim.SetBool("isFalling", true);
            anim.SetBool("isIdle", false);
            anim.SetBool("isWalk", false);
            anim.SetBool("isDance", false);
            anim.SetBool("isJump", false);
            rb.linearVelocity = new Vector3(0, -10f, 0);
        }


        if (menuAction.IsPressed())
        {
            SceneManager.LoadScene("Menu");
        }
    }

    #endregion

    #region Late Update
    private void LateUpdate()
    {
        grounded = false;
    }

    #endregion

    #region Logic

    void DoLogic()
    {
        if (state == States.Idle)
        {
            PlayerIdle();
        }

        if (state == States.Jump)
        {
            PlayerJumping();
        }

        if (state == States.Walk)
        {
            PlayerWalk();
        }

        if (state == States.Dead)
        {
            PlayerDead();
        }

        if (state == States.Dance)
        {
            PlayerDance();
        }
    }

    #endregion

    #region Player Idle
    void PlayerIdle()
    {
        anim.SetBool("isWalk", false);
        anim.SetBool("isDead", false);
        anim.SetBool("isIdle", true);

        deathCooldown = true;
        waiting = 3f;

        if (jumpAction.IsPressed())
        {
            anim.SetBool("isIdle", false);
            // simulate jump
            state = States.Jump;
            rb.linearVelocity = new Vector3(0, 4.5f, 0);
        }

        if (moveAction.IsPressed())
        {
            transform.Rotate(0, 0.5f, 0, Space.Self);

        }
        if (moveAction.IsPressed())
        {
            transform.Rotate(0, -0.5f, 0, Space.Self);
        }

        if (moveAction.IsPressed())
        {
            state = States.Walk;
        }     

        if (danceAction.IsPressed())
        {
            state = States.Dance;
        }
    }

    #endregion

    #region Player Jump
    void PlayerJumping()
    {
        Vector3 vel;
        anim.SetBool("isWalk", false);
        anim.SetBool("isDance", false);
        anim.SetBool("isIdle", false);
        anim.SetBool("isJump", true);

        // player is jumping, check for hitting the ground
        if (grounded == true)
        {
            //player has landed on floor
            anim.SetBool("isJump", false);
            state = States.Idle;

        }

        float magnitude = rb.linearVelocity.magnitude;

        if (moveAction.IsPressed())
        {
            vel = transform.forward * 3.65f;
        }
        else
        {
            vel = transform.forward * 0.5f;
        }

       



        rb.linearVelocity = new Vector3(vel.x, rb.linearVelocity.y, vel.z);

        if (magnitude <= 0.5f)
        {
            state = States.Idle;
        }
    }

    #endregion

    #region Player Walk
    void PlayerWalk()
    {
        Vector3 vel;
        anim.SetBool("isWalk", true);
        anim.SetBool("isIdle", false);
        anim.SetBool("isDance", false);
        anim.SetBool("isJump", false);

        //magnitude = the player's speed
        float magnitude = rb.linearVelocity.magnitude;

        //move forward and preserve original y velocity

        if (moveAction.IsPressed())
        {
            vel = transform.forward * 3.65f;
        }
        else
        {
            vel = transform.forward * 0.5f;
        }

        if (jumpAction.IsPressed())
        {
            anim.SetBool("isWalk", false);
            state = States.Jump;
            rb.linearVelocity = new Vector3(0, 4.5f, 0);
        }

        if (danceAction.IsPressed())
        {
            state = States.Dance;
        }

        rb.linearVelocity = new Vector3(vel.x, rb.linearVelocity.y, vel.z);

        if (magnitude <= 0.5f)
        {
            state = States.Idle;
        }

    }

    #endregion

    #region Player Dead
    void PlayerDead()
    {
        if (deathCooldown == true)
        {
            anim.SetBool("isFalling", false);
            anim.SetBool("isIdle", false);
            anim.SetBool("isWalk", false);
            anim.SetBool("isDead", true);
            anim.SetBool("isDance", false);
        }


        waiting -= Time.deltaTime;
        deathCooldown = false;


        if (waiting <= 0)
        {
            anim.SetBool("fadeIn", true);
            anim.SetBool("isDead", false);
            anim.SetBool("isIdle", true);
            state = States.Idle;
            transform.position = new Vector3(8f, 0.5f, 4f);
            rb.angularVelocity = Vector3.zero;
            print("reset transform");
        }


    }

    #endregion

    #region Player Dance
    void PlayerDance()
    {
        Vector3 vel;
        anim.SetBool("isDance", true);
        anim.SetBool("isWalk", false);
        anim.SetBool("isIdle", false);

        //magnitude = the player's speed
        float magnitude = rb.linearVelocity.magnitude;

        //move forward and preserve original y velocity

        if (moveAction.IsPressed())
        {
            anim.SetBool("isDance", false);
            anim.SetBool("isWalk", true);
            vel = transform.forward * 3.65f;
        }
        else
        {
            vel = transform.forward * 0f;
        }

        rb.linearVelocity = new Vector3(vel.x, rb.linearVelocity.y, vel.z);

        if (magnitude > 0.5f)
        {
            state = States.Walk;
        }
        
    }

    #endregion

    #region Collision

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Floor")
        {
            grounded = true;
            print("landed!");
        }

        if (col.gameObject.tag == "Enemy")
        {
            state = States.Dead;
        }

        if (col.gameObject.tag == "Death")
        {
            falling = false;
            anim.SetBool("isFalling", false);
            anim.SetBool("isDance", false);
            anim.SetBool("isDead", true);
            anim.SetBool("fadeOut", true);
            state = States.Dead;
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "FallTrig")
        {
            falling = true;
            anim.SetBool("isFalling", true);
            anim.SetBool("isDance", false);
        }
    }

    #endregion

    #region GUI Test
    //Output debug info to canvas
    private void OnGUI()
    {
        float mag = rb.linearVelocity.magnitude;

        mag = Mathf.Round(mag * 100) / 100;

        //debug text
        string text = "Left/Right arrows = Rotate\nSpace = Jump\nUp Arrow = Forward\nCurrent state=" + state;
        text += "\nmag=" + mag;

        // define debug text area
        GUILayout.BeginArea(new Rect(10f, 450f, 1600f, 1600f));
        GUILayout.Label($"<size=16>{text}</size>");
        GUILayout.EndArea();
    }

    #endregion
}