using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public struct Segment { 
    public Vector3 prevPos;
    public Vector3 pos;
    public Vector3 velocity;
  public Segment(Vector3 pos)
  {
    prevPos = pos;
    this.pos = pos;
    velocity = Vector3.zero;
  }

  public Segment(Vector3 pos, Vector3 prevPos,Vector3 velocity)
  {
      this.pos = pos;
      this.prevPos = prevPos;
      this.velocity = velocity;
  }

  public Segment(Segment segment)
  {
      pos = segment.pos;
      prevPos = segment.prevPos;
      velocity = segment.velocity;
  }
  public static Segment  SetPreviousPosition(Segment segment)
  {
      var temp = segment;
      temp.prevPos = segment.pos;
      segment = temp;
      return segment;
  }
 //  public Segment  SetPreviousPosition()
 //  {
 //      return new Segment(this.pos);
 //  }
 //  public static Segment SetPosition(Vector3 pos,Vector3 prevPos)
 // {
 //     var tmp = new Segment(pos, prevPos);
 //     return tmp;
 // }
 public  Segment SetPosition()
 {
     var tmp = new Segment(this.pos, this.prevPos,this.velocity);
     return tmp;
 }

 public Segment UpdatePosition(Vector3 velocity)
 {
     var temp = new Segment(pos, prevPos,velocity);
     temp.pos += velocity;
     return temp;
 }
 public static Segment UpdatePosition(Segment segment,Vector3 velocity)
 {
     var temp = segment;
     temp.pos = segment.pos + velocity;
     segment = temp;
     return segment;
 }

 
}
