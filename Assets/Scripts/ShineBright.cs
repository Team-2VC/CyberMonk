using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShineBright : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if(Conductor.instance != null){
            Conductor.instance.downBeat += OnDownBeat;
        }
    }

    private void OnEnable() {
        if(Conductor.instance != null)
            Conductor.instance.downBeat += OnDownBeat;
    }

    private void OnDisable() {
        Conductor.instance.downBeat -= OnDownBeat;
    }

    void OnDownBeat(){
        Debug.Log("beat down");
    }

    
}
