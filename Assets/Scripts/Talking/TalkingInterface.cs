using System;
using System.Collections.Generic;
using UnityEngine;
 
///summary
/// Interface to play talking minigame
///summary
public class TalkingInterface : Singleton<TalkingInterface>
{
 
    #region Public Fields

    private TalkingMiniGame _miniGameInstance;
    private Grid _grid;
    private Camera _cam;
    public Idea startingIdea;
    public GameObject ideaSprite;
    #endregion
 
    #region Unity Methods
 
    void Start()
    {
	    _cam = Camera.main;
	    _grid = gameObject.GetComponent<Grid>();
		_miniGameInstance = new TalkingMiniGame();
		_miniGameInstance.StartGame(startingIdea);
		Display(startingIdea, 0, 0, ideaSprite);
    }

    public void Display(Idea idea, int x, int y, GameObject ideaSprite)
    {
	    for (int i = 0; i < 9; i++)
	    {
		    if (idea.shape.shape[i])
		    {
			    int newX = x + i % 3;

			    int newY = y + (int) Math.Floor((double) i / 3) - 1;
			    var cell = Instantiate(ideaSprite, _grid.CellToWorld(new Vector3Int(newX, newY, 0)), Quaternion.identity, gameObject.transform);
		    }
	    }
    }
    
 
    void Update()
    {
	    if (Input.GetKeyDown(KeyCode.Mouse0))
	    {
		    var gridPos = _grid.GetCellCenterWorld(new Vector3Int());
		    var pos = _grid.WorldToCell(gridPos - _cam.ScreenToWorldPoint(Input.mousePosition));
		    
		    Debug.Log(pos);
		    if (_miniGameInstance.NextTurn(startingIdea, pos.x, pos.y))
		    {
			    Display(startingIdea, pos.x, pos.y , ideaSprite);
		    }
	    }
    }
 
    #endregion
 
    #region Private Methods
    #endregion
}