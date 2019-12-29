using UnityEngine;

namespace BlueNoah.RPG.PathFinding
{

    public enum GridNeighborType {Four=0,Eight=1} 

    /*
     * 経路探索用グリッド。
     * Floatで計算バージョン
     */
    public class GStarGrid : MonoBehaviour
    {
        //G、H、F計算用倍数。
        public const int Multiple = 1000;
        //斜めコスト、1.4に設定するる。1.41421356237
        public const int DiagonalPlus = 1414;
        //ノード（マス）毎に単位長さ。
        public float EdgeLength = 1.0f;
        //グリッドの中心ポジション。
        public Vector3 CenterPosition = new Vector3(0, 0, 0);
        //最初のノード（マス）スタートポジション。
        public Vector3 NodeStartPosition;
        //グリッドのX方向のノード（マス）の数。
        public int XCount;
        //グリッドのZ方向のノード（マス）の数。
        public int ZCount;
        //地形の高さ、ノーマル、通過情報など保存されてる配列。
        public BattleTileInfo[] OtherInfos;
        //このグリッド中全てのノード。
        public Node[,] Nodes;
        //グリッドの合壁の数タイプ。
        public GridNeighborType GridNeighborType = GridNeighborType.Four;

        public BaseSeeker SeekerService;

        public Snapshoter SnapshotService;

        public BaseBarrier BarrierService;

        public ColorChanger ColorChangerService;

        public BaseNeighborCalculater NeighborCalculaterService;

        public BasePathSmoother PathSmootherService;

        public NodesDebuger NodesDebuger;

        public Identity SearchIdentity;

        public GridViewGroup GridView;

        //グリッドを初期化
        [System.Obsolete]
        public void Initialize(int x, int y, float edgeLength)
        {
            XCount = x;
            ZCount = y;
            EdgeLength = edgeLength;
            Init();
        }
        
        //グリッドを初期化
        public void Init(BattleFieldDataSO data)
        {
            SearchIdentity = new Identity(ResetSearchIndex);
            this.XCount = data.TileMapWidth;
            this.ZCount = data.TileMapHeight;
            this.EdgeLength = data.TileMapCellSize;
            this.OtherInfos = data.TileMap;
            Init();
        }
        
        //グリッドを初期化
        void Init()
        {
            XCount = Mathf.Max(1, XCount);
            ZCount = Mathf.Max(1, ZCount);
            Nodes = new Node[XCount, ZCount];
            float startX = -XCount / 2f * EdgeLength + 0.5f * EdgeLength;
            float startZ = -ZCount / 2f * EdgeLength + 0.5f * EdgeLength;
            NodeStartPosition = new Vector3(startX, 0, startZ);
            for (int j = 0; j < ZCount; j++)
            {
                for (int i = 0; i < XCount; i++)
                {
                    Node node = new Node(0);
                    node.X = i;
                    node.Z = j;
                    float height = 0;
                    //To set the cost and height if there is .
                    if (OtherInfos != null && OtherInfos.Length > 0)
                    {
                        if (OtherInfos.Length > j * ZCount + i)
                        {
                            BattleTileInfo battleTileInfo = this.OtherInfos[j * ZCount + i];
                            node.otherInfo = battleTileInfo;
                            //node.Pos = new Vector3(i * EdgeLength, battleTileInfo.Height, j * EdgeLength) + NodeStartPosition + CenterPosition;
                            //Nodes[i, j] = node;
                            height = battleTileInfo.Height;
                            node.ResetMaskAndCost();
                        }
                        else
                        {
                            Debug.LogError(j * ZCount + i + "is not existing.");
                        }
                    }
                    node.Pos = new Vector3(i * EdgeLength, height, j * EdgeLength) + NodeStartPosition + CenterPosition;
                    Nodes[i, j] = node;
                }
            }
            InitNeighborCalculater();
            InitGridSeeker();
            InitView();
            InitBarrier();
            
            SnapshotService = new Snapshoter(this);
            ColorChangerService = new ColorChanger(this);
            PathSmootherService = new BezierPathSmoother(this);
            NodesDebuger = new NodesDebuger(this);
#if UNITY_EDITOR
            ColorChangerService.UpdateAllNodesColor();
            ColorChangerService.ApplyColors();
#endif
        }

        //デバッグために。
        void Update()
        {
#if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.F))
            {
                GridView.ShowGrid();
            }
            if (Input.GetKeyDown(KeyCode.G))
            {
                GridView.HideGrid();
            }
            if (Input.GetKeyDown(KeyCode.H))
            {
                for (int i=0;i<XCount;i++)
                {
                    for (int j=0;j<ZCount;j++){
                        ColorChangerService.UpdateNodeColor(Nodes[i,j]);
                    }
                }
                var gridColorChanger = PathFindingManager.Single.Grid.ColorChangerService;
                gridColorChanger.IsShowInfluence = false;
                gridColorChanger.IsShowTarget = false;
                PathFindingManager.Single.Grid.GridView.ShowGrid();
                ColorChangerService.ApplyColors();
            }
            if (Input.GetKeyDown(KeyCode.J))
            {
                SkillTargetNodeDisplayUtility.ShowPlayerInfluenceEdgeNodes();
            }

            if (Input.GetKeyDown(KeyCode.U))
            {
                // 1.This is used to test skill range without move range.
                if (false)
                {
                    //UnitModel unitModel = BattlePresenter.Find().__UnitTurnContext.Unit;
                    //SkillTargetNodeDisplayUtility.ShowSkillRange(unitModel, unitModel.RegularAttack.TargetRangeNear, unitModel.RegularAttack.TargetRangeFar);
                }
                // 2.This is used to test home gimmick skill range without move range.
                if (false)
                {
                    //List<UnitModel> unitModels = BattlePresenter.Find().BattleModel.TurnOrderedUnitList;
                    //foreach (UnitModel unitModel in unitModels)
                    //{
                    //    if (unitModel.IsGimmick && unitModel.XSize >= 3 && unitModel.TeamId == TeamId.PlayerOne)
                    //    {
                    //        SkillTargetNodeDisplayUtility.ShowSkillRange(unitModel, unitModel.RegularAttack.TargetRangeNear, unitModel.RegularAttack.TargetRangeFar);
                    //    }
                    //}
                }
                // 3.This is used to test skill range with move range.
                if (false)
                {
                    //UnitModel unitModel = BattlePresenter.Find().__UnitTurnContext.Unit;

                    //SkillTargetNodeDisplayUtility.ShowSkillCastableNodes(unitModel, unitModel.RegularAttack.TargetRangeNear,unitModel.RegularAttack.TargetRangeFar);

                    //SkillTargetNodeDisplayUtility.ShowMoveableNodes(unitModel);
                }
            }
#endif
        }

