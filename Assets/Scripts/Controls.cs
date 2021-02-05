using UnityEngine;
 
///summary
///summary
public class Controls : MonoBehaviour
{
	private Rigidbody2D rb;
	public float speed = 0.5f;
	private Camera _camera;

	#region Public Fields
    #endregion
 
    #region Unity Methods
 
    void Start()
    {
	    _camera = Camera.main;
	    rb = GetComponent<Rigidbody2D>();
    }
 
    void Update()
    {
	    Vector2 target = _camera.ScreenToWorldPoint(Input.mousePosition);
	    var direction = target - (Vector2) transform.position;
	    rb.velocity = direction * speed;
    }
 
    #endregion
 
    #region Private Methods
    #endregion
}