using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct Segment {
    public Vector3 prevPos;
    public Vector3 pos;

    public Segment(Vector3 pos)
    {
        prevPos = pos;
        this.pos = pos;
    }

    public Segment(Segment segment)
    {
        pos = segment.pos;
        prevPos = segment.prevPos;
    }
}