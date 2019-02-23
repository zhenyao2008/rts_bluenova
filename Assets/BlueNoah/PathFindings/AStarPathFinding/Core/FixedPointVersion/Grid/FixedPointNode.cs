using System.Collections.Generic;
using BlueNoah.Math.FixedPoint;
namespace BlueNoah.PathFinding.FixedPoint
{
    public class FixedPointNode
    {

        public static FixedPoint64 PREDICT_CONSUME_PLUS = 2;

        public static FixedPoint64 USED_CONSUME_PLUS = 99;

        public FixedPointGrid grid;

        public int index;

        public int x;
        //ノードの位置
        public int z;
        //ノードの位置
        public FixedPoint64 G;
        //スタート distance.
        public FixedPoint64 H;
        //Manhattan distance  曼哈顿算法
        public FixedPoint64 F;

        public List<FixedPointNode> neighbors = new List<FixedPointNode>();
        //このノードに接続(せつぞく)するノード。
        public List<FixedPoint64> consumes = new List<FixedPoint64>();

        public FixedPoint64 consumeRoadSizePlus;

        public FixedPoint64 consumeUsedPlus;

        //このノードから接続ノードまで、移動消費(いどうしょうひ)コスト。
        bool mIsBlock;
        //マースを使えからし。
        bool mIsEnable;
        //壁中にいるのかどうか,グリードの辺
        public bool isWallSide;
        public int blockNeighborCount;
        //world position.
        public FixedPointVector3 pos;
        //ゲーム世界のポジション。
        public FixedPointNode previous;
        //用以下两个参数来判断是否在open close list，这样就可以不用hashset contain方法了，这样更快。（100*100的grid情况下从8ms降低6ms）
        //用int自增，这样就不用在第二次开始的时候reset这两个值了
        public long isOpen;
        public long isClose;

        public long isOpenFront;
        public long isOpenBack;

        public long isCloseFront;
        public long isCloseBack;

        public bool isBridge;
        //当前不能通过，但是路径检查不受影响。
        public bool isTempBlock;

        public FixedPointMoveAgent tempBlockMoveAgent;

        public FixedPointBridge bridge;

        public int unitCount;

        public FixedPointMoveAgent moveAgent;

        public bool IsBlock
        {
            get
            {
                return mIsBlock;
            }
            set
            {
                mIsBlock = value;
            }
        }

        public bool Enable
        {
            get
            {
                return mIsEnable;
            }
            set
            {
                mIsEnable = value;
            }
        }

    }
}
