using BattleCore;
using PlayerActions;
using UnityEngine;

namespace BattleCore
{
    public abstract class BattleUnit : ScriptableObject, IBattleOccupant
    {
        public string name;
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
        public Vector2 directionFacing;

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

        public void Died()
        {
            isDead = true;
            map.map[pos.x, pos.y] = null;
        }

        public override string ToString()
        {
            return name + " " +base.ToString();
        }
    }

    [System.Serializable]
    public abstract class BattleAction 
    {
        public GameObject attackEffect;
        public abstract void DoAction(BattleUnit unit, BattleUnit enemy);

    }

    [System.Serializable]
    public class IdleAttack : BattleAction
    {
        public override void DoAction(BattleUnit unit, BattleUnit enemy)
        {
            var unitPos = BattleManager.ConvertUnitPos(unit.pos);
            var enemyPos = BattleManager.ConvertUnitPos(enemy.pos);
            var fromToRotation = Quaternion.FromToRotation(unitPos, enemyPos);
            Debug.Log(fromToRotation + " " + unitPos + " " + enemyPos);
            var attackEffectGO = BattleManager.Instantiate(attackEffect, unitPos, fromToRotation);
            enemy.hp -= unit.damage;
            
            if (enemy.hp <= 0)
            {
                enemy.Died();
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
                    var enemyUnit = (BattleUnit) map[x, y];

                    if (unit != enemyUnit)
                    {
                        if (unit.team.IsEnemy(enemyUnit))
                        {
                            return enemyUnit;
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

        public bool Move(BasicUnit basicUnit, Vector2Int move)
        {
            var basicUnitPos = basicUnit.pos + move;
            if (OOB(basicUnitPos)) return false;
            if (map[basicUnitPos.x, basicUnitPos.y] != null)
            {
                return false;
            }
            else
            {
                map[basicUnitPos.x, basicUnitPos.y] = basicUnit;
                map[basicUnit.pos.x, basicUnit.pos.y] = null;
                basicUnit.pos = basicUnitPos;
                return true;
            }
        }

        private bool OOB(Vector2Int basicUnitPos)
        {
            return basicUnitPos.x < 0 || basicUnitPos.x > size.x || basicUnitPos.y < 0 || basicUnitPos.y > size.y;
        }
    }

    public interface IBattleOccupant
    {
    }
}