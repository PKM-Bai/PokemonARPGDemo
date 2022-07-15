using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MyPokemon.Transition
{
    public class Teleport : MonoBehaviour
    {
        [SceneName]
        public string sceneToGo;
        public TeleportType targetScene;

        public Vector3 positionToGo;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                EventHandler.CallTransitionEvent(sceneToGo, positionToGo, targetScene);
            }
        }

        public void ToSwitchScene()
        {
            EventHandler.CallTransitionEvent(sceneToGo, positionToGo, targetScene);
        }
    }
}

