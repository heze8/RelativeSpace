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
	    timeAlive = BattleManager.Instance.tickTime/2;
		Destroy(gameObject, timeAlive);
    }
 
    void Update()
    {
	
    }
 
    #endregion
 
    #region Private Methods
    #endregion
}