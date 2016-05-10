using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Enemy : MonoBehaviour {

    float health;
    public float maxHealth = 50;
    public Image healthBar;
    public GameObject damageText;
    public Canvas canvas;

	// Use this for initialization
	void Start () {
        health = maxHealth;
        damageText.GetComponent<Text>().text = "";
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
        int critical = Random.Range(1, 15);
        if (critical == 1)
        {
            damage *= 2;
            Debug.Log("CRÍTICO!");
        }
        health -= damage;
        Debug.Log("Causou " + damage + " de dano");
        healthBar.fillAmount -= damage / maxHealth;
        GameObject dt = Instantiate(damageText, canvas.transform.position, canvas.transform.rotation) as GameObject;
        dt.GetComponent<Text>().text = damage.ToString();
        dt.transform.SetParent(canvas.transform);
        //damageText.GetComponent<Text>().text = damage.ToString();
        //damageText.GetComponent<Text>().color.a = //Alpha
    }
}
