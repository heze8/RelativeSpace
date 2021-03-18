using PlayerActions;
using UnityEngine;

namespace BattleCore
{
    public abstract class BattleUnit : ScriptableObject, IBattleOccupant
    {
        public Team team;
        public BattleAction action;
        public bool isDead;
        protected BattleMap map;
        public Vector2Int pos;
        public float attackRange;
        public float hp;
        public float damage;
        public float speed;
        public Sprite sprite;

        public void StartBattle(BattleMap map)
        {
            this.map = map;
        }

        public BattleUnit(int team)
        {
            this.team = new Team(team);
        }
        public abstract void DoAction();

        public Sprite GetSprite()
        {
            return sprite;
        }
    }
    [CreateAssetMenu]

    public class BasicUnit : BattleUnit
    {
        public BasicUnit(int team) : base(team)
        {
        }

        public override void DoAction()
        {
            BattleUnit enemy = map.FindClosestEnemyUnit(this);
            if (enemy == null) return;
            
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

    [System.Serializable]
    public abstract class BattleAction 
    {
        public abstract void DoAction(BattleUnit unit, BattleUnit enemy);

    }

    [System.Serializable]
    public class IdleAttack : BattleAction
    {
        public override void DoAction(BattleUnit unit, BattleUnit enemy)
        {
            enemy.hp -= unit.damage;
            if (enemy.hp <= 0)
            {
                enemy.isDead = true;
            }
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

        public Team(int team)
        {
            this.teamIndex = team;
        }

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