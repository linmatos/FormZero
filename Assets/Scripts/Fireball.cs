﻿using UnityEngine;
using System.Collections;

public class Fireball : MonoBehaviour {

    public float timeLeft = 3.0f;
    private Rigidbody2D fireball;
    public float speed = 2;

	// Use this for initialization
	void Start () {
        fireball = GetComponent<Rigidbody2D>();
        Vector2 scale = transform.localScale;
        //Esse 10 tá aqui por causa da escala do prefab da fireball
        scale.x = 10*Mathf.Sign(speed);
        transform.localScale = scale;
        //fireball.velocity = transform.forward * velocity;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            other.SendMessage("ApplyDamage", Random.Range(18, 25));
            Debug.Log("Acertou o inimigo");
            Destroy(gameObject);
        }
    }

    void Update()
    {
        fireball.position = new Vector2(fireball.position.x + speed, fireball.position.y);
        //Destrói a fireball depois de 5 segundos
        timeLeft -= Time.deltaTime;
        if (timeLeft < 0)
        {
            Destroy(gameObject);
        }
    }
}
