using UnityEngine;
 
///summary
///summary
public class CameraController : MonoBehaviour
{
 
    #region Public Fields

    public GameObject target;
    public float scrollSenstivity;
    #endregion
 
    #region Unity Methods
 
    void Start()
    {
	
    }
 
    void Update()
    {
		gameObject.transform.LookAt(target.transform);
		var position = gameObject.transform.position;
		Camera.main.orthographicSize -= Input.mouseScrollDelta.y * scrollSenstivity;
    }
 
    #endregion
 
    #region Private Methods
    #endregion
}