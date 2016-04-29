﻿using UnityEngine;
using System.Collections;
[RequireComponent (typeof(Controller2D))]

public class Player : MonoBehaviour {

    [System.Serializable]
    public class PlayerStats {
        public int health = 100;
    }

    public GameObject fireball;
    public Transform firePoint;
    public float fireballSpeed = 1;

    public PlayerStats playerStats = new PlayerStats();

    public Vector2 wallJumpClimb;
    public Vector2 wallJumpOff;
    public Vector2 wallLeap;

    public float maxJumpHeight = 12;
    public float minJumpHeight = 1;
    public float timeToJumpApex = .4f;
    float accelerationTimeAirbourne = .2f;
    float accelerationTimeGrounded = .1f;
    public float moveSpeed = 30;

    public float wallSpeedMax = 5;
    public float wallStickTime = .25f;
    float timeToWallUnstick;

    float gravity;
    float maxJumpVelocity;
    float minJumpVelocity;
    Vector3 velocity;
    float velocityXSmoothing;

    Controller2D controller;

    void Start()
    {
        controller = GetComponent<Controller2D>();

        gravity = -(2 * maxJumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
        minJumpVelocity = Mathf.Sqrt(Mathf.Abs(gravity) * minJumpHeight);
        print("Gravity: " + gravity + " Jump velocity: " + maxJumpVelocity);
    }

    void Update()
    {
        //CÓDIGO INÚTIL AQUI PRA QUANDO O PLAYER CAIR
        if (transform.position.y <= -40)
        {
            DamagePlayer(9999);
        }

        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        int wallDirX = (controller.collisions.left) ? -1 : 1;

        //Atirando Fireballs
        if (Input.GetButtonDown("Fire1"))
        {
            //fireball.transform.localScale.z = 1*Mathf.Sign(wallDirX);
            GameObject fireballClone = (GameObject) Instantiate(fireball, firePoint.position, firePoint.rotation);
            //fireballClone.transform.position = new Vector3(fireballClone.transform.position.x + fireballSpeed * Mathf.Sign(wallDirX), fireballClone.transform.position.y, fireballClone.transform.position.z);
            fireballClone.GetComponent<Fireball>().speed = fireballSpeed * Mathf.Sign(controller.collisions.faceDir);
        }

        float targetVelocityX = input.x * moveSpeed;
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirbourne);

        bool wallSliding = false;
        if ((controller.collisions.left || controller.collisions.right) && !controller.collisions.below && velocity.y < 0)
        {
            wallSliding = true;

            if (velocity.y <= -wallSpeedMax)
            {
                velocity.y = -wallSpeedMax;
            }
            if (timeToWallUnstick > 0)
            {
                velocityXSmoothing = 0;
                velocity.x = 0;
                if(input.x != wallDirX && input.x != 0)
                {
                    timeToWallUnstick -= Time.deltaTime;
                }
                else
                {
                    timeToWallUnstick = wallStickTime;
                }
            }
            else
            {
                timeToWallUnstick = wallStickTime;
            }
        }

        if (controller.collisions.above || controller.collisions.below)
        {
            velocity.y = 0;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (wallSliding)
            {
                if (wallDirX == input.x)
                {
                    velocity.x = -wallDirX * wallJumpClimb.x;
                    velocity.y = wallJumpClimb.y;
                }
                else if (input.x == 0)
                {
                    velocity.x = -wallDirX * wallJumpOff.x;
                    velocity.y = wallJumpOff.y;
                }
                else
                {
                    velocity.x = -wallDirX * wallLeap.x;
                    velocity.y = wallLeap.y;
                }
            }
            if (controller.collisions.below)
            {
                velocity.y = maxJumpVelocity;
            }
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            if (velocity.y > minJumpVelocity)
            {
                velocity.y = minJumpVelocity;
            }


        }
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    public void DamagePlayer(int damage)
    {
        playerStats.health -= damage;
        if (playerStats.health <= 0)
        {
            GameMaster.KillPlayer(this);
        }
    }
}
