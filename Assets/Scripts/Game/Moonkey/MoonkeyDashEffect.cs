using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CyberMonk.Game.Moonkey
{

    public struct DashEffectComponents
    {
        public GameObject gameObject;
        public SpriteRenderer renderer;
    }

    public class MoonkeyDashEffect
    {
        private MoonkeyComponent _component;
        private DashEffectComponents? _dashEffect = null;
        
        private float _currentDisplayTime, _maxDisplayTime;
        private float _startAlpha;

        public bool IsFinished
            => this.DisplayPercentage <= 0;

        protected float DisplayPercentage
            => this._currentDisplayTime / this._maxDisplayTime;

        public MoonkeyDashEffect(float displayTime, MoonkeyComponent component)
        {
            this._currentDisplayTime = this._maxDisplayTime = displayTime;
            this._startAlpha = component.Settings.DashEffectSettings.StartAlpha;
            this._component = component;

            this.SpawnObject();
        }

        private void SpawnObject()
        {
            MoonkeyGFXComponent gfxComponent = this._component.Properties.GraphicsComponent;

            GameObject dashObject = new GameObject("Dash_Effect");
            dashObject.transform.position = this._component.transform.position;

            SpriteRenderer renderer = dashObject.AddComponent<SpriteRenderer>();
            renderer.sprite = gfxComponent.SpriteRenderer.sprite;
            renderer.flipX = gfxComponent.SpriteRenderer.flipX;
            renderer.flipY = gfxComponent.SpriteRenderer.flipY;

            renderer.sortingLayerID = gfxComponent.SpriteRenderer.sortingLayerID;
            renderer.sortingOrder = gfxComponent.SpriteRenderer.sortingOrder - 1;
            renderer.renderingLayerMask = gfxComponent.SpriteRenderer.renderingLayerMask;

            DashEffectComponents effects = new DashEffectComponents();
            effects.gameObject = dashObject;
            effects.renderer = renderer;

            this._dashEffect = effects;
        }

        /// <summary>
        /// Updates the dash effect.
        /// </summary>
        public void Update()
        {
            if(!this._dashEffect.HasValue)
            {
                return;
            }

            DashEffectComponents components = this._dashEffect.Value;

            if(this._currentDisplayTime > 0)
            {
                this._currentDisplayTime -= Time.deltaTime;
                
                if (this._currentDisplayTime <= 0)
                {
                    this._currentDisplayTime = 0f;
                    
                    GameObject.Destroy(components.gameObject);
                    this._dashEffect = null;
                    return;
                }
            }
            
            float alphaPercentage = this.DisplayPercentage;
            Color color = components.renderer.color;
            color.a = Mathf.Lerp(0, this._startAlpha, alphaPercentage);
            components.renderer.color = color;
        }
    }
}

