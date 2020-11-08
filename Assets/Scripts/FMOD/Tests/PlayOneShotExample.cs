using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayOneShotExample : MonoBehaviour
{
  

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space)){
            FMODUnity.RuntimeManager.PlayOneShot("event:/Music");
        }
    }
}
