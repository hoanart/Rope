using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopePhysics : MonoBehaviour {
    public Transform startTr;
    
    public LineRenderer lineRenderer;

    public float segmentCount;

    //More than 1
    public float segmentLength;
    
    [SerializeField]
    public List<Segment> segments = new List<Segment>();
    

    // Start is called before the first frame update
    void Start()
    {
        SetVertex();
    }

    private void FixedUpdate()
    {
        SimulateRope();
    }

    public void Update()
    {
        DrawRope();
    }

    public void SetVertex()
    {
        Vector3 segmentPos = startTr.position;
        
        for (int i = 0; i < segmentCount; i++)
        {
            Segment currentSegment = new Segment(segmentPos); 
            segments.Add(currentSegment);
            segmentPos.y -= segmentLength;
        }
    }

    public void SimulateRope()
    {
        //Define gravity and deltaTime.
        Vector3 gravity = new Vector3(0, -9.81f, 0);
        float deltaTime = Time.fixedDeltaTime;
        
        for (int i = 0; i < segmentCount; i++)
        {
            //Calculate velocity
            Vector3 velocity = segments[i].pos - segments[i].prevPos;
            //segments[i] = Segment.SetPreviousPosition(segments[i]);
            segments[i].SetPreviousPosition();
            Vector3 newVelocity = velocity * deltaTime + 0.5f * gravity * deltaTime * deltaTime;
            segments[i] = Segment.UpdatePosition(segments[i], newVelocity);
            
            //segments[i].pos += velocity * t + 0.5f * gravity * t * t;
        }
    }

    public void ApplyFixedPosition()
    {
        segments[0] = Segment.SetPosition(segments[0], startTr.position);
        var temp = segments[0];
        segments[0] =Segment.SetPosition(segments[0], startTr.position);
        segments[0] = temp;

    }
    
    //public void ApplyConstraint
    public void DrawRope()
    {
        Vector3[] vertices = new Vector3[segments.Count];
        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i] = segments[i].pos;
        }

        lineRenderer.positionCount = vertices.Length;
        lineRenderer.SetPositions(vertices);
    }
}
