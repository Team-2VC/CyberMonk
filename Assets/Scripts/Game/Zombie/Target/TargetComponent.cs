using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CyberMonk.Game.Zombie.Target
{

    [RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
    public class TargetComponent : MonoBehaviour
    {

        // TODO: Set the parent 

        /// <summary>
        /// Called when the mouse is over the target.
        /// </summary>
        private void OnMouseOver()
        {
            // TODO: Add abstraction.
            if(Input.GetKeyDown(KeyCode.Space))
            {
                this.gameObject.SetActive(false);
            }
        }
    }
}