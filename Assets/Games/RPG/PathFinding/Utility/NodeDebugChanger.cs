using UnityEngine;
///
/// @file  NodeDebugChanger.cs
/// @author Ying YuGang
/// @date   
/// @brief 
/// Copyright 2019 Grounding Inc. All Rights Reserved.
///
namespace BlueNoah.RPG.PathFinding
{
    public class NodeDebugChanger : MonoBehaviour
    {
        public int XIndex = 21;
        public int ZIndex = 16;
        public Node CurrentNode;

        public int XIndex1 = 22;
        public int ZIndex1 = 35;
        public Node CurrentNode1;

        public bool IsGetNode;


        private void Update()
        {
            if (IsGetNode)
            {
                IsGetNode = false;
                CurrentNode = PathFindingManager.Single.Grid.GetNode(Mathf.Max(0,XIndex),Mathf.Max(0,ZIndex));
                CurrentNode1 = PathFindingManager.Single.Grid.GetNode(Mathf.Max(0, XIndex1), Mathf.Max(0, ZIndex1));

                GameObject go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                go.transform.position = CurrentNode.Pos;

                GameObject go1 = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                go1.transform.position = CurrentNode1.Pos;

                CurrentNode.Links.Add(CurrentNode1);
                CurrentNode.LinkCosts.Add(1000);

                CurrentNode1.Links.Add(CurrentNode);
                CurrentNode1.LinkCosts.Add(1000);

                Debug.Log("<color=yellow>Link is added.</color>");
            }
        }
    }
}