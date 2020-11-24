using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lookat : MonoBehaviour
{
    public GameObject target;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(target.transform, Vector2.up);
    }
}
