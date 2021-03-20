using System;
using System.Collections;
using System.Collections.Generic;
using BattleCore;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

///summary
///summary
public class Battle : MonoBehaviour
{
 
    #region Public Fields

    public GameObject unitPrefab;
    private List<BattleUnit> battleUnits;
    private List<GameObject> renderBattleUnits;
    private List<List<BattleUnit>> teams;
    private BattleMap _battleMap;
    private bool hasGivenUp;
    private float timeSinceLast;
    private bool warStarted;
    public BattleCore.BasicUnit basicUnit;

    #endregion
 

    #region Private Methods

    public void BeginWar(Vector2Int size, List<BattleUnit> battleUnits)
    {
        this.battleUnits = battleUnits;
	    _battleMap = new BattleMap(size);
        
        Dictionary<int, int> hasTeams = new  Dictionary<int, int>();
        teams = new List<List<BattleUnit>>();
        
        foreach (var unit in battleUnits)
        {
            unit.StartBattle(_battleMap);
            int team = unit.team.teamIndex;
            
            if (!hasTeams.ContainsKey(team))
            {
                hasTeams.Add(team, teams.Count);
                var units = new List<BattleUnit> {unit};
                teams.Add(units);
            }
            else
            {
                var units = teams[hasTeams[team]];
                units.Add(unit);
            }
            
        }
        
        var offset = 0;
        var x = 0;
        var y = 0;
        //init map and teams
        foreach (var team in teams)
        {
            foreach (var unit in team)
            {
                unit.pos = new Vector2Int(x + offset, y);
                y = (y + 1) % _battleMap.size.y;
                _battleMap.map[unit.pos.x, unit.pos.y] = unit;
            }

            offset += 5;
        }
        
        
        
        
        BattleManager.Instance.StartCoroutine(UnitActions());
        warStarted = true;
    }

    public void Start()
    {
        var units = new List<BattleUnit>();
        for (int i = 0; i < 10; i++)
        {
            int team = Random.value < 0.5f ? 0 : 1;
            var battleUnit = Instantiate(basicUnit);
            battleUnit.team = new Team(team); //might want to change to team manager
            units.Add(battleUnit);
        }
        BeginWar(new Vector2Int(20,20), units);
        InitRender();

    }

    private void InitRender()
    {
        renderBattleUnits = new List<GameObject>();
        
        foreach (var unit in battleUnits)
        {
            var sprite = unit.GetSprite();
            var go = Instantiate(unitPrefab, ConvertUnitPos(unit.pos), Quaternion.identity, transform);
            go.GetComponent<SpriteRenderer>().sprite = sprite;
            renderBattleUnits.Add(go);
        }
    }

    public void Update()
    {
        if(warStarted)
            RenderBattleField();
    }

    private void RenderBattleField()
    {
        var fracTickTime = (Time.deltaTime / BattleManager.Instance.tickTime);
        for(int i = 0; i< battleUnits.Count; i++)
        {
            
            var unit = battleUnits[i];
            
            var render = renderBattleUnits[i];
            if (unit.isDead)
            {
                if(render)
                    Destroy(render);
                continue;
                
            }
            render.transform.position = ConvertUnitPos(unit.pos);
            
            //fracTickTime * (ConvertUnitPos(unit.pos) - render.transform.position);
        }
    }

    private Vector3 ConvertUnitPos(Vector2Int pos)
    {
        return new Vector3(pos.x, 0,pos.y);
    }

    public IEnumerator UnitActions()
    {
        var i = 0;
        while (!HasWarEnded())
        {
            Debug.Log("tick " + i++);

            foreach (var unit in battleUnits)
            {
                unit.BehaviourTick();
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