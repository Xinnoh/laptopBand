using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveUp : MonoBehaviour
{
    public bool moveUp;
    public float speed = 1f;

    // Update is called once per frame
    void Update()
    {
        if(moveUp)
        {
            Vector3 newPosition = transform.position;
            newPosition.y += speed;
            transform.position = newPosition;
        }
    }
}
