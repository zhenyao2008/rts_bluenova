using System.Collections.Generic;
using UnityEngine;

namespace TD.SceneController
{
    public class TimeChangeService
    {

        Material mLightingMat;

        Light mMainLight;

        List<TimeChangeEntity> mTimeAttributes;

        public void OnAwake()
        {
            mLightingMat = Resources.Load<Material>("town/td_blddeco_0801_parklight_m");

            mMainLight = GameObject.Find("Directional Light").GetComponent<Light>();

            Texture2D normalLightingTexture = Resources.Load<Texture2D>("town/td_blddeco_0801_parklight_t");

            Texture2D nightLightingTexture = Resources.Load<Texture2D>("town/td_blddeco_0801_parklight_night_t");

            Color color;

            mTimeAttributes = new List<TimeChangeEntity>();

            if (ColorUtility.TryParseHtmlString("#CFE2FA", out color))
            {
                mTimeAttributes.Add(new TimeChangeEntity(normalLightingTexture, color));
            }
            if (ColorUtility.TryParseHtmlString("#FFFFFF", out color))
            {
                mTimeAttributes.Add(new TimeChangeEntity(normalLightingTexture, color));
            }
            if (ColorUtility.TryParseHtmlString("#FFAC6D", out color))
            {
                mTimeAttributes.Add(new TimeChangeEntity(nightLightingTexture, color));
            }
            if (ColorUtility.TryParseHtmlString("#5F96F8", out color))
            {
                mTimeAttributes.Add(new TimeChangeEntity(nightLightingTexture, color));
            }

            BlueNoah.Event.EasyInput.Instance.AddListener(BlueNoah.Event.TouchType.DoubleClick, 0, (BlueNoah.Event.EventData ev) =>
             {
                 mIsHighSpeed = !mIsHighSpeed;
             });
        }

        float progress = 0;

        bool mIsHighSpeed = false;

        public void OnUpdate()
        {
            //TODO GameTimeUtility が必要
            //int index = (int)GameTimeUtility.GetTimeScaleNow;
            int index = 1;
            //TODO　GameTimeUtility が必要
            //float lerp = GameTimeUtility.GetTimeScaleProgress;
            float lerp = 0;
            if (mIsHighSpeed)
            {
                progress += Time.deltaTime / 3;

                index = (int)progress;

                lerp = progress % 1;
            }

            TimeChangeEntity timeAttribute = mTimeAttributes[index % mTimeAttributes.Count];

            mLightingMat.mainTexture = timeAttribute.lightTexture;

            if (lerp < 0.5f)
            {
                TimeChangeEntity timeAttributeBefore = mTimeAttributes[(index + mTimeAttributes.Count - 1) % mTimeAttributes.Count];

                mMainLight.color = Color.Lerp(timeAttributeBefore.lightColor, timeAttribute.lightColor, lerp + 0.5f);
            }
            else
            {
                TimeChangeEntity timeAttributeAfter = mTimeAttributes[(index + 1) % mTimeAttributes.Count];

                mMainLight.color = Color.Lerp(timeAttribute.lightColor, timeAttributeAfter.lightColor, lerp - 0.5f);
            }
        }
    }
}

