using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace BattleCore
{
    [CreateAssetMenu]

    public class BasicUnit : BattleUnit
    {
        
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
            Vector2Int unitDirec = new Vector2Int(Mathf.CeilToInt(direc.x / direc.magnitude), Mathf.CeilToInt((direc.y / direc.magnitude)));
            var move =   movement *unitDirec;
            Debug.Log("move: " + move);
            directionFacing = unitDirec;
            if (move.sqrMagnitude > direc.sqrMagnitude)
            {
                var vector2Int =   enemy.pos - unitDirec;
                Debug.Log(vector2Int);
                map.Move(this,
                    vector2Int);
            }
            else
            {
                map.Move(this, move);
            }
        }
    }
}