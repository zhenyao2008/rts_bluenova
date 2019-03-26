using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;

namespace BlueNoah.AI.FSM
{
    public class FiniteStateMachineWindowData
    {

        public FiniteStateMachine finiteStateMachine;

        public List<DragableState> dragableStateList;

        public Dictionary<short, DragableState> dragableStateDic;

        public int index;

        public void AddDragableState(DragableState dragableState)
        {
            dragableStateList.Add(dragableState);
            dragableStateDic.Add(dragableState.state.state, dragableState);
        }

        public FiniteStateMachineWindowData(FiniteStateMachine finiteStateMachine)
        {
            this.finiteStateMachine = finiteStateMachine;
            dragableStateList = new List<DragableState>();
            dragableStateDic = new Dictionary<short, DragableState>();
        }
    }

    public class DragableState
    {
        public FSMState state;

        public Rect rect;

        public FiniteStateMachineWindowData subFiniteStateMachineWindowData;

        public int stateIndex;

        public bool isDraging;

        public DragableState(FSMState state, Rect rect)
        {
            this.state = state;
            this.rect = rect;
        }
    }

    public class FiniteStateMachineWindow : EditorWindow
    {

        const int FSM_PARAMETER_START_X = 10;

        const int FSM_PARAMETER_START_Y = 10;

        const int FSM_PARAMETER_WIDTH = 180;

        const int FSM_DEFAULT_BUTTON_WIDTH = 100;

        const int FSM_DEFAULT_BUTTON_HEIGHT = 30;

        const int STATE_WINDOW_HEIGHT = 30;

        const int FSM_BOX_HEIGHT = 20;

        GameObject mCurrentGO;

        FiniteStateMachineSaveService mFiniteStateMachineSaveService;

        FiniteStateMachineWindowData mMainFiniteStateMachineWindowData;

        List<Dictionary<short, BoolVar>> mConditionParameterList;

        DragableState mDragableState;

        DragableState mCurrentDragingState;

        GUIStyle mActiveStyle;

        List<DragableState> mDragableStateList;

        Dictionary<int, DragableState> mDragableStateDic;

        int mCurrentIndex = 0;

        int mFSMCount = 0;

        Dictionary<int, FiniteStateMachineConfig> mFiniteStateMachineDic;

        object dragData = "dragData";

        bool isDraging;

        bool isBoxing;

        int cid;

        Vector2 mBoxStartPos;

        Vector2 mBoxCurrentPos;

        int mYOffset = 0;

        [MenuItem("Window/FiniteStateMachineWindow &f")]
        static void ShowWindow()
        {
            FiniteStateMachineWindow window = (FiniteStateMachineWindow)GetWindow(typeof(FiniteStateMachineWindow));
            window.Show();
            window.Init();
        }

        public void Init()
        {

            mFiniteStateMachineSaveService = new FiniteStateMachineSaveService();

            mConditionParameterList = new List<Dictionary<short, BoolVar>>();
        }

        void OnGUI()
        {
            if (!Application.isPlaying)
            {
                OnGUIUnPlaying();
            }
            else
            {
                OnGUIPlaying();
            }

            if (mMainFiniteStateMachineWindowData != null)
            {
                OnGUIFSM();
                OnGUISaveFSM();
            }
        }

        void OnGUIUnPlaying()
        {
            int index = 0;
            index++;
            if (mMainFiniteStateMachineWindowData == null)
            {
                if (GUI.Button(new Rect(FSM_PARAMETER_START_X, FSM_PARAMETER_START_Y, FSM_DEFAULT_BUTTON_WIDTH, FSM_DEFAULT_BUTTON_HEIGHT), "Load Configs"))
                {
                    mFiniteStateMachineDic = FiniteStateMachineLoader.LoadAIConfig();
                    Reset();
                }
                if (mFiniteStateMachineDic != null)
                {
                    foreach (int id in mFiniteStateMachineDic.Keys)
                    {
                        if (GUI.Button(new Rect(FSM_PARAMETER_START_X, FSM_PARAMETER_START_Y + FSM_DEFAULT_BUTTON_HEIGHT * index, FSM_DEFAULT_BUTTON_WIDTH, FSM_DEFAULT_BUTTON_HEIGHT), id.ToString()))
                        {
                            GameObject gameObject = new GameObject();
                            gameObject.hideFlags = HideFlags.HideInHierarchy;
                            FiniteStateMachine finiteStateMachine = new FiniteStateMachine(gameObject);
                            FiniteStateMachineLoader.InitFSM(finiteStateMachine, id);
                            InitFSM(finiteStateMachine);
                        }
                        index++;
                    }
                }
            }
            else
            {
                Rect backRect = new Rect(FSM_PARAMETER_START_X, FSM_PARAMETER_START_Y, 110, 30);
                if (GUI.Button(backRect, "Back"))
                {
                    Reset();
                }
            }
        }

