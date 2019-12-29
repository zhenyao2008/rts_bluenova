/*
 *　2019.3.6 午後７時４分、東京恵比寿
 *　應　彧剛(yingyugang@gmail.com)
*/
using HighlightingSystem;
using UnityEngine;

namespace BlueNoah.RPG.View
{
    public class ActorHighlighter : MonoBehaviour
    {

        Highlighter mHighlighter;
        void Awake()
        {
            mHighlighter = GetComponentInChildren<Highlighter>(true);
            if (mHighlighter == null)
            {
                mHighlighter = gameObject.GetOrAddComponent<Highlighter>();
            }
        }

        public void ShowHighlighter()
        {
            mHighlighter.ConstantOnImmediate(Color.green);
        }

        public void HideHighlighter()
        {
            mHighlighter.ConstantOffImmediate();
        }
    }
}