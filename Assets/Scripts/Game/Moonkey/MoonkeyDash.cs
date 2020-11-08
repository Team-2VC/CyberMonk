using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoonkeyDash : MonoBehaviour
{

    private Vector2 direction;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        direction = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        
    }
}
