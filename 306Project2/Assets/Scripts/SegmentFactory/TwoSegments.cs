﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwoSegments : ISegment
{
     public List<Vector2> CalculatePoints(Vector2 origonalPos, Vector2 endPos, bool clockwise)
    {
        List<Vector2> rectanglePoints = new List<Vector2>();

        Vector2 secondPos;
        Vector2 fourthPos;
        if (clockwise)
        {
            secondPos = new Vector2(origonalPos.x, endPos.y);
            fourthPos = new Vector2(endPos.x, origonalPos.y);

        }
        else
        {
            secondPos = new Vector2(endPos.x, origonalPos.y);
            fourthPos = new Vector2(origonalPos.x, endPos.y);
        }

        //Important ordering of adding to rectangle
        rectanglePoints.Add(secondPos);
        rectanglePoints.Add(endPos);
        rectanglePoints.Add(secondPos);
        rectanglePoints.Add(origonalPos);

        return rectanglePoints;
    }
}
