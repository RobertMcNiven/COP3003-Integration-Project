using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class TestBase : MonoBehaviour
{
    // fields

    // base health
    [SerializeField]
    protected static int HEALTH_BASE = 100;
    [SerializeField]
    private int healthTotal = HEALTH_BASE;
    private int health;

    // passive modifiers
    [SerializeField]
    private float walkSpeedMultiplier = 1.0f; // how fast player should walk
    //[SerializeField]
    // private float runSpeedMultiplier = 1.2f; // runSpeed for when it's implemented
    [SerializeField]
    private float attackMod = 1.0f; // mod for physical attacks
    //[SerializeField]
    // private float magicMod = 1.0f; // mod for magic-based attacks
    [SerializeField]
    private float defenseMod = 1.0f; // physical defense modifier
    //[SerializeField]
    // private float defenseModMagic = 1.0f; // possible alt defmod for magic-property attacks
    //[SerializeField]
    //private float attackSpeed; // how long until player can attack again

    public int Health { get => health; set => health = value; }
    public float SpeedMultiplier { get => walkSpeedMultiplier; set => walkSpeedMultiplier = value; }
    public int HealthTotal { get => healthTotal; set => healthTotal = value; }
    public float DefenseMod { get => defenseMod; set => defenseMod = value; }

    [SerializeField]
    protected float movementSpeed;

    protected Vector2 movement;
    protected Rigidbody2D body;
    protected Animator anim;

    // Awake is called when Unity creates the object
    protected void baseAwake()
    {
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // FixedUpdate is called at a fixed interval, not always once per frame.
    protected void baseFixedUpdate()
    {
        RotateTowardDirection();
        Movement();
    }

    float GetMagnitude()
    {
        float velocityX = body.velocity.x * body.velocity.x;
        float velocityY = body.velocity.y * body.velocity.y;
        float magnitude = Mathf.Sqrt(velocityX + velocityY);
        return magnitude;
    }

    protected void Movement()
    {
        // get current position
        Vector2 currentPos = body.position;
        // calculate move delta
        Vector2 adjustedMovement = movement * movementSpeed;
        // add move delta to current position
        Vector2 newPos = currentPos + adjustedMovement * Time.fixedDeltaTime;
        // move player to new position
        body.MovePosition(newPos);
    }

    protected void RotateTowardDirection()
    {
        //turn off walking
        if (movement != Vector2.zero) // if we have player movement input
        {
            // rotate sprite to face direction of movement
            transform.rotation =
                Quaternion.LookRotation(Vector3.forward, movement);
            // turn on walking animation
            anim.SetBool("walking", true);
        }
        else
        {
            //turn off walking
            anim.SetBool("walking", false);
        }
    }

    public int DamageCalculate(int baseDamage)
    {
        int modifiedDamage = (int)(baseDamage * attackMod);
        return modifiedDamage;
    }

    public int HealthReduce(int damageValue)
    {
        //damageValue = (int)(damageValue * DefenseMod); // multiply damage value by player defense
        this.health -= damageValue; // subtract modified damage from health
        return damageValue; // return if print damage needed
    }

    public int HealthHeal(int healValue)
    {
        if (healValue < 1)
        {
            healValue = 1;
            Debug.Log("This healing item healed for less than 1 HP\n");
        }
        int healMax = healthTotal - health;
        if (healValue > healMax)
        {
            healValue = healMax;
        }
        this.health += healValue;
        return healValue; // return value if heal number needed
    }

    public int HealthHeal(int healValue, char healType)
    {
        switch (healType)
        {
            // choose small, medium, large, or x-large potion types.
            // default to 1 if no type or less than 1
            case 's':
                healValue = (int)(healValue * 0.7);
                break;
            case 'm':
                //healValue = (int)(healValue * 1.0);
                break;
            case 'l':
                healValue = (int)(healValue * 1.4);
                break;
            case 'x':
                healValue = (int)(healValue * 2.0);
                break;
            default:
                healValue = 1;
                Debug.Log("This healing item doesn't have a healType\n");
                break;
        }
        if (healValue < 1)
        {  // minimum healing value = 1
            healValue = 1;
            Debug.Log("This healing item healed for less than 1 HP\n");
        }

        int healMax = healthTotal - health;  //check to see if healValue is more than max health
        if (healValue > healMax)
        {
            healValue = healMax;  // set healValue to most health that can be healed
        }

        this.health += healValue; // heal the player
        return healValue; // return value if heal number needed
    }
}