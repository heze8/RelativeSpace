namespace BattleCore
{
    public abstract class BattleUnit
    {
        public Team team;
        public BattleAction action;
        
    }

    public abstract class BattleAction
    {
        public abstract void DoAction(BattleUnit unit, BattleMap map);

    }

    public class BattleMap
    {
    }

    public class Team
    {
        public int teamIndex;
    }
}