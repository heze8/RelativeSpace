using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;

public class TalkingMiniGame
{
    public int width;
    public int height;
    public List<List<IdeaSquare>> gameGrid;

    public class IdeaSquare
    {
        public Idea parent;

        public bool IsEmpty()
        {
            return parent == null;
        }
        public IdeaSquare(Idea parent)
        {
            this.parent = parent;
        }
    }
    
    public void StartGame(Idea startingIdea)
    {
        width = 0;
        height = 0;
        gameGrid = new List<List<IdeaSquare>>();
        
        for (int i = 0; i < 3; i++)
        {           
            ExtendYGrid();

            ExtendXGrid(false);
        }
        Place(startingIdea, 0, 0);
    }

    public bool NextTurn(Idea idea, int x, int y)
    {
        if (Check(idea, x, y))
        {
             Place(idea, x, y);
             return true;
        }
        else
        {
            return false;
        }
    }

    public bool Check(Idea idea, int x, int y)
    {
        //check if x is negative
        if (x < 0)
        {
            for (int i = 0; i < 9; i++)
            {
                if (idea.shape.shape[i])
                {
                    int newX = x + i % 3;
                    if (newX < 0)
                    {
                        continue;
                    }
                    int newY = y + (int) Math.Floor((double) i / 3) - 1;
                    if (!gameGrid[newX][newY].IsEmpty())
                    {
                        return false;
                    }
                }
            }
        }
        //check if idea occupies any existing space
        else
        {
            for (int i = 0; i < 9; i++)
            {
                if (idea.shape.shape[i])
                {
                    int newX = x + i % 3;
                    int newY = y + (int) Math.Floor((double) i / 3) - 1;
                    if (!gameGrid[newX][newY].IsEmpty())
                    {
                        return false;
                    }
                }
            }
        }

        return true;
    }

    public void ExtendXGrid(bool beforeOrigin)
    {
        width++;
        for (int i = 0; i < height; i++)
        {
            if(beforeOrigin)
                gameGrid[i].Insert(0,new IdeaSquare(null));
            else
                gameGrid[i].Add(new IdeaSquare(null));
        }
    }

    public void ExtendYGrid()
    {
        height++;
        var emptyXlist = new List<IdeaSquare>();
        for (int i = 0; i < width; i++)
        {
            emptyXlist.Add(new IdeaSquare(null));
        }
        
        gameGrid.Add(emptyXlist);
    }
    
    
    public void Place(Idea idea, int x, int y)
    {
        // List<Tuple<int, int>> coordinates = new List<Tuple<int, int>>();
        if (x < 0)
        {
            ExtendXGrid(true);
        }
        for (int i = 0; i < 9; i++)
        {
            if (idea.shape.shape[i])
            {
                int newX = x + i % 3;
                int newY = y + (int) Math.Floor(((double) i) / 3.0);
                
                //checking if out of bounds
                if (height <= newY)
                {
                    ExtendYGrid();
                }

                if (width <= newX)
                {
                    ExtendXGrid(false);
                }
                Debug.Log(newX+" "+ newY  + " " + i);
                Debug.Log(gameGrid.Count); //placing
                gameGrid[newX][newY] = new IdeaSquare(idea);
            }

        }

        // return coordinates;

    }
}