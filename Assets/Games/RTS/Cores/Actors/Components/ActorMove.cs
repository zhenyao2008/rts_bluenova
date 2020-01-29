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
                BlockNodes();
            mFixedPointMoveAgent = PathFindingMananger.Single.CreateMoveAgent(mActorCore.transform, actorCore.actorAttribute.runSpeed / 100);

        }

        public void MoveTo(FixedPointVector3 fixedPointVector3, PathMoveAction onComplete)
        {
            if (mActorCore.actorAttribute.isBuilding==0) 
                mFixedPointMoveAgent.SetDestination(fixedPointVector3, onComplete);
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
                    BuildManager.Instance.UpdateNodes(node1);
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
                    BuildManager.Instance.UpdateNodes(node1);
                }
            }
            BuildManager.Instance.ApplyColors();
        }

        public void OnUpdate()
        {

        }
    }
}
