using UnityEngine;
using System.Collections;
//using UnityEditor;

[RequireComponent(typeof(PolygonTester))]
public class ShapeRunner: MonoBehaviour
{
    public Vector3 velocity = Vector3.zero;
    private PolygonTester meshShape = null;
    private int finalSides = 3;
    
    private ParticleSystem particleExplosion = null;

    private int durationUntilNextVertex = 10;
    private int counter = 1;
    // Use this for initialization
    void Start()
    {
        finalSides = Random.Range(3, 8);
        meshShape = GetComponent<PolygonTester>();
        meshShape.NumberOfSides = 20;
    }

    bool ReadyToDie
    {
        get
        {
            return meshShape.NumberOfSides == finalSides;
        }
    }

    // Update is called once per frame
    void Update()
    {
        ++counter;
        if (counter % durationUntilNextVertex == 0 && meshShape.NumberOfSides > finalSides)
        {
            meshShape.NumberOfSides -= 1;
            counter = 1;
        }
        
        CorrectVelocity();
        this.transform.position += velocity;
        if (ReadyToDie && particleExplosion != null && !particleExplosion.IsAlive())
        {
            Destroy(this);
        }

        if (ReadyToDie)
        {
            KillWithExplosion();
        }
    }

    void CorrectVelocity()
    {
        // On each border, reverse an object's velocity as long as it's not on its way back in.
        var BottomLeft = Camera.main.ViewportToWorldPoint(Vector3.zero);
        var TopRight = Camera.main.ViewportToWorldPoint(Vector3.right + Vector3.up);
        var pos = transform.position;

        if ((pos.x <= BottomLeft.x) && velocity.x < 0f)
        {
            velocity = new Vector3(-velocity.x, velocity.y, 0);
            transform.position.Set(BottomLeft.x, pos.y, pos.z);
            if (ReadyToDie)
            {
                KillWithExplosion();
            }
        }

        if ((pos.x >= TopRight.x) && velocity.x > 0f)
        {
            velocity = new Vector3(-velocity.x, velocity.y, 0);
            transform.position.Set(TopRight.x, pos.y, pos.z);
            if (ReadyToDie)
            {
                KillWithExplosion();
            }
        }

        if ((pos.y <= BottomLeft.y) && velocity.y < 0)
        {
            velocity = new Vector3(velocity.x, -velocity.y, 0);
            transform.position.Set(pos.x, BottomLeft.y, pos.z);
            if (ReadyToDie)
            {
                KillWithExplosion();
            }
        }

        if ((pos.y >= TopRight.y) && velocity.y > 0)
        {
            velocity = new Vector3(velocity.x, -velocity.y, 0);
            transform.position.Set(pos.x, TopRight.y, pos.z);
            if (ReadyToDie)
            {
                KillWithExplosion();
            }
        }
    }

    void KillWithExplosion()
    {
        if(particleExplosion == null) {
            particleExplosion = gameObject.AddComponent<ParticleSystem>();

            ParticleSystemRenderer emitter = particleExplosion.GetComponent<ParticleSystemRenderer>();
            
            particleExplosion.startSpeed = velocity.magnitude + 3;
            particleExplosion.maxParticles = 10;
            particleExplosion.loop = false;
            particleExplosion.emissionRate = 100;
            particleExplosion.startLifetime = 5f;

            emitter.renderMode = ParticleSystemRenderMode.Mesh;
            emitter.mesh = meshShape.filter.mesh;
            particleExplosion.Play(true);
            velocity = Vector3.zero;
            meshShape.hideMesh();
        }
    }
}