        void OnGUIPlaying()
        {
            CheckSelection();
        }

        void CheckSelection()
        {
            if (mCurrentGO != null)
            {
                Rect rect = new Rect(FSM_PARAMETER_START_X, FSM_PARAMETER_START_Y, 110, 30);
                GUI.Label(rect, mCurrentGO.name);
            }

            GameObject gameObject = Selection.activeGameObject;
            if (gameObject != null)
            {
                FiniteStateMachineBehaviour finiteStateMachineBehaviour = gameObject.GetComponent<FiniteStateMachineBehaviour>();
                if (finiteStateMachineBehaviour != null)
                {
                    if (mMainFiniteStateMachineWindowData == null || finiteStateMachineBehaviour.MainFiniteStateMachine != mMainFiniteStateMachineWindowData.finiteStateMachine)
                    {
                        mCurrentGO = gameObject;
                        InitFSM(finiteStateMachineBehaviour.MainFiniteStateMachine);
                    }
                }
            }
        }

        void Reset()
        {
            mCurrentIndex = 0;
            mFSMCount = 0;
            mDragableStateList = new List<DragableState>();
            mDragableStateDic = new Dictionary<int, DragableState>();
            mConditionParameterList = new List<Dictionary<short, BoolVar>>();
            mMainFiniteStateMachineWindowData = null;
            mFiniteStateMachineSaveService = new FiniteStateMachineSaveService();
        }

        void InitFSM(FiniteStateMachine finiteStateMachine)
        {
            Reset();
            mMainFiniteStateMachineWindowData = InitDragableStateDatas(finiteStateMachine, 0);
            Debug.Log(mMainFiniteStateMachineWindowData);
        }

        int GetStateHeight(FSMState state)
        {
            int height = STATE_WINDOW_HEIGHT + FSM_BOX_HEIGHT * (state.actions.Count + 1);
            return height;
        }

        void OnGUIFSM()
        {
            GUI.color = Color.white;
            BeginWindows();
            mYOffset = 0;
            OnGUIParameters();
            OnGUIFiniteStateMachineWindow(this.mMainFiniteStateMachineWindowData);
            EndWindows();

            UnityEngine.Event e = UnityEngine.Event.current;
            cid = GUIUtility.GetControlID(FocusType.Passive);
            switch (e.GetTypeForControl(cid))
            {
                case EventType.MouseDown:
                    //Debug.Log("MouseDown");
                    mCurrentDragingState = null;
                    GUIUtility.hotControl = cid;
                    for (int i = 0; i < mDragableStateList.Count; i++)
                    {
                        if (mDragableStateList[i].rect.Contains(e.mousePosition))
                        {
                            mCurrentDragingState = mDragableStateList[i];
                            DragAndDrop.PrepareStartDrag();
                            DragAndDrop.SetGenericData("dragflag", dragData);
                            DragAndDrop.StartDrag("");
                            isDraging = true;
                            e.Use();
                            break;
                        }
                    }
                    if (!isDraging)
                    {
                        mBoxStartPos = e.mousePosition;
                        isBoxing = true;
                        Debug.Log(mBoxStartPos);
                    }
                    break;
                case EventType.MouseUp:
                    //Debug.Log("MouseUp");
                    if (GUIUtility.hotControl == cid)
                        GUIUtility.hotControl = 0;
                    mCurrentDragingState = null;
                    isBoxing = false;
                    isDraging = false;
                    break;
                case EventType.MouseDrag:
                    //Debug.Log("MouseDrag");
                    if (mCurrentDragingState != null)
                        mCurrentDragingState.rect.center = e.mousePosition;
                    break;
                case EventType.DragUpdated:
                    //Debug.Log("DragUpdated");
                    e.Use();
                    break;
                case EventType.DragPerform:
                    //Debug.Log("DragPerform");
                    DragAndDrop.AcceptDrag();
                    //Debug.Log("DragPerform : " + DragAndDrop.GetGenericData("dragflag"));
                    e.Use();
                    break;
                case EventType.DragExited:
                    //Debug.Log("DragExited");
                    isDraging = false;
                    if (GUIUtility.hotControl == cid)
                        GUIUtility.hotControl = 0;
                    e.Use();
                    break;
            }
        }

