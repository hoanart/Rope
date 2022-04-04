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

  public Segment(Vector3 pos, Vector3 prevPos)
  {
      this.pos = pos;
      this.prevPos = prevPos;
  }
  // public static Segment  SetPreviousPosition(Segment segment)
  // {
  //     var temp = segment;
  //     temp.prevPos = segment.prevPos;
  //     segment = temp;
  //     return segment;
  // }
  public Segment  SetPreviousPosition()
  {
      return new Segment(this.pos, prevPos);

  }
 public static Segment SetPosition(Segment segment,Vector3 pos)
 {
     var temp = segment;
     temp.pos = pos;
     segment = temp;
     return segment;
 }

 public static Segment UpdatePosition(Segment segment,Vector3 velocity)
 {
     var temp = segment;
     temp.pos = segment.pos + velocity;
     segment = temp;
     return segment;
 }

 
}
