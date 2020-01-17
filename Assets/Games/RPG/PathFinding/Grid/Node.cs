using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace BlueNoah.RPG.PathFinding
{

    [System.Serializable]
    public class Node
    {
        //グリッドのノード配列でノード（マス）のXインデックス
        public int X;
        //グリッドのノード配列でノード（マス）のYインデックス
        public int Z;
        //この最初ノードからこのノードまで、探索したコスト
        public int G;
        //このノードからターゲットノードまでの予想コスト
        public int H;
        //予想コストの和
        //F = Full.
        public int F;
        //検索中ノード（マス）と次のノード（マス）の間の方向関係。
        [System.Obsolete]
        public int SearchDirect;
        //このノード（マス）上で移動速度関連、例えば：水と草地上の速度が違うのために、これで判断する。
        [System.Obsolete]
        public float MultiplyCost = 1;
        //その他コスト
        float _AdditionCost = 0;

        public float AdditionCost
        {
            get
            {
                return _AdditionCost;
            }
        }

        //このノード（マス）に接続しているノード（マス）。
        [System.NonSerialized]
        public List<Node> Neighbors = new List<Node>();
        //接続しているノード（マス）の距離コスト、斜めは1.4f.
        [System.NonSerialized]
        public List<int> NeighborCosts = new List<int>();
        //地形的な接続ノードではなく、特定仕様によって繋ぐ。
        [System.NonSerialized]
        public List<Node> Links = new List<Node>();
        //繋ぐコスト、経路と範囲をコントロール。
        [System.NonSerialized]
        public List<int> LinkCosts = new List<int>();
        //このノード（マス）で移動できるかどうか
        public bool IsBlock;
        //臨時的にノードをブロックする
        [System.Obsolete]
        public bool IsTempBlock;
        //壁に向かってるのかどうか
        [System.Obsolete]
        public bool IsWallSide;
        //ゲーム世界のポジション。
        //[System.NonSerialized]
        public Vector3 Pos;
        //グリッドポジション。
        public Vector3Int GridPos { get { return new Vector3Int(X, 0, Z); } }
        [System.NonSerialized]
        public Node Previous;
        //このノード（マス）がオッペンリスト中にあるかどうか判断
        public int IsOpen;
        //このノード（マス）がクローズリスト中にあるかどうか判断
        public int IsClose;
        //ノード（マス）上のマップ情報
        [System.NonSerialized]
        public BattleTileInfo otherInfo;
        //オッペンリスト操作ために使う
        [System.NonSerialized]
        [System.Obsolete]
        public Node preLinkedNode;
        //オッペンリスト操作ために使う
        [System.NonSerialized]
        [System.Obsolete]
        public Node nextLinkedNode;
        //違うエリアのノード（マス）が接続していない、これはIsBlockに基づいて判断。
        public int AreaId;
        //アリアを更新インデックス
        public int AreaUpdateIndex;

        public int IndexInBinaryHeap;

        public int LayerMask;

        public int SubLayerMask;

        public int SlopeLayerMask;

        //このノードがパスの中にいくつかある。
        [System.Obsolete]
        public int CountInPath;
        //通過中のユニットはこれでブロック
        [System.NonSerialized]
        [System.Obsolete]
        public GStarMoveAgentBase PassReserveAgent;
        //停止中のユニットはこれでブロック
        [System.NonSerialized]
        public GStarMoveAgentBase HaltReserveAgent;
        //移動時の地形コスト
        [System.Obsolete]
        public float LandFormCost = 1;
        //プレイヤー側の火力評価値
        [System.Obsolete]
        public float PlayerFirepower = 0;
        //AI側の火力評価値
        [System.Obsolete]
        public float ComputerFirepower = 0;

        [System.Obsolete]
        public float Distance = 0;

        [System.Obsolete]
        public int MoveFirePowerCountIndex;
        [System.Obsolete]
        public int FirePowerUpdateCount;

        public int PlayerScore;

        public int ComputerScore;

        public float PlayerDistanceScore;

        public float ComputerDistanceScore;
        [System.Obsolete]
        public int NodeSearchIndex;
        //範囲スキルキャスト場合AI計算用。
        [System.Obsolete]
        public float EffectRangeScore;
        //見た目はターゲットと距離が短いノードを選択ために
        [System.Obsolete]
        public float EffectAdditionScore;
        //ドーア
        [System.Obsolete]
        public bool IsDoor;
        //破壊できる障碍
        [System.Obsolete]
        public bool IsViolableObstacle;

        public void ResetMaskAndCost()
        {
            LayerMask = (int)otherInfo.MainAttr;
            SubLayerMask = (int)otherInfo.SubAttr;
            SlopeLayerMask = (int)otherInfo.SlopeAttr;
            if (otherInfo.MainAttr == FieldMainAttr.OutOfField)
            {
                IsBlock = true;
            }
            MultiplyCost = 1;
            /*
            LayerMask = NodeLayer.Land;
            MultiplyCost = 1;

            if (otherInfo.MainAttr == FieldMainAttr.OutOfField)
            {
                IsBlock = true;
            }
            else if (otherInfo.MainAttr == FieldMainAttr.NotWalkable)
            {
                LayerMask = NodeLayer.Precipice;
            }
            else if (otherInfo.MainAttr == FieldMainAttr.WalkableWater)
            {
               //MultiplyCost = 3f;
                LayerMask = NodeLayer.WalkableWater;
                LandFormCost = 2;
            }
            else if (otherInfo.MainAttr == FieldMainAttr.Water)
            {
                //MultiplyCost = 10f;
                LayerMask = NodeLayer.Water;
                LandFormCost = 2;
            }else if (otherInfo.MainAttr == FieldMainAttr.Lava)
            {
                LayerMask = NodeLayer.Lava;
            }*/
        }

        public Node(int f)
        {
            this.F = f;
        }

        public bool IsNearBy(Node node)
        {
            if (Mathf.Abs(X - node.X) <= 1 && Mathf.Abs(Z - node.Z) <=1)
            {
                return true;
            }
            return false;
        }

        public void PlusAdditionCost(float cost)
        {
            _AdditionCost = Mathf.Max(cost, _AdditionCost);
        }

        public void MinusAdditionCost(float cost)
        {
            _AdditionCost = Mathf.Max(0, _AdditionCost - cost);
        }

        const float CommonAddtionCost = 3;

        public void PlusCommonAddtionCost()
        {
            PlusAdditionCost(CommonAddtionCost);
        }

        public void MinusCommonAddtionCost()
        {
            MinusAdditionCost(CommonAddtionCost);
        }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(string.Format("X:{0};Z:{1};\n",X,Z));
            stringBuilder.Append(string.Format("G:{0};H:{1};F:{2};\n", G, H,F));
            stringBuilder.Append(string.Format("Pos:{0};\n", Pos));
            stringBuilder.Append(string.Format("LayerMask:{0};\n", LayerMask));
            stringBuilder.Append(string.Format("IsBlock:{0};\n", GetBoolString(IsBlock,true)));
            //stringBuilder.Append(string.Format("IsTempBlock:{0};\n", GetBoolString(IsTempBlock, true)));
            stringBuilder.Append(string.Format("IsDoor:{0};\n", GetBoolString(IsDoor, true)));
            stringBuilder.Append(string.Format("IsViolableObstacle:{0};\n", GetBoolString(IsViolableObstacle, true)));
            //stringBuilder.Append(string.Format("AdditionCost:{0};\n", GetFloatString(_AdditionCost,0)));
            //stringBuilder.Append(string.Format("MultiplyCost:{0};\n", GetFloatString(MultiplyCost,1)));
            stringBuilder.Append(string.Format("HaltReserveAgent:{0};\n", GetBoolString(HaltReserveAgent != null, true)));
            if (HaltReserveAgent != null)
            {
                stringBuilder.Append(string.Format("X{0},Z:{1};\n", HaltReserveAgent.XSize, HaltReserveAgent.ZSize));
            }
            return stringBuilder.ToString();
        }

        string GetBoolString(bool v,bool target)
        {
            return v== target ? "<color=red>" + v +"</color>" : v.ToString();
        }
    }
}
