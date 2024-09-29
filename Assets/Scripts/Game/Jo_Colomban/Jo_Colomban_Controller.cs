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
            Heart closestHeart = FindClosestMBFromList(Heart.AllHearts);
            EnemyController closestEnemyController = FindClosestMBFromList(EnemyController.AllEnemies);

            if(HP <= healingThreshold && Heart.AllHearts.Count != 0) {
                if(IsNeighbor(closestEnemyController))
                    if(closestEnemyController.HP > 1)
                        return new Action_Kick(this, closestEnemyController);
                    else
                        return new Action_Attack(this, closestEnemyController);
                else
                    return new Action_MoveTowards(this, closestHeart.gameObject.transform.position);
            }

            if(EnemyController.AllEnemies.Count != 0) {
                if(IsNeighbor(closestEnemyController)) {
                    ControllerAction attackAction = EvaluateAttack(closestEnemyController);
                    return attackAction;
                }
                else if(HP < MaxHP && Heart.AllHearts.Count != 0) //this doesn't need the else, it's just there for readability
                    return new Action_MoveTowards(this, closestHeart.gameObject.transform.position);
                else
                    return new Action_MoveTowards(this, closestEnemyController.gameObject.transform.position);
            }

            if(Heart.AllHearts.Count != 0) { //head to the closest heart even if HP is full rather than waiting
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