        FiniteStateMachineWindowData InitDragableStateDatas(FiniteStateMachine finiteStateMachine, int index)
        {
            mFSMCount++;

            FiniteStateMachineWindowData finiteStateMachineWindowData = new FiniteStateMachineWindowData(finiteStateMachine);

            finiteStateMachineWindowData.index = index;

            Dictionary<short, BoolVar> mConditionParameterDic = new Dictionary<short, BoolVar>();

            foreach (short condition in finiteStateMachine.ConditionDic.Keys)
            {
                if (!mConditionParameterDic.ContainsKey(condition))
                {
                    mConditionParameterDic.Add(condition, finiteStateMachine.ConditionDic[condition]);
                }
            }

            mConditionParameterList.Add(mConditionParameterDic);

            for (int i = 0; i < finiteStateMachineWindowData.finiteStateMachine.stateList.Count; i++)
            {

                mCurrentIndex++;

                FSMState state = finiteStateMachineWindowData.finiteStateMachine.GetState(finiteStateMachineWindowData.finiteStateMachine.stateList[i]);

                DragableState dragableState;

                dragableState = new DragableState(state, new Rect(state.statePosition.x, state.statePosition.y, 200, GetStateHeight(state)));

                //if (i % 2 == 0)
                //{
                //    dragableState = new DragableState(state, new Rect(FSM_WINDOW_START_X + 220 * index * 2, yOffset, 200, GetStateHeight(state)));
                //    maxCount = state.actions.Count;
                //}
                //else
                //{
                //    dragableState = new DragableState(state, new Rect(FSM_WINDOW_START_X + 220 * (index * 2 + 1), yOffset, 200, GetStateHeight(state)));
                //    maxCount = Mathf.Max(maxCount, state.actions.Count);
                //    yOffset = yOffset + 30 + 20 + 20 * (maxCount + 1);
                //}

                mDragableStateList.Add(dragableState);

                dragableState.stateIndex = mCurrentIndex;

                mDragableStateDic.Add(mCurrentIndex, dragableState);

                finiteStateMachineWindowData.AddDragableState(dragableState);

                if (state.HasSubState())
                {
                    dragableState.subFiniteStateMachineWindowData = InitDragableStateDatas(state.SubFiniteStateMachine, mFSMCount);
                }
            }
            return finiteStateMachineWindowData;
        }

        void OnGUIFiniteStateMachineWindow(FiniteStateMachineWindowData finiteStateMachineDragData)
        {
            OnGUIFSMStates(finiteStateMachineDragData);
        }

        void OnGUIFSMStates(FiniteStateMachineWindowData finiteStateMachineDragData)
        {
            for (int i = 0; i < finiteStateMachineDragData.dragableStateList.Count; i++)
            {
                OnGUIFSMState(finiteStateMachineDragData, finiteStateMachineDragData.dragableStateList[i]);
            }
        }

