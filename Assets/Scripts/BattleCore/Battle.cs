using System;
using System.Collections;
using System.Collections.Generic;
using BattleCore;
using UnityEngine;
 
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

    public void Start()
    {
        InitRender();
    }

    private void InitRender()
    {
        renderBattleUnits = new List<GameObject>();
        foreach (var unit in battleUnits)
        {
            var sprite = unit.GetSprite();
            var go = Instantiate(unitPrefab, ConvertUnitPos(unit.pos), Quaternion.identity);
            go.GetComponent<SpriteRenderer>().sprite = sprite;
            renderBattleUnits.Add(go);
        }
    }

    public void Update()
    {
        
        RenderBattleField();
    }

    private void RenderBattleField()
    {
        timeSinceLast = Time.time - timeSinceLast;

        for(int i = 0; i< battleUnits.Count; i++)
        {
            var unit = battleUnits[i];
            var render = renderBattleUnits[i];

            render.transform.position = timeSinceLast / BattleManager.Instance.tickTime *
                                        (ConvertUnitPos(unit.pos) - render.transform.position);
        }
    }

    private Vector3 ConvertUnitPos(Vector2Int pos)
    {
        return new Vector3(pos.x, pos.y);
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