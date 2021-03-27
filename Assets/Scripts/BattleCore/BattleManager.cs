using BattleCore;
using UnityEngine;

public class BattleManager : Singleton<BattleManager>
{
    public float tickTime = 0.1f;
    public float mapScale = 2f;
    public Battle existingBattle;

    public static Vector3 ConvertUnitPos(Vector2Int pos)
    {
        return Instance.mapScale * new  Vector3(pos.x, 0,pos.y);
    }

    public static BattleUnit CreateBattleUnit<T>(T unit, Team team, BattleAction unitAction) where T : BattleUnit
    {
        var battleUnit = Instantiate(unit);
        battleUnit.name = $"{Random.value:0.#}";
        battleUnit.team = team;
        
        battleUnit.action = unitAction;
        return battleUnit;
    }

}