using System.Collections.Generic;
using UnityEngine;

public class RopePhysics : MonoBehaviour {
    [Header("Anchor")]
    public Transform startTr;

    public Transform endTr;

    [Header("Rope")]
    public LineRenderer lineRenderer;

    //Rope Width
    public float width = 0.1f;

    public float segmentCount = 15;

    //The more increase, elasticity is low.
    public float elasticity = 15.0f;

    //More than 1.
    public float segmentLength = 10.0f;

    //A list of all segment; 
    [SerializeField]
    public List<Segment> segments = new List<Segment>();
    private Vector3[] vertices;

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
        
        //Bottom object is positioned by segments' last position.
        endTr.position = segments[segments.Count - 1].pos;
        endTr.LookAt(segments[segments.Count - 2].pos);
    }

    /// <summary>
    /// Each of segments position is set.
    /// </summary>
    private void SetVertex()
    {
        Vector3 segmentPos = startTr.position;

        for (int i = 0; i < segmentCount; i++)
        {
            Segment currentSegment = new Segment(segmentPos);
            segments.Add(currentSegment);
            /// y of segment's position goes down by segmentLength.
            segmentPos.y -= segmentLength;
        }
    }

    /// <summary>
    /// Simulate Rope using velocity-Verlet algorithm.
    /// </summary>
    private void SimulateRope()
    {
        //Define gravity and deltaTime.
        Vector3 gravity = new Vector3(0, -9.81f, 0);
        float deltaTime = Time.deltaTime;

        //Set start constraint.
        var startSegment = segments[0];
        startSegment.pos = startTr.position;
        segments[0] = startSegment;

        for (int i = 1; i < segmentCount; i++)
        {
            var currentPos = segments[i];
            //Calculate velocity
            Vector3 velocity = segments[i].pos - segments[i].prevPos;
            currentPos.prevPos = segments[i].pos;
            //use velocity-Verlet.
            Vector3 newVelocity = velocity + 0.5f * gravity * deltaTime*deltaTime;
            currentPos.pos += newVelocity;
            segments[i] = currentPos;
        }
    }

    /// <summary>
    /// Move the moved segment to correct position.
    /// </summary>
    private void ApplyFixedPosition()
    {
        //First segment is fixed.
        segments[0] = new Segment(segments[0]);

        for (int i = 0; i < segments.Count - 1; i++)
        {
            var top = segments[i];
            var bottom = segments[i + 1];
            //Distance from top to bottom.
            float dist = (top.pos - bottom.pos).magnitude;

            // How long stretch or compress.
            float diff = dist - segmentLength;
            //Direction to position of top.
            Vector3 dir = (top.pos - bottom.pos).normalized;
            //value that deviate from segment's length.
            Vector3 offset = diff * dir;
            
            //In order to correct position, position of top go down and position of bottom go up.
            if (i == 0)
            {
                bottom.pos += offset * 0.5f;
                segments[i + 1] = bottom;
            }
            else
            {
                top.pos -= offset * 0.5f;
                segments[i] = top;
                bottom.pos += offset * 0.5f;
                segments[i + 1] = bottom;
            }
        }
    }

    /// <summary>
    /// Draw rope using LineRenderer.
    /// </summary>
    private void DrawRope()
    {
        if (vertices == null || vertices.Length != segments.Count)
        {
            vertices = new Vector3[segments.Count];
        }
        
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