        void OnGUIFSMState(FiniteStateMachineWindowData finiteStateMachineDragData, DragableState dragableState)
        {
            if (finiteStateMachineDragData.finiteStateMachine.isActive && finiteStateMachineDragData.finiteStateMachine.CurrentState == dragableState.state.state)
            {
                if (Time.time * 4 % 1 > 0.5f)
                {
                    GUI.color = new Color(0, 1f, 0, 1f);
                }
                else
                {
                    GUI.color = new Color(0, 0.8f, 0, 1f);
                }
            }
            dragableState.rect = GUI.Window(dragableState.stateIndex, dragableState.rect, DrawNodeWindow, dragableState.state.state.ToString());
            GUI.color = Color.white;
            //OnGUIFSMActions(dragableState.rect, dragableState.state.actions);
            OnGUIFSMTransitions(finiteStateMachineDragData, dragableState.state.transitions);
            if (dragableState.subFiniteStateMachineWindowData != null)
            {
                OnGUIFiniteStateMachineWindow(dragableState.subFiniteStateMachineWindowData);
                if (dragableState.subFiniteStateMachineWindowData.dragableStateList.Count > 0)
                {
                    //DrawLine(dragableState.rect,dragableState.subFiniteStateMachineWindowData.dragableStateList[0].rect, Color.blue);
                    if (!dragableState.subFiniteStateMachineWindowData.finiteStateMachine.isActive)
                    {
                        DrawNodeCurve(dragableState.rect, dragableState.subFiniteStateMachineWindowData.dragableStateList[0].rect, Color.gray);
                    }
                    else
                    {
                        DrawNodeCurve(dragableState.rect, dragableState.subFiniteStateMachineWindowData.dragableStateList[0].rect, Color.yellow);
                    }
                }
            }
        }

        void DrawNodeWindow(int id)
        {
            if (mDragableStateDic != null && mDragableStateDic.ContainsKey(id))
            {
                DragableState dragableState = mDragableStateDic[id];
                OnGUIFSMActions(dragableState.state.actions);
            }
            GUI.DragWindow();
        }

        void OnGUIFSMActions(List<FSMAction> actions)
        {
            for (int i = 0; i < actions.Count; i++)
            {
                Rect rect = new Rect(10, 30 + 20 * (i), 170, 20);
                GUI.Box(rect, actions[i].ToString().Substring(actions[i].ToString().LastIndexOf(".", StringComparison.CurrentCulture) + 1));
            }
        }

        void OnGUIFSMTransitions(FiniteStateMachineWindowData finiteStateMachineDragData, List<FSMTransition> transitions)
        {
            for (int i = 0; i < transitions.Count; i++)
            {
                OnGUIFSMTransition(finiteStateMachineDragData, transitions[i]);
            }
        }

        void OnGUIFSMTransition(FiniteStateMachineWindowData finiteStateMachineDragData, FSMTransition transition)
        {
            DrawLine(finiteStateMachineDragData.dragableStateDic[transition.fromState].rect, finiteStateMachineDragData.dragableStateDic[transition.toState].rect, Color.green, transition.CurrentProgress);
            DrawLine(finiteStateMachineDragData.dragableStateDic[transition.fromState].rect, finiteStateMachineDragData.dragableStateDic[transition.toState].rect, Color.white);
            DrawArrow(finiteStateMachineDragData.dragableStateDic[transition.fromState].rect, finiteStateMachineDragData.dragableStateDic[transition.toState].rect, Color.white);
        }

        void OnInspectorUpdate()
        {
            Repaint();
        }

        void DrawSelectBox()
        {
            Rect rect = new Rect(mBoxStartPos.x, mBoxStartPos.y, mBoxCurrentPos.x - mBoxStartPos.x, mBoxCurrentPos.y - mBoxStartPos.y);
            Handles.DrawSolidRectangleWithOutline(rect, new Color(0, 1, 0, 0.1f), Color.green);
        }

        void DrawArrow(Rect start, Rect end, Color color)
        {
            Vector3[] arrowHead = new Vector3[3];
            Vector2 forward = (end.center - start.center).normalized;
            Vector2 direct = new Vector2(-forward.y, forward.x);
            Vector3 startPos = start.center + direct.normalized * 4;
            Vector3 endPos = end.center + direct.normalized * 4;
            Vector3 middlePos = Vector3.Lerp(startPos, endPos, 0.5f);
            float length = 5f;
            Vector3 forwardOffset = (Vector3)((end.center - start.center).normalized * length);
            arrowHead[0] = middlePos + forwardOffset;
            arrowHead[1] = middlePos - (Vector3)(direct * length + (end.center - start.center).normalized * length * 2) + forwardOffset;
            arrowHead[2] = middlePos + (Vector3)(direct * length - (end.center - start.center).normalized * length * 2) + forwardOffset;
            Handles.color = color;
            Handles.DrawAAConvexPolygon(arrowHead);
            Handles.color = Color.white;
        }

