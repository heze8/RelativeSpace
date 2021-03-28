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
            var retreatChance = (baseHp - hp) / baseHp;
            retreatChance *= retreatChance;
            Random.InitState(this.GetHashCode());
            var retreat = Random.value < retreatChance;
            if (retreat && map.TeamDensity(this, false) < 0.3f && map.TeamDensity(this, true) < 0.8f)
            {
                map.AttemptRetreat(this);
            }
            BattleUnit enemy = map.FindClosestEnemyUnit(this);
            if (enemy == null) return;
            
        
            directionFacing = enemy.pos - pos;
            
            if (map.WithinRange(this, enemy))
            {
                action.DoAction(this, enemy);
            }
            else
            {

                MoveTowards(enemy);
            }
        }

        private void MoveTowards(BattleUnit enemy)
        {
            var direc = (Vector2) (enemy.pos - pos);
            //Converts decimal into int using probability
            int movement = Mathf.FloorToInt(speed);
            movement += Random.value < (speed % 1)? 0: 1;
            
            //
            
            
            Vector2Int unitDirec = Vector2Int.RoundToInt(direc.normalized);
            var move =   movement * unitDirec;


            if (move.sqrMagnitude == 0)
            {
                return;
            }
            if (move.sqrMagnitude >= direc.sqrMagnitude)
            {
                var vector2Int = enemy.pos - unitDirec;
                var canMove = map.Move(this,
                    vector2Int);
                if (!canMove)
                {
                    Debug.Log("can't move "+ enemy.pos);
                }
            }
            else
            {
                map.Move(this, move + new Vector2Int(Random.Range(-1,2),Random.Range(-1,2)));
            }
        }
    }
}