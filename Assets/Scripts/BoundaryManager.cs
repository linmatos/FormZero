using UnityEngine;
using System.Collections;

public class BoundaryManager : MonoBehaviour {

    private BoxCollider2D managerBox;               //This is the boxCollider of the BoundaryManager
    private Transform player;                       //This is rather obvious
    public GameObject boundary;                     //The real camera boundary which will be atcivated and deactivated

    void Start()
    {
        managerBox = GetComponent<BoxCollider2D>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        player = GetComponent<Transform>();
    }

    void Update()
    {
        ManageBoundary();
    }

    void ManageBoundary()
    {
        if (managerBox.bounds.min.x < player.position.x && player.position.x < managerBox.bounds.max.x &&
            managerBox.bounds.min.y < player.position.y && player.position.y < managerBox.bounds.max.y)
        {
            boundary.SetActive(true);
        }
        else
        {
            boundary.SetActive(false);
        }
    }
}
