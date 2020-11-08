using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CyberMonk.Game.Moonkey
{
    /// <summary>
    /// Moonkey Settings
    /// </summary>
    [CreateAssetMenu(fileName = "Moonkey Settings", menuName = "Moonkey Settings")]
    public class MoonkeySettings : ScriptableObject
    {
        #region fields
        [SerializeField]
        private float speed;

        [SerializeField]
        private float gravityScale;

        [SerializeField]
        private float dashSpeed;

        [SerializeField]
        private float dashTime;

        #endregion

        #region properties

        public float Speed => this.speed;

        public float GravityScale => this.gravityScale;

        public float DashSpeed => this.dashSpeed;

        public float DashTime => this.dashTime;

        #endregion
    }
}
