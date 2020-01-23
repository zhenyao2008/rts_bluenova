using System.Collections.Generic;
using UnityEngine;
///
/// @file  ColorChanger.cs
/// @author Ying YuGang
/// @date   
/// @brief 
/// Copyright 2019 Grounding Inc. All Rights Reserved.
///
namespace BlueNoah.RPG.PathFinding
{
    public class ColorChanger : GStarGridBaseService
    {
       
        public Dictionary<int,Node[]> NodeGroups;

        public bool IsShowGrid;
        [System.Obsolete]
        public bool IsShowMovement;

        public bool IsShowInfluence = false;

        public bool IsShowTarget = true;

        GridViewGroup GridView;

        public ColorChanger(GStarGrid grid) : base(grid)
        {
            NodeGroups = new Dictionary<int,Node[]>();
            GridView = grid.GridView;
        }

        public void ToggleGridShowing(bool showInfluence = false)
        {
            this.IsShowInfluence = showInfluence;
            IsShowGrid = !IsShowGrid;
            if (!IsShowGrid)
            {
                GridView.HideGrid();
            }
            else
            {
                GridView.ShowGrid();
            }
        }

        public void ResetNodeColors(int groupId)
        {
            if (NodeGroups.ContainsKey(groupId))
            {
                ResetNodeGroupColors(NodeGroups[groupId]);
            }
        }
        
        void ResetNodeGroupColors(Node[] nodes)
        {
            if (nodes != null)
            {
                 UpdateNodeColors(nodes);
            }
        }

        public void ClearAllGroupColors()
        {
            if (NodeGroups!=null)
            {
                foreach (int groupId in NodeGroups.Keys)
                {
                    ResetNodeGroupColors(NodeGroups[groupId]);
                }
                NodeGroups.Clear();
            }
        }

        public void ChangeNodeGroupColors(int groupId, Node[] nodes,Color color)
        {
            if (IsShowTarget)
            {
                if (!NodeGroups.ContainsKey(groupId))
                {
                    NodeGroups.Add(groupId, null);
                }
                Node[] preNodes = NodeGroups[groupId];
                if (preNodes != null)
                {
                    ResetNodeGroupColors(preNodes);
                }
                ChangeNodeColors(nodes, color);
                NodeGroups[groupId] = nodes;
            }
        }

        void ChangeNodeColors(Node[] nodes,Color color)
        {
            if (nodes!=null)
            {
                for (int i = 0; i < nodes.Length; i++)
                {
                    if (nodes[i] != null)
                    {
                        GridView.SetNodeColor(nodes[i], color);
                    }
                }
            }
        }
        [System.Obsolete]
        public const int ACTION_SKILL_TARGET_RANGE_0 = 0;
        [System.Obsolete]
        public const int ACTION_SKILL_EFFECT_RANGE_0 = 1;
        [System.Obsolete]
        public const int ACTION_SKILL_TARGET_RANGE_1 = 2;
        [System.Obsolete]
        public const int ACTION_SKILL_EFFECT_RANGE_1 = 3;

        public const int SKILL_TARGET_RANGE = 4;
        [System.Obsolete]
        public const int SKILL_EFFECT_RANGE = 5;

        public const int MOVE_RANGE = 6;
        [System.Obsolete]
        public const int SKILL_RANGE = 7;
        [System.Obsolete]
        public const int SELF_RANGE = 8;
        [System.Obsolete]
        public const int MOVE_IMPACT_RANGE = 9;
        public const int MOVE_PATH = 10;
        public const int SCAN_RANGE = 11;
        [System.Obsolete]
        public const int BEST_TARGET = 12;

        public static Color MovePathColor = Color.yellow;
        public static Color ScanRangeColor = Color.yellow;

        public static Color TargetRangeColor = Color.green;
        [System.Obsolete]
        public static Color EffectRangeColor = Color.green;
        [System.Obsolete]
        public static Color HaltReserveColor = new Color(128f / 255, 0, 128f / 255, 1);

