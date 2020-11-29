using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lookat : MonoBehaviour
{

    [SerializeField]
    private GameObject target;
    
    /// <summary>
    /// Update is called once per frame.
    /// </summary>
    private void Update()
    {
        if(this.target == null)
        {
            return;
        }

        Vector3 ourPosition = this.transform.position;
        Vector3 targetPosition = this.target.transform.position;

        Vector2 direction = new Vector2(targetPosition.x - ourPosition.x, targetPosition.y - ourPosition.y);
        float rotation = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;
        this.transform.eulerAngles = new Vector3(0, 0, rotation);
    }
}
