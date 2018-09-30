﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SegmentFactory  {

    public static ISegment MakePoints(int segmentNum, PathMovement shape)
    {
        if (shape == PathMovement.square) {
            if (segmentNum == 2)
                {
                    return new TwoSegments();
                }
            else if (segmentNum == 3)
                {
                    return new ThreeSegments();
                }
            else
                {
                    return new FourSegments();
                }
        } else if (shape == PathMovement.curve) {
            if (segmentNum == 1) {
                return new OneSegmentCircle();
            }
            else if (segmentNum == 2)
                {
                    return new TwoSegmentsCircle();
                }
            else if (segmentNum == 3)
                {
                    return new ThreeSegmentsCircle();
                }
            else
                {
                    return new FourSegmentsCircle();
                }
        }
        else
        {
            return null;
        }
       
    }
}