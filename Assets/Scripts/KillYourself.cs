using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class KillYourself : MonoBehaviour
{
    public float delay;
    void Start()
    {
        Destroy(gameObject, delay);
    }
}