        //表示用グリッド。
        void InitView()
        {
            GridView = new GridViewGroup(this);
        }

        public void ChangeNeighborType(GridNeighborType gridNeighborType)
        {
            GridNeighborType = gridNeighborType;
            InitNeighborCalculater();
        }

        void InitNeighborCalculater()
        {
            if (GridNeighborType == GridNeighborType.Four)
            {
                NeighborCalculaterService = new FourNeighborCalculater(this);
            }
            else if (GridNeighborType == GridNeighborType.Eight)
            {
                NeighborCalculaterService = new EightNeighborCalculater(this);
            }
            NeighborCalculaterService.CalculateNeighbors();
        }

        void InitGridSeeker()
        {
            if (GridNeighborType == GridNeighborType.Four)
            {
                SeekerService = new FourNeighborSeeker(this);
            }
            else if (GridNeighborType == GridNeighborType.Eight)
            {
                SeekerService = new EightNeighborSeeker(this);
            }
        }

        void InitBarrier()
        {
            if (GridNeighborType == GridNeighborType.Four)
            {
                BarrierService = new FourNeighborBarrier(this);
            }
            else if (GridNeighborType == GridNeighborType.Eight)
            {
                BarrierService = new FourNeighborBarrier(this);
            }
        }

        public Node[] GetNodes(Node node, int xSize, int zSize)
        {
            Node[] nodes = new Node[xSize * zSize];
            GetNodes(node,xSize,zSize,ref nodes);
            return nodes;
        }

        public void GetNodes(Node node ,int xSize,int zSize,ref Node[] nodes)
        {
            for (int i = 0; i < xSize; i++)
            {
                for (int j = 0; j < zSize; j++)
                {
                    nodes[i + j * xSize] = GetNode(node.X + i, node.Z + j);
                }
            }
        }
        [System.Obsolete]
        public Vector3Int GetOffset(int xSize, int zSize)
        {
            return new Vector3Int(xSize / 2, 0, zSize / 2);
        }
        //ポジションによってグリードからノード（マス）を取る
        public Node GetNode(Vector3Int pos)
        {
            return GetNode(pos.x, pos.z);
        }
        
        public Node GetNode(int xIndex, int zIndex)
        {
            if (HasNode(xIndex,zIndex))
            {
                return Nodes[xIndex, zIndex];
            }
            return null;
        }

        public bool HasNode(int xIndex,int zIndex)
        {
            if (xIndex >= 0 && xIndex < XCount && zIndex >= 0 && zIndex < ZCount)
            {
                return true;
            }
            return false;
        }
        //近距離攻撃判断為
        [System.Obsolete]
        public bool IsNeighborhood(Node sourceNode,int sourceXSize,int sourceZSize,Node targetNode,int targetXSize,int targetZSize)
        {
            int maxX0 = sourceNode.X + (sourceXSize);
            int maxZ0 = sourceNode.Z + (sourceZSize);
            int minX0 = sourceNode.X - 1;
            int minZ0 = sourceNode.Z - 1;

            int maxX1 = targetNode.X + (targetXSize - 1);
            int maxZ1 = targetNode.Z + (targetZSize - 1);
            int minX1 = targetNode.X;
            int minZ1 = targetNode.Z;

            if (minX0 > maxX1 || minX1 > maxX0)
            {
                return false;
            }
            if (minZ0 > maxZ1 || minZ1 > maxZ0)
            {
                return false;
            }
            return true;
        }

        public static bool IsNodeValid(Node node, GridLayerMask mask)
        {
            return node != null && !node.IsBlock && !node.IsTempBlock && mask.ContainLayer(1 << node.LayerMask);
        }
        public static bool IsNodeValid(Node node, GridLayerMask mask,GridLayerMask subMask)
        {
            return node != null && !node.IsBlock && !node.IsTempBlock && (mask.ContainLayer(1 << node.LayerMask) || subMask.ContainLayer(1 << node.SubLayerMask));
        }
        public static bool IsNodeValid(Node node)
        {
            return node != null && !node.IsBlock && !node.IsTempBlock;
        }
        
        void ResetSearchIndex(int index)
        {
            for (int i = 0; i < XCount; i++)
            {
                for (int j = 0; j < ZCount; j++)
                {
                    Nodes[i, j].IsClose = index;
                    Nodes[i, j].IsOpen = index;
                    Nodes[i, j].FirePowerUpdateCount = index;
                }
            }
        }

        public void DrawNodeInfos()
        {
            if (SnapshotService!=null && SnapshotService.IsShowSnapshot)
            {
                SnapshotService.DrawSnapshotNodeInfos();
            }
            else
            {
                NodesDebuger.DrawNodeInfos();
            }
        }
    }
}
