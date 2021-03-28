using System;
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
        protected float baseHp;
        public float hp;
        public float damage;
        public float speed;
        public Sprite sprite;
        public Vector2 directionFacing;
        //public Strategies?
        public void StartBattle(BattleMap map)
        {
            this.map = map;
            this.isDead = false;
        }

        public void OnEnable()
        {
            baseHp = hp;
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
            var unitPos = BattleManager.Instance.existingBattle.GetUnitPosition(unit);
            var enemyPos = BattleManager.Instance.existingBattle.GetUnitPosition(enemy);
            
            var attackEffectGO = BattleManager.Instantiate(attackEffect, unitPos, Quaternion.identity);
            // attackEffectGO.transform.LookAt(enemyPos, Vector3.down);
            var lineRenderer = attackEffectGO.GetComponent<LineRenderer>();
            lineRenderer.SetPosition(0, unitPos);
            lineRenderer.SetPosition(1, enemyPos);

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

        /*
         * Returns a value between 0 and 1 representing the density of enemies or teammates near this pos.
         */
        public float TeamDensity(BasicUnit b, bool returnSameTeam)
        {
            var pos = b.pos;
            var team = b.team;
            
            var val = 0.0f;
            for (int i = 1; i < 10; i++)
            {
                for (int x = -1; x < 2; x++)
                {
                    for (int y = -1; y < 2; y++)
                    {
                        if (x == 0 && y == 0) continue;
                        var checkPos = new Vector2Int(pos.x + x * i, pos.y + y * i);
                        if(OOB(checkPos)) continue;

                        if (returnSameTeam)
                        {
                            if (!team.IsEnemy(Get(checkPos)))
                            {
                                val += (10 - i);
                            }
                        }
                        else
                        {
                            if (team.IsEnemy(Get(checkPos)))
                            {
                                val += (10 - i);
                            }
                        }
                        
                    }
                }
            }
            if(val > 360) Debug.LogError("team density not working");
            return val / 360.0f;
        }

        public IBattleOccupant Get(Vector2Int pos)
        {
            return map[pos.x, pos.y];
        }

        public bool AttemptRetreat(BasicUnit basicUnit)
        {
            throw new NotImplementedException();
        }
    }

    public interface IBattleOccupant
    {
    }
}