using System.Collections;
using System.Collections.Generic;
using BattleCore;
using UnityEngine;
 
///summary
///summary
public class Battle 
{
 
    #region Public Fields

    private List<BattleUnit> battleUnits;
    private List<List<BattleUnit>> teams;
    private BattleMap _battleMap;
    private bool hasGivenUp;

    #endregion
 

    #region Private Methods

    public void BeginWar(Vector2Int size, List<BattleUnit> battleUnits)
    {
        this.battleUnits = battleUnits;
	    _battleMap = new BattleMap(size);
        
        foreach (var unit in battleUnits)
        {
            unit.StartBattle();
        }

        BattleManager.Instance.StartCoroutine(UnitActions());
    }

    public IEnumerator UnitActions()
    {
        while (!HasWarEnded())
        {
            foreach (var unit in battleUnits)
            {
                unit.DoAction();
            }
            yield return new WaitForSeconds(BattleManager.Instance.tickTime);
        }
    }

    private bool HasWarEnded()
    {
        if (hasGivenUp || _battleMap.noValidEnemies)
        {
            return true;
        }
        foreach (var team in teams)
        {
            bool teamAlive = false;
            foreach (var unit in team)
            {
                if (!unit.isDead)
                {
                    teamAlive = true;
                    break;
                }
            }

            if (!teamAlive)
            {
                return true;
            }
        }

        return false;
    }

    #endregion
}

public class BattleManager : Singleton<BattleManager>
{
    public float tickTime = 0.1f;

}