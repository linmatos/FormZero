using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

    public float health = 50;

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
	    if (health <= 0)
        {
            Destroy(gameObject);
        }
	}

    public void ApplyDamage(float damage)
    {
        health -= damage;
        Debug.Log("Causou " + damage + " de dano");
    }
}
