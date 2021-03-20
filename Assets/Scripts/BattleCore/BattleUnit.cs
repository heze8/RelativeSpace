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
            this.isDead = false;
        }
        
        public abstract void BehaviourTick();

        public Sprite GetSprite()
        {
            return sprite;
        }
    }

    [System.Serializable]
    public abstract class BattleAction : MonoBehaviour
    {
        public abstract void DoAction(BattleUnit unit, BattleUnit enemy);

    }

    [System.Serializable]
    public class IdleAttack : BattleAction
    {
        public override void DoAction(BattleUnit unit, BattleUnit enemy)
        {
            Instantiate(attackObj, Battle)
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
                    if (unit != (BattleUnit) map[x, y])
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
}