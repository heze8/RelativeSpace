using PlayerActions;
using UnityEngine;

namespace BattleCore
{
    public abstract class BattleUnit : IBattleOccupant
    {
        public Team team;
        public BattleAction action;
        public bool isDead;
        protected BattleMap map;
        public Vector2Int pos;
        public float attackRange;


        public void StartBattle(BattleMap map)
        {
            this.map = map;
        }


        public abstract void DoAction();
  
    }

    public class BasicUnit : BattleUnit
    {
        public override void DoAction()
        {
            BattleUnit enemy = map.FindClosestEnemyUnit(this);
            if (enemy == null) ;
            if (map.WithinRange(this, enemy))
            {
                Attack(enemy);
                
            }
            
            

        }
        
    }

    public abstract class BattleAction
    {
        public abstract void DoAction(BattleUnit unit, BattleMap map);

    }

    public class IdleAttack : BattleAction
    {
        public override void DoAction(BattleUnit unit, BattleMap map)
        {
            BattleUnit enemy = map.FindClosestEnemyUnit(unit);
            if (enemy == null) ;
            
            


        }
    }

    public class BattleMap
    {
        public IBattleOccupant[,] map;
        public Vector2Int size;
        public bool noValidEnemies;
        public BattleMap(Vector2Int size)
        {
            this.size = size;
            map = new IBattleOccupant[size.x, size.y];
        }
        
        public BattleUnit FindClosestEnemyUnit(BattleUnit unit)
        {
            for (int x = 0; x <  size.x; x ++)
            {
                for (int y = 0; y <  size.y; y ++)
                {
                    if (unit != map[x, y])
                    {
                        if (unit.team.IsEnemy(map[x, y]) )
                        {
                            return unit;
                        }
                    }
                }
            }

            noValidEnemies = true;
            return null;
            //find closest pair of points algorithm.
        }

        public bool WithinRange(BattleUnit unit, BattleUnit enemy)
        {
            return ((unit.pos - enemy.pos).magnitude < unit.attackRange);

        }
    }

    public interface IBattleOccupant
    {
    }

    public class Team
    {
        public int teamIndex;

        public bool IsEnemy(IBattleOccupant battleOccupant)
        {
            BattleUnit attempt = (BattleUnit) battleOccupant;
            if (attempt != null)
            {
                return teamIndex != attempt.team.teamIndex;
            }

            return false;
        }
    }
}