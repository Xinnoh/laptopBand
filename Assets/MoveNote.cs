using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveNote : MonoBehaviour
{
    public float speed = 5f;
    public bool played = false;

    void Update()
    {
        // Move the object downwards
        transform.Translate(Vector3.down * speed * Time.deltaTime);

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("End"))
        {
            played = true;
            GetComponent<Renderer>().enabled = false; // Disable the renderer
            Debug.Log("d");
        }
        else
        {
            Debug.Log("e");
        }
    }
}
