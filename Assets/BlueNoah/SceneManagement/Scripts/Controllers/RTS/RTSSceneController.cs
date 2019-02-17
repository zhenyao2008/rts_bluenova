using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlueNoah.SceneControl
{
    public class RTSSceneController : BaseSceneController<RTSSceneController>
    {
        ScreenSelectService mScreenSelectService;

        protected override void InitBuildingGrid()
        {
            //throw new System.NotImplementedException();
        }

        protected override void InitBuildings()
        {
            //throw new System.NotImplementedException();
        }

        protected override void InitCamera()
        {
            //throw new System.NotImplementedException();
        }

        protected override void InitEnviroment()
        {
            //throw new System.NotImplementedException();
        }

        protected override void InitInput()
        {
            //throw new System.NotImplementedException();
            mScreenSelectService = new ScreenSelectService();
        }

        protected override void InitSmallObjects()
        {
            //throw new System.NotImplementedException();
        }

        protected override void InitUI()
        {
            //throw new System.NotImplementedException();
        }

        protected override void InitUnitGrid()
        {
            //throw new System.NotImplementedException();
        }

        protected override void InitUnits()
        {
            //throw new System.NotImplementedException();
        }

        private void OnGUI()
        {
            mScreenSelectService.OnGUI();
        }
    }

}

