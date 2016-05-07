using UnityEngine;
using System.Collections;
[RequireComponent (typeof(Controller2D))]

public class Player : MonoBehaviour {

    [System.Serializable]
    public class PlayerStats {
        public int health = 100;
    }

    public GameObject barrell;
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
            Debug.Log("Morreu!");
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

        // ------- Código base do sliding -------
        /*
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            float minimum = 10;
            float maximum = 25;
            transform.position = new Vector3(Mathf.Lerp(minimum, maximum, Time.time), 0, 0);
        }
        */

        float targetVelocityX = input.x * moveSpeed;

        //Alterei aqui
        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            velocity.x = Mathf.SmoothDamp(velocity.x, (targetVelocityX * 20), ref velocityXSmoothing, (controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirbourne);
            controller.collisions.isDashing = true;

            //INATANCIAR A ARMADILHA
            //Cria um barril onde ele começa a dar o dash
            //GameObject barrellClone1 = (GameObject)Instantiate(barrell, firePoint.position, firePoint.rotation);
        }
        else
        {
            velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirbourne);
            controller.collisions.isDashing = false;
        }

        if (Input.GetKeyUp(KeyCode.LeftAlt))
        {
            /****************************************************************************************
            ===================== CÓDIGO DE SOLTAR O BOTÃO E ATIVAR A ARMADILHA =====================

            É o seguinte, a armadilha é instanciada lá em cima, quando começa o dash
            Aqui ela vai ser ativada
            Quando soltar o botão de criar armadilha (aqui no caso Alt Esquerdo), ela vai ser ativada
            O código seria algo assim:

            armadilha.activate(1);

            O método activate(int type) é público e o parâmetro passado fala qual armadilha vai ativar
            O tipo da armadilha já vai ser definido antes, quando o jogador der o dash

            O tamanho da armadilha ainda não vai ser mutável, mas pensarei nisso depois
            ******************************************************************************************/

            //Cria um barril onde ele solta o botão de dash
            //GameObject barrellClone2 = (GameObject)Instantiate(barrell, firePoint.position, firePoint.rotation);
        }

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