        void DrawLine(Rect start, Rect end, Color color, float t = 1)
        {
            Vector2 direct = (end.center - start.center).normalized;
            direct = new Vector2(-direct.y, direct.x);
            Vector2 startPos = start.center + direct.normalized * 4;
            Vector2 endPos = end.center + direct.normalized * 4;
            endPos = Vector2.Lerp(startPos, endPos, t);
            DrawLine(startPos, endPos, color);
        }

        void DrawLine(Vector2 startPos, Vector2 endPos, Color color)
        {
            Handles.color = color;
            for (int i = 0; i < 2; i++)
            {   //Draw a shadow
                Handles.DrawAAPolyLine(2 + i, new Vector3[] { startPos, endPos });
            }
            Handles.DrawAAPolyLine(2, new Vector3[] { startPos, endPos });
            Handles.color = Color.white;
        }

        void DrawNodeCurve(Rect start, Rect end, Color color)
        {
            Vector3 startPos;
            Vector3 endPos;
            bool isRight = false;
            if (end.x > start.x)
            {
                isRight = true;
            }
            if (isRight)
            {
                startPos = new Vector3(start.x + start.width, start.y + start.height / 2, 0);
                endPos = new Vector3(end.x, end.y + end.height / 2, 0);
            }
            else
            {
                startPos = new Vector3(start.x, start.y + start.height / 2, 0);
                endPos = new Vector3(end.x + end.width, end.y + end.height / 2, 0);
            }
            Vector3 startTan = startPos + Vector3.right * 50;
            Vector3 endTan = endPos + Vector3.left * 50;
            Color shadowCol = new Color(1, 1, 1, 0.06f);
            for (int i = 0; i < 3; i++)
            {   //Draw a shadow
                Handles.DrawBezier(startPos, endPos, startTan, endTan, shadowCol, null, (i + 1) * 5);
            }
            Handles.DrawBezier(startPos, endPos, startTan, endTan, color, null, 1);
        }

        void OnGUIParameters()
        {
            if (mConditionParameterList == null || mConditionParameterList.Count == 0)
            {
                return;
            }
            int count = 0;
            for (int i = 0; i < mConditionParameterList.Count; i++)
            {
                count += mConditionParameterList[i].Count;
            }
            count += (mConditionParameterList.Count);
            for (int i = 0; i < mConditionParameterList.Count; i++)
            {
                Dictionary<short, BoolVar> mConditionParameterDic = mConditionParameterList[i];
                if (i > 0)
                {
                    GUI.Label(new Rect(FSM_PARAMETER_START_X, 30 + FSM_BOX_HEIGHT * (1 + mYOffset), FSM_PARAMETER_WIDTH, FSM_BOX_HEIGHT), "-------------------------");
                    mYOffset++;
                }
                foreach (short finiteConditionConstant in mConditionParameterDic.Keys)
                {
                    DrawBoolVar(new Rect(FSM_PARAMETER_START_X, 30 + FSM_BOX_HEIGHT * (1 + mYOffset), FSM_PARAMETER_WIDTH, FSM_BOX_HEIGHT), finiteConditionConstant, mConditionParameterDic[finiteConditionConstant]);
                    mYOffset++;
                }
            }
        }

        void DrawBoolVar(Rect rect, short finiteConditionConstant, BoolVar boolVar)
        {
            boolVar.value = GUI.Toggle(rect, boolVar.value, finiteConditionConstant.ToString());
        }

        void OnGUISaveFSM()
        {
            Rect rect = this.position;

            Rect saveRect = new Rect(rect.width - 120, rect.height - 40, 110, 30);
            if (GUI.Button(saveRect, "Save"))
            {
                this.mFiniteStateMachineSaveService.Save(this.mMainFiniteStateMachineWindowData);
            }
        }

    }
}