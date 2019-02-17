using UnityEngine;

namespace TD.SceneController
{
    public class TimeChangeEntity
    {

        public Color lightColor;

        public Texture2D lightTexture;

        public TimeChangeEntity(Texture2D lightTexture, Color lightColor)
        {
            this.lightColor = lightColor;
            this.lightTexture = lightTexture;
        }
    }
}