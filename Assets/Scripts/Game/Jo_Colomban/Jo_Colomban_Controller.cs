using Game;
using Game.Actions;
using Graphs;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using static UnityEngine.GraphicsBuffer;

namespace Jo_Colomban
{
    public class Jo_Colomban_Controller : HeroController {
        [SerializeField] private float healingThreshold = 3f;

        public override ControllerAction Think()
        {
            if(HP <= healingThreshold && Heart.AllHearts.Count != 0) {
                Heart closestHeart = FindClosestMBFromList(Heart.AllHearts);
                return new Action_MoveTowards(this, closestHeart.gameObject.transform.position);
            }
            if(EnemyController.AllEnemies.Count != 0) {
                EnemyController closestEnemyController = FindClosestMBFromList(EnemyController.AllEnemies);
                if(IsNeighbor(closestEnemyController)) {
                    ControllerAction attackAction = EvaluateAttack(closestEnemyController);
                    return attackAction;
                }
                else
                    return new Action_MoveTowards(this, closestEnemyController.gameObject.transform.position);
            }
            if(HP < MaxHP) {
                Heart closestHeart = FindClosestMBFromList(Heart.AllHearts);
                return new Action_MoveTowards(this, closestHeart.gameObject.transform.position);
            }

            return new Action_Wait(this, 1.0f);
        }

        private ControllerAction EvaluateAttack(EnemyController closestEnemyController) {
            return new Action_Attack(this, closestEnemyController);
            //TODO:check for sparta kick
        }

        private T FindClosestMBFromList<T>(List<T> monoBehaviours) where T : MonoBehaviour { //ugly but will do
            if(monoBehaviours.Count == 0)
                return null;

            T currentClosestMonoBehaviour = monoBehaviours[0];
            foreach(T mb in monoBehaviours) {
                // this should use GetShortestPath instead!
                if(Vector3.Distance(transform.position, mb.gameObject.transform.position) < Vector3.Distance(transform.position, currentClosestMonoBehaviour.gameObject.transform.position))
                    currentClosestMonoBehaviour = mb;
            }

            return currentClosestMonoBehaviour;
        }
    }
}
