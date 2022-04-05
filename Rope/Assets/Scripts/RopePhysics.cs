using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class RopePhysics : MonoBehaviour {
    public Transform startTr;
    public Transform endTr;
    
    [Header("Rope")]
    public LineRenderer lineRenderer;
    public float width = 0.1f;
    
    public float segmentCount =15;
    public float elasticity=15.0f;
    //More than 1
    public float segmentLength=10.0f;
    
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
        for (int i = 0; i < elasticity; i++)
        {
            ApplyFixedPosition();
        }
        
    }

    public void Update()
    {
        DrawRope();
        endTr.position = segments[segments.Count - 1].pos;
        endTr.LookAt(segments[segments.Count-2].pos);
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
        
        var startSegment = segments[0];
        startSegment.pos = startTr.position;
        segments[0] = startSegment;
        
        for (int i = 1; i < segmentCount; i++)
        {
            var currentPos = segments[i];
            //Calculate velocity
            Vector3 velocity = segments[i].pos - segments[i].prevPos;
            currentPos.prevPos = segments[i].pos;
            
            Vector3 newVelocity = velocity+ 0.5f * gravity * deltaTime;
            currentPos.pos += newVelocity;
            currentPos.velocity += gravity * deltaTime;
            segments[i] = currentPos;
            
        }
    }

    public void ApplyFixedPosition()
    {
        segments[0] = new Segment(segments[0]);
        for (int i = 0; i < segments.Count-1; i++)
        {
            var top = segments[i];
            var bottom = segments[i + 1];
            float dist = (top.pos-bottom.pos).magnitude;
            
            // How long stretch or compress.
            float diff = dist - segmentLength;
            Vector3 dir = (top.pos - bottom.pos).normalized;

            Vector3 distortion = diff * dir;



                if (i == 0)
            {
                bottom.pos += distortion*0.5f;
                segments[i + 1] = bottom;
            }
            else
            {
                top.pos -= distortion*0.5f;
                segments[i] = top;
                bottom.pos += distortion*0.5f;
                segments[i + 1] = bottom;
            }
        }
        
    }
    
    //public void ApplyConstraint
    public void DrawRope()
    {
        Vector3[] vertices = new Vector3[segments.Count];
        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i] = segments[i].pos;
        }

        lineRenderer.startWidth = width;
        lineRenderer.endWidth = width;
        
        lineRenderer.positionCount = vertices.Length;
        lineRenderer.SetPositions(vertices);
    }
}
