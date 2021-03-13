namespace BattleCore
{
    public abstract class BattleUnit : IBattleOccupant
    {
        public Team team;
        public BattleAction action;
        
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
            
        }
    }

    public class BattleMap
    {
        public IBattleOccupant[,] map;
        public BattleUnit FindClosestEnemyUnit(BattleUnit unit)
        {
            //find closest pair of points algorithm.
        }
    }

    public interface IBattleOccupant
    {
    }

    public class Team
    {
        public int teamIndex;
    }
}