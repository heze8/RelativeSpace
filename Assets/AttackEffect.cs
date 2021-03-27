using BattleCore;
using UnityEngine;
 
///summary
///summary
public class AttackEffect : MonoBehaviour
{
 
    #region Public Fields

    public float timeAlive;
    #endregion
 
    #region Unity Methods
 
    void Start()
    {
	    timeAlive = BattleManager.Instance.tickTime;
		Destroy(gameObject, timeAlive);
    }
 
    void Update()
    {
	
    }
 
    #endregion
 
    #region Private Methods
    #endregion
}