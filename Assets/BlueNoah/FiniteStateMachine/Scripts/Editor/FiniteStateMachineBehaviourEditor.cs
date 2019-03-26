using UnityEditor;
using UnityEngine;

namespace BlueNoah.AI.FSM
{
    //[CustomEditor(typeof(FiniteStateMachineBehaviour))]
    public class FiniteStateMachineBehaviourEditor : UnityEditor.Editor
    {

        FiniteStateMachineBehaviour mFiniteStateMachineBehaviour;
        GUIStyle editorGUIStyle;

        private void OnEnable()
        {
            mFiniteStateMachineBehaviour = this.target as FiniteStateMachineBehaviour;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            GUI.color = Color.white;
            if (editorGUIStyle == null)
            {
                editorGUIStyle = GUI.skin.box;
                editorGUIStyle.alignment = TextAnchor.MiddleCenter;
            }
            DrawFiniteStateMachine(mFiniteStateMachineBehaviour.MainFiniteStateMachine);
        }

        void DrawDragAndDrop()
        {
            DragAndDrop.AcceptDrag();
        }

        void DrawFiniteStateMachine(FiniteStateMachine finiteStateMachine)
        {
            for (int i = 0; i < finiteStateMachine.stateList.Count; i++)
            {
                if (finiteStateMachine.stateList[i] == finiteStateMachine.CurrentState)
                    GUI.color = Color.green;
                EditorGUILayout.BeginHorizontal();
                FSMState state = finiteStateMachine.GetState(finiteStateMachine.stateList[i]);
                DrawState(state);
                GUI.color = Color.white;
                EditorGUILayout.EndHorizontal();
            }
        }

        void DrawState(FSMState state)
        {
            EditorGUILayout.LabelField("stateId:" + state.state.ToString(), GUILayout.Width(100));
            for (int i = 0; i < state.actions.Count; i++)
            {
                DrawAction(state.actions[i]);
            }
        }

        void DrawAction(FSMAction action)
        {
            string actionName = action.GetType().ToString();
            EditorGUILayout.LabelField(actionName, GUILayout.Width(GetTextSizeForGUI(actionName) + 10));
        }

        float GetTextSizeForGUI(string text)
        {
            GUIContent content = new GUIContent(text);
            Vector2 size = editorGUIStyle.CalcSize(content);
            return size.x;
        }
    }
}

