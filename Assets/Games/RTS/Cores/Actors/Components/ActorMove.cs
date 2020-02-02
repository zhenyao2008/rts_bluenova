using BlueNoah.Build;
using BlueNoah.Math.FixedPoint;
using BlueNoah.PathFinding;
using BlueNoah.PathFinding.FixedPoint;
using TD.Config;
using UnityEngine;

namespace BlueNoah.AI.RTS
{
    //主に移す行為が三つある：WayPoint,Grid,Navmesh。
    //これでGridを使っている。
    public class ActorMove
    {
        FixedPointMoveAgent mFixedPointMoveAgent;

        public FixedPointMoveAgent FixedPointMoveAgent
        {
            get
            {
                return mFixedPointMoveAgent;
            }
        }

        ActorCore mActorCore;

        public ActorMove(ActorCore actorCore)
        {
            this.mActorCore = actorCore;
            if(mActorCore.actorAttribute.isBuilding != 0)
            {
                Debug.Log(mActorCore.actorAttribute.isWall);
                if(mActorCore.actorAttribute.isWall == 0)
                {
                    BlockNodes();
                }
                else
                {
                    SetNodeHeight(actorCore.actorAttribute.wallHeight, actorCore.actorAttribute.isStair, 6);
                }
            }
            mFixedPointMoveAgent = PathFindingMananger.Single.CreateMoveAgent(mActorCore.transform, actorCore.actorAttribute.runSpeed / 100);

        }

        public void MoveTo(FixedPointVector3 fixedPointVector3, PathMoveAction onComplete)
        {
            if (mActorCore.actorAttribute.isBuilding==0) 
                mFixedPointMoveAgent.SetDestination(fixedPointVector3, onComplete);
        }

        public void SetNodeHeight(int height,int isStair,int step )
        {
            FixedPointGrid grid = PathFindingMananger.Single.Grid;
            FixedPointNode node = grid.GetNode(mActorCore.transform.position);
            Debug.Log(node.x + ":" + node.z);
            for (int i = 0; i < mActorCore.actorAttribute.sizeX; i++)
            {
                for (int j = 0; j < mActorCore.actorAttribute.sizeZ; j++)
                {
                    FixedPointNode node1 = grid.GetNode(node.x + i, node.z + j);
                    if (isStair != 0)
                    {
                        node1.pos.y = new FixedPoint64(height) / step * (j + new FixedPoint64(1) / 2);
                    }
                    else
                    {
                        node1.pos.y = height;
                    }
                    BuildManager.Instance.UpdateNodesVertexs(node1);
                }
            }
            BuildManager.Instance.ApplyVertexs();
        }

        public void BlockNodes()
        {
            UnityEngine.Debug.Log("BlockNodes");
            FixedPointGrid grid = PathFindingMananger.Single.Grid;
            FixedPointNode node = grid.GetNode(mActorCore.transform.position);
            Debug.Log(node.x + ":" + node.z);
            for (int i = 0;i < mActorCore.actorAttribute.sizeX;i++)
            {
                for (int j = 0; j < mActorCore.actorAttribute.sizeZ; j++)
                {
                    FixedPointNode node1 = grid.GetNode(node.x + i,node.z + j);
                    node1.IsBlock = true;
                    BuildManager.Instance.UpdateNodesColor(node1);
                }
            }
            BuildManager.Instance.ApplyColors();
        }

        public void UnBlockNodes()
        {
            FixedPointGrid grid = PathFindingMananger.Single.Grid;
            FixedPointNode node = grid.GetNode(mActorCore.transform.position);
            for (int i = 0; i < mActorCore.actorAttribute.sizeX; i++)
            {
                for (int j = 0; j < mActorCore.actorAttribute.sizeZ; j++)
                {
                    FixedPointNode node1 = grid.GetNode(node.x + i, node.z + j);
                    node1.IsBlock = false;
                    BuildManager.Instance.UpdateNodesVertexs(node1);
                }
            }
            BuildManager.Instance.ApplyColors();
        }

        public void OnUpdate()
        {

        }
    }
}
