using UnityEngine;
using System.Collections;

public class DamageZone : MonoBehaviour {

    public float damageRate = 1.0F;
    void OnCollisionEnter2D(Collision2D coll)
    {
        Debug.Log("Colidindo com " + coll.gameObject.tag);
        if (coll.gameObject.tag == "Player")
        {
            coll.gameObject.SendMessage("DamagePlayer", damageRate);
            Debug.Log("Causando " +damageRate + " de dano em " + coll.gameObject.tag);
        }
    }
}
