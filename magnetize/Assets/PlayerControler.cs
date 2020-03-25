using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControler : MonoBehaviour
{

    private Rigidbody2D rb2D;
    public float moveSpeed = 5f;
    public float pullForce = 100f;
    public float rotateSpeed = 360f;
    private GameObject closestTower;
    private GameObject hookedTower;
    private bool isPulled = false;
    

    // Start is called before the first frame update
    void Start()
    {
        rb2D = this.gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        rb2D.velocity = -transform.up * moveSpeed;
        if(Input.GetKey(KeyCode.Z)&& !isPulled)
        {
            if(closestTower != null && hookedTower == null)
            {
                hookedTower = closestTower;
            }
            if(hookedTower)
            {
                float distance = Vector2.Distance(transform.position, hookedTower.transform.position);
                Vector3 pullDirection = (hookedTower.transform.position - transform.position).normalized;
                float newPullforce = Mathf.Clamp(pullForce / distance, 20, 50);
                rb2D.AddForce(pullDirection * newPullforce);
                rb2D.angularVelocity = -rotateSpeed / distance;
                isPulled = true;
            }
        }
        if (Input.GetKeyUp(KeyCode.Z))
        {
            isPulled = false;
        }
    }
    public void OnCollisionEnter2D(Collision2D collision) {
        if(collision.gameObject.tag == "Wall")
        {
            this.gameObject.SetActive(false);
        }
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Goal")
        {
            Debug.Log("Levelclear!");
        }
    }

    public void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Tower")
        {
            closestTower = collision.gameObject;
            collision.gameObject.GetComponent<SpriteRenderer>().color = Color.green;
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (isPulled) return;
        if (collision.gameObject.tag == "Tower")
        {
            closestTower = null;
            collision.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
        }
    }
}
