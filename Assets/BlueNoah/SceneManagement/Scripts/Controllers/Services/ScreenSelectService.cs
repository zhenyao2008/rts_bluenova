using BlueNoah.Event;
using UnityEngine;
using UnityEngine.Events;

namespace BlueNoah.SceneControl
{
    public class ScreenSelectService
    {

        Vector3 mStartPos;

        Vector3 mCurrentPos;

        bool mIsSelecting;

        bool mIsSelectable;

        Texture2D texture2D;

        public UnityAction<Rect> onSelected;

        public ScreenSelectService()
        {
            texture2D = CreateSelectionTexture2D();
            EasyInput.Instance.AddListener(Event.TouchType.TouchBegin, OnTouchBegin);
            EasyInput.Instance.AddListener(Event.TouchType.TouchEnd, OnTouchEnd);
            EasyInput.Instance.AddListener(Event.TouchType.Touch, OnTouchMove);
        }

        public void OnGUI()
        {
            DrawSelectGUI();
        }
        //need drive by controller.
        public void DrawSelectGUI()
        {
            if (mIsSelecting)
            {
                float xMin = Mathf.Min(mStartPos.x, mCurrentPos.x);
                float yMin = Mathf.Min(mStartPos.y, mCurrentPos.y);
                float xMax = Mathf.Max(mStartPos.x, mCurrentPos.x);
                float yMax = Mathf.Max(mStartPos.y, mCurrentPos.y);
                Rect rect = new Rect(xMin, Screen.height - yMax, xMax - xMin, yMax - yMin);
                Graphics.DrawTexture(rect, texture2D, 1, 1, 1, 1);
            }
        }

        Texture2D CreateSelectionTexture2D()
        {
            texture2D = new Texture2D(4, 4);
            texture2D.SetPixel(0, 0, new Color(0, 0, 0, 1));
            texture2D.SetPixel(0, 1, new Color(0, 0, 0, 1));
            texture2D.SetPixel(0, 2, new Color(0, 0, 0, 1));
            texture2D.SetPixel(0, 3, new Color(0, 0, 0, 1));
            texture2D.SetPixel(1, 0, new Color(0, 0, 0, 1));
            texture2D.SetPixel(1, 3, new Color(0, 0, 0, 1));
            texture2D.SetPixel(2, 0, new Color(0, 0, 0, 1));
            texture2D.SetPixel(2, 3, new Color(0, 0, 0, 1));
            texture2D.SetPixel(3, 0, new Color(0, 0, 0, 1));
            texture2D.SetPixel(3, 1, new Color(0, 0, 0, 1));
            texture2D.SetPixel(3, 2, new Color(0, 0, 0, 1));
            texture2D.SetPixel(3, 3, new Color(0, 0, 0, 1));
            texture2D.SetPixel(1, 1, new Color(0, 1, 0, 0.3f));
            texture2D.SetPixel(1, 2, new Color(0, 1, 0, 0.3f));
            texture2D.SetPixel(2, 1, new Color(0, 1, 0, 0.3f));
            texture2D.SetPixel(2, 2, new Color(0, 1, 0, 0.3f));
            texture2D.Apply();
            return texture2D;
        }

        void OnTouchBegin(EventData eventData)
        {
            if (Input.GetKey(KeyCode.LeftControl))
            {
                mIsSelecting = true;
            }
            mStartPos = eventData.touchStartPos0;
        }

        void OnTouchMove(EventData eventData)
        {
            if (!Input.GetKey(KeyCode.LeftControl))
            {
                mIsSelecting = false;
            }
            mCurrentPos = eventData.touchPos0;
        }

        void OnTouchEnd(EventData eventData)
        {
            if (mIsSelecting)
            {
                mIsSelecting = false;
                float xMin = Mathf.Min(mStartPos.x, mCurrentPos.x);
                float yMin = Mathf.Min(mStartPos.y, mCurrentPos.y);
                float xMax = Mathf.Max(mStartPos.x, mCurrentPos.x);
                float yMax = Mathf.Max(mStartPos.y, mCurrentPos.y);
                Rect rect = new Rect(xMin, yMin, xMax - xMin, yMax - yMin);
                if (onSelected != null)
                    onSelected(rect);
            }
        }

        public bool IsSelectable
        {
            get
            {
                return mIsSelectable;
            }
            set
            {
                mIsSelecting = value;
            }
        }
    }
}
