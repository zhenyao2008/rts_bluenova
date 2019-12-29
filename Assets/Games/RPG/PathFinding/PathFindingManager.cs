using System.Collections.Generic;
using UnityEngine;

namespace BlueNoah.RPG.PathFinding
{
    public class PathFindingManager : MonoBehaviour
    {

        static PathFindingManager mInstance;

        public static PathFindingManager Single
        {
            get
            {
                if (mInstance == null)
                {
                    NewInstance();
                }
                return mInstance;
            }
        }

        public GStarGrid Grid;
        //同期に実行
        public PathAgent PathAgent;

        public bool IsEditorDebug = false;

        public bool IsShowGrid = false;

        private PathFindingManager(){}

        public static PathFindingManager NewInstance()
        {
            GameObject go = new GameObject("PathFinding");
            go.transform.position = Vector3.zero;
            mInstance = go.AddComponent<PathFindingManager>();
            return mInstance;
        }

        public void InitPathFinding(BattleFieldDataSO battleFieldData)
        {
            GameObject gridGo = new GameObject("Grid");
            gridGo.transform.SetParent(gameObject.transform);
            gridGo.transform.localPosition = Vector3.zero;
            Grid = gridGo.GetOrAddComponent<GStarGrid>();
            Grid.Init(battleFieldData);
            PathAgent = new PathAgent(Grid);
#if !UNITY_EDITOR
            IsEditorDebug = false;
#endif
        }

        public Node GetNode(ActorCore unit)
        {
            return unit.MoveAgent.MainNode;
        }

        public Node GetNode(Vector3Int position)
        {
            return Grid.GetNode(position);
        }

        public List<Node> FindPath(Vector3Int startPos,Vector3Int endPos, ActorCore targetUnit, float maxCost, int stopDistance,GStarMoveAgentBase moveAgent)
        {
            return PathAgent.StartFind(startPos, endPos, targetUnit, maxCost, stopDistance, moveAgent);
        }

        public void OnUpdate() { }

        private void OnDrawGizmos()
        {
            if (IsShowGrid && Grid != null)
            {
                Grid.DrawNodeInfos();
            }
        }

        private void OnDrawGizmosSelected()
        {
            if (PathAgent != null)
            {
                PathAgent.OnDrawGizmos();
            }
        }
    }
}