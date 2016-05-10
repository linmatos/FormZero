using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DestroyText : MonoBehaviour {

    public float timeLeft = 1.0f;
    public float velocity = 0.0f;
    public Text txt;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        Color cor = Color.white;
        cor.a = Mathf.SmoothDamp(0, 255, ref velocity, 2f);
        txt.color = cor;
        timeLeft -= Time.deltaTime;
        if (timeLeft < 0)
        {
            Destroy(gameObject);
        }
    }
}
