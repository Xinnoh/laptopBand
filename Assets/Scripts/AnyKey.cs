using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AnyKey : MonoBehaviour
{
    public MoveUp transition;

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKey)
        {
            transition.moveUp = true;
            StartCoroutine(loadSceneDelay(0.2f));
        }
    }

    IEnumerator loadSceneDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(1);
    }
}
