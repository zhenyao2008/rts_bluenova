using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlueNoah.Control.Service
{

    public interface ServiceInterface
    {
        void OnAwake();

        void OnStart();

        void OnUpdate();

        void OnDestory();

    }

}

