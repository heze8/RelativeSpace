using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace BattleCore
{
    [CreateAssetMenu]

    public class BasicUnit : BattleUnit
    {
        public void OnEnable()
        {
            action = new IdleAttack();
        }

        public override void BehaviourTick()
        {
            BattleUnit enemy = map.FindClosestEnemyUnit(this);
            if (enemy == null) return;
            
            if (map.WithinRange(this, enemy))
            {
                action.DoAction(this, enemy);
                Debug.Log("Attacking");
            }
            else
            {
                Debug.Log("Moving");

                MoveTowards(enemy);
            }
        }

        private void MoveTowards(BattleUnit enemy)
        {
            var direc = enemy.pos - pos;
            int movement = (int) speed;
            movement += Random.value < (speed % 1)? 0: 1;
            var move =   movement * direc;
            if (move.sqrMagnitude > direc.sqrMagnitude)
            {
                pos = enemy.pos - new Vector2Int( (int) (direc.x / direc.magnitude), (int) (direc.y / direc.magnitude));
            }
            else
            {
                pos += move;
            }
        }
    }
}