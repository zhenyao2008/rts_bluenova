using System.Collections.Generic;
using BlueNoah.AI.Stage;
using BlueNoah.CSV;
using BlueNoah.Event;
using BlueNoah.SceneControl;
using UnityEngine;
using UnityEngine.EventSystems;

namespace BlueNoah.SimpleRTS
{
    public class SpawnManager : SimpleSingleMonoBehaviour<SpawnManager>
    {

        StageService mStageService;

        ScreenSelectService mScreenSelectService;

        public Dictionary<int, List<Actor>> Actors;

        protected override void Awake()
        {
            base.Awake();
            Init();
        }

        void Init()
        {
            mStageService = new StageService();
            mStageService.onSpawnActor = SpawnStageActor;
            mStageService.LoadStage(0);

            mScreenSelectService = new ScreenSelectService();

            mScreenSelectService.onSelected = OnSeleced;

            EasyInput.Instance.AddListener(BlueNoah.Event.TouchType.Click, 0, OnClick);

            EasyInput.Instance.AddListener(BlueNoah.Event.TouchType.Click, 1, OnRightClick);

            CameraControl.CameraController.Instance.MoveSpeed = TD.Config.InGameConfig.Single.cameraDragSpeed;
        }
        public void OnGUI()
        {
            mScreenSelectService.OnGUI();
        }
        void OnClick(EventData eventData)
        {
            if (!eventData.currentTouch.isPointerOnGameObject && (EventSystem.current == null || !EventSystem.current.IsPointerOverGameObject()))
            {
                /*
                UnSelectActors();
                RaycastHit raycastHit;
                if (BlueNoah.CameraControl.CameraController.Instance.GetWorldPositionByMousePosition(out raycastHit, LayerConstant.LAYER_GROUND))
                {
                    if (onCreateActor != null)
                    {
                        onCreateActor(playerId, 8, raycastHit.point, Vector3.zero);
                    }
                }*/
            }
        }

        void OnSeleced(Rect rect)
        {
            /*
            Dictionary<long, ActorViewer> actorViewers = RTSSceneController.Instance.GetActorViewers(playerId);
            UnSelectActors();
            if (actorViewers != null)
            {
                foreach (ActorViewer actorViewer in actorViewers.Values)
                {
                    if (mScreenSelectService.IsInSelectRect(rect, actorViewer.screenPosition.x, actorViewer.screenPosition.y))
                    {
                        Debug.Log(actorViewer);
                        mSelectedActors.Add(actorViewer.actorCore.actorAttribute.actorId, actorViewer);
                        actorViewer.gameObject.GetOrAddComponent<ActorHighlighter>().ShowHighlighter();
                    }
                }
            }*/
        }

        void OnRightClick(EventData eventData)
        {
            if (!eventData.currentTouch.isPointerOnGameObject && !UICamera.isOverUI && (EventSystem.current == null || !EventSystem.current.IsPointerOverGameObject()))
            {
                /*
                if (mSelectedActors.Count > 0)
                {
                    RaycastHit raycastHit;
                    if (BlueNoah.CameraControl.CameraController.Instance.GetWorldPositionByMousePosition(out raycastHit, LayerConstant.LAYER_GROUND))
                    {
                        //FixedPointNode fixedPointNode = PathFindingMananger.Single.Grid.GetNode(raycastHit.point.ToFixedPointVector3());
                        List<FixedPointVector3> fixedPointVectors = GetNearPositions(raycastHit.point.ToFixedPointVector3(), mSelectedActors.Count);
                        int index = 0;
                        foreach (ActorViewer actorViewer in mSelectedActors.Values)
                        {
                            if (actorViewer != null)
                            {
                                if (Input.GetKey(KeyCode.A))
                                {
                                    //TODO let action system to do this,can not 操る　actorCore by Player directly.
                                    actorViewer.actorCore.ActorAI.MoveTo(null, fixedPointVectors[index], true, true);
                                }
                                else
                                {
                                    //TODO let action system to do this,can not 操る　actorCore by Player directly.
                                    actorViewer.actorCore.ActorAI.MoveTo(null, fixedPointVectors[index], true, false);
                                }
                            }
                            index++;
                        }
                    }
                }*/
            }
        }

        Dictionary<string, GameObject> mCachedActor;

        void SpawnStageActor(MapMonster mapMonster)
        {
            if (mCachedActor == null)
            {
                mCachedActor = new Dictionary<string, GameObject>();
            }
            string path = mapMonster.ActorCSVStructure.resource_path;
            if (!mCachedActor.ContainsKey(path))
            {
                mCachedActor.Add(path, GetPrefab(path));
            }

            if (Actors==null)
            {
                Actors = new Dictionary<int, List<Actor>>();
            }

            GameObject go = GameObject.Instantiate(mCachedActor[path]);
           // go.AddMissingComponent<UnityEngine.AI.NavMeshAgent>();
            Vector3 position = new Vector3(mapMonster.pos_x / 1000f, 0, mapMonster.pos_y / 1000f);
            Vector3 eulerAngles = new Vector3(0, mapMonster.angle_y, 0);
            go.transform.position = position;
            go.transform.eulerAngles = eulerAngles;


            go.AddMissingComponent<Actor>().Init(mapMonster);

            if (!Actors.ContainsKey(mapMonster.alignment))
            {
                Actors.Add(mapMonster.alignment,new List<Actor>());
            }
            Actors[mapMonster.alignment].Add(go.GetComponent<Actor>());
        }

        GameObject GetPrefab(string path)
        {
            return Resources.Load<GameObject>(path);
        }

    }
}