        public static Color OutOfFieldColor = new Color(1, 0, 0, 0.0f);

        public static Color NotWalkableColor = new Color(1, 0, 0, 0.0f);

        public static Color WaterColor = new Color(0, 0, 1, 0f);
        
        public static Color WalkbableWaterColor = new Color(0.5f, 0.5f, 1, 0f);

        public static Color NormalColor = new Color(0, 1, 0, 1f);

        public void UpdateAllNodesColor()
        {
            if (IsShowInfluence)
            {
                UpdateAllNodesWithInfluence();
            }
            else if (IsShowMovement)
            {
                UpdateAllNodesWithMovement();
            }
            else
            {
                UpdateAllNodesWithMainAttr();
            }
        }

        public void UpdateNodeColors(Node[] nodes)
        {
            if (IsShowInfluence)
            {
                SetNodeColorWithInfluence(nodes);
            }
            else if (IsShowMovement)
            {
                SetNodeColorWithMovement(nodes);
            }
            else
            {
                SetNodeColorByFieldMainAttr(nodes);
            }
        }

        public void UpdateNodeColor(Node node)
        {
            if (IsShowInfluence)
            {
                SetNodeColorWithInfluence(node);
            }
            else if (IsShowMovement)
            {
                SetNodeColorWithMovement(node);
            }
            else
            {
                SetNodeColorByFieldMainAttr(node);
            }
        }

        #region 1.Movement.
        [System.Obsolete]
        void UpdateAllNodesWithMovement()
        {
            for (int i = 0; i < Grid.XCount; i++)
            {
                for (int j = 0; j < Grid.ZCount; j++)
                {
                    SetNodeColorWithMovement(Grid.Nodes[i, j]);
                }
            }
        }
        [System.Obsolete]
        void SetNodeColorWithMovement(Node[] nodes)
        {
            if (nodes!=null)
            {
                for (int i = 0;i<nodes.Length;i++)
                {
                    SetNodeColorWithMovement(nodes[i]);
                }
            }
        }
        [System.Obsolete]
        void SetNodeColorWithMovement(Node node)
        {
            if (node !=null)
            {
                if (node.HaltReserveAgent != null)
                {
                   GridView.SetNodeColor(node, HaltReserveColor);
                }
                else
                {
                    SetNodeColorByFieldMainAttr(node);
                }
            }
        }
        #endregion

        #region 2.Field information.
        public void UpdateAllNodesWithMainAttr()
        {
            for (int i = 0; i < Grid.XCount; i++)
            {
                for (int j = 0; j < Grid.ZCount; j++)
                {
                    SetNodeColorByFieldMainAttr(Grid.Nodes[i, j]);
                }
            }
            GridView.ApplyColors();
        }

        void SetNodeColorByFieldMainAttr(Node[] nodes) {
            if (nodes!=null)
            {
                for (int i = 0; i < nodes.Length; i++)
                {
                    SetNodeColorByFieldMainAttr(nodes[i]);
                }
            }
        }

        void SetNodeColorByFieldMainAttr(Node node)
        {
            if (node!=null && node.otherInfo != null)
            {
                switch (node.otherInfo.MainAttr)
                {
                    case FieldMainAttr.OutOfField:
                        GridView.SetNodeColor(node, OutOfFieldColor);
                        break;
                    case FieldMainAttr.NotWalkable:
                        GridView.SetNodeColor(node, NotWalkableColor);
                        break;
                    case FieldMainAttr.Water:
                        GridView.SetNodeColor(node, WaterColor);
                        break;
                    case FieldMainAttr.WalkableWater:
                        GridView.SetNodeColor(node, WalkbableWaterColor);
                        break;
                    default:
                        GridView.SetNodeColor(node, NormalColor);
                        break;
                }
            }
        }
        #endregion

