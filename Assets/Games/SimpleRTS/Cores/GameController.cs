using System.Collections.Generic;
using UnityEngine.UI;

namespace BlueNoah.SimpleRTS
{
    public class GameController : SimpleSingleMonoBehaviour<GameController>
    {
        public Button StartButton;

        protected override void Awake()
        {
            base.Awake();
            StartButton.onClick.AddListener(() =>
            {
                foreach (List<Actor> actors in SpawnManager.Instance.Actors.Values)
                {
                    foreach (Actor actor in actors)
                    {
                        actor.Begin();
                    }
                }
            });
        }

        private void Update()
        {
            
        }


    }
}
