using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

namespace CyberMonk.Game.Lights
{
    /// <summary>
    /// The moonkey spotlights.
    /// </summary>
    [RequireComponent(typeof(Light2D))]
    public class MoonkeySpotlight : MonoBehaviour
    {
        #region fields

        [SerializeField]
        private GameObject target;
        [SerializeField]
        private float distanceOffset;

        private Light2D _light;

        #endregion

        #region methods

        /// <summary>
        /// Called to start the moonkey.
        /// </summary>
        private void Start()
        {
            this._light = this.GetComponent<Light2D>();
        }

        /// <summary>
        /// Update is called once per frame.
        /// </summary>
        private void Update()
        {
            if (this.target == null)
            {
                return;
            }

            Vector3 ourPosition = this.transform.position;
            Vector3 targetPosition = this.target.transform.position;

            Vector2 direction = new Vector2(targetPosition.x - ourPosition.x, targetPosition.y - ourPosition.y);
            float rotation = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;
            this.transform.eulerAngles = new Vector3(0, 0, rotation);

            // Updates the radius.
            float distance = Vector2.Distance(ourPosition, targetPosition);
            this._light.pointLightOuterRadius = distance + this.distanceOffset;
        }

        #endregion
    }
}