        #region 3.Influnce. 
        public void UpdateAllNodesWithInfluence(bool isPlayer = false)
        {
            for (int i = 0; i < Grid.XCount; i++)
            {
                for (int j = 0; j < Grid.ZCount; j++)
                {
                    SetNodeColorWithInfluence(Grid.Nodes[i, j], isPlayer);
                }
            }
            GridView.ApplyColors();
        }

        public void UpdateAllNodesWithInfluenceEdge(bool isPlayer = false)
        {
            for (int i = 0; i < Grid.XCount; i++)
            {
                for (int j = 0; j < Grid.ZCount; j++)
                {
                    SetNodeColorWithInfluenceEdge(Grid.Nodes[i, j], isPlayer);
                }
            }
            GridView.ApplyColors();
        }

        void SetNodeColorWithInfluence(Node[] nodes, bool isPlayer = false)
        {
            if (nodes!=null)
            {
                for (int i=0;i<nodes.Length;i++)
                {
                    SetNodeColorWithInfluence(nodes[i], isPlayer);
                }
            }
        }

        public void SetNodeColorWithInfluenceEdge(Node node, bool isPlayer = false)
        {
            if (node != null)
            {
                SetNodeColorByFieldMainAttr(node);
                
                if (isPlayer)
                {
                   
                    if ( node.PlayerScore > 0)
                    {
                        bool isEdge = false;
                        for (int i = 0; i < node.Neighbors.Count; i++)
                        {
                            if (node.Neighbors[i].PlayerScore == 0)
                            {
                                isEdge = true;
                            }
                        }
                        if (isEdge)
                        {
                            GridView.SetNodeColor(node, Color.red);
                        }
                        else
                        {
                            GridView.SetNodeColor(node, Color.white);
                        }
                    }
                }
                else
                {
                    if (node.ComputerScore > 0)
                    {
                        bool isEdge = false;
                        for (int i = 0; i < node.Neighbors.Count; i++)
                        {
                            if (node.Neighbors[i].ComputerScore == 0)
                            {
                                isEdge = true;
                            }
                        }
                        if (isEdge)
                        {
                            GridView.SetNodeColor(node, Color.red);
                        }
                        else
                        {
                            GridView.SetNodeColor(node, Color.white);
                        }
                    }
                }
            }
        }

        public void SetNodeColorWithInfluence(Node node, bool isPlayer = false)
        {
            if (node != null)
            {
                SetNodeColorByFieldMainAttr(node);
                if (isPlayer)
                {
                    if (node.PlayerScore > 0)
                    {
                        switch (node.PlayerScore)
                        {
                            case 1:
                                GridView.SetNodeColor(node,Color.yellow);
                                break;
                            case 2:
                                GridView.SetNodeColor(node, new Color(1, 0.5f, 0, 1));
                                break;
                            case 3:
                                GridView.SetNodeColor(node, Color.red);
                                break;
                            default:
                                GridView.SetNodeColor(node, new Color(1, 0, 1, 1));
                                break;
                        }
                    }
                }
                else
                {
                    if (node.ComputerScore > 0)
                    {
                        switch (node.ComputerScore)
                        {
                            case 1:
                                GridView.SetNodeColor(node, Color.yellow);
                                break;
                            case 2:
                                GridView.SetNodeColor(node, new Color(1, 0.5f, 0, 1));
                                break;
                            case 3:
                                GridView.SetNodeColor(node, Color.red);
                                break;
                            default:
                                GridView.SetNodeColor(node, new Color(1,0,1,1));
                                break;
                        }
                    }
                }
            }
        }
        #endregion
        public void SetNodeColorByMainProperties(Node node)
        {
            if (node != null)
            {
                if (node.IsBlock)
                {
                    GridView.SetNodeColor(node, OutOfFieldColor);
                }
                else
                {
                    GridView.SetNodeColor(node, NormalColor);
                }
            }
        }

        public void ApplyColors()
        {
            GridView.ApplyColors();
        }
    }
}
