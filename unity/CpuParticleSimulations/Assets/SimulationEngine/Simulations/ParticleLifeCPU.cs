using System;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

using SimulationEngine.Core;

namespace SimulationEngine.Simulations
{
    public class ParticleLifeCPU : MonoBehaviour {
    public GameObject particlePrefab;
    public int n = 100;
    public float dt = 0.02f;
    public float frictionHalfLife = 0.040f;
    public float distanceMax = 3f;
    public int types = 6;
    public float simSize;
    public float particleRadius;
    public MatrixRow[] matrix; 
    public float forceFactor = 1f;
    private float frictionFactor;

    private Particle[] particles;
    private ParticleManager particleManager;

    private void Start()
    {
        particleManager = new ParticleManager();
        matrix = makeRandomMatrix();
        frictionFactor = Mathf.Pow(0.5f, dt / frictionHalfLife);

        particles =  particleManager.InitializeParticles(n, simSize, particleRadius, particlePrefab, types); // int n, float simSize, float particleRadius, int types, GameObject particlePrefab
    }

    private void Update() {
        ComputeForces(particles);
        UpdateParticleVisuals(particles);
    }

    private MatrixRow[] makeRandomMatrix() {
        MatrixRow[] rows = new MatrixRow[types];
        
        for (int row = 0; row < types; row++) {
            rows[row] = new MatrixRow { columns =  new float[types]};
            
            for (int element = 0; element < types; element++) {
                rows[row].columns[element] = Random.Range(-1.0f, 1.0f);
            }
        }

        return rows;
    }

    

    private void ComputeForces(Particle[] particles) {
        for (int i = 0; i < n; i++) {
            Particle particle = particles[i];
            Vector3 totalForce = Vector3.zero;
            for (int j = 0; j < n; j++) {
                if (j == i) continue;
                Particle otherParticle = particles[j];
                Vector3 direction = particles[j].position - particles[i].position;
                float distanceSquared = math.dot(direction, direction);
                float distance = math.sqrt(distanceSquared);
                if (distance > 0 && distance < distanceMax) {
                    float force = CalculateForce(distance / distanceMax,
                        matrix[particle.type].columns[otherParticle.type]);
                    totalForce += (direction / (distance + 0.001f) * force) * forceFactor;
                }

            }

            totalForce *= distanceMax;
            particle.velocity *= frictionFactor;
            particle.velocity += totalForce * dt;

            particles[i] = particle;
        }
    }

    private void UpdateParticleVisuals(Particle[] particles) {
        for (int i = 0; i < n; i++) {
            particles[i].position += particles[i].velocity;
            particles[i].gameObject.transform.position = particles[i].position;
   
        }
        
    }

    private float CalculateForce(float distance, float attraction) {
        float beta = 0.3f;
        if (distance < beta) {
            return distance / beta - 1;
        } else if (distance > beta && distance < 1) {
            return attraction * (1 - Math.Abs(2 * distance - 1 - beta) / (1 - beta));
        } else {
            return 0;
        }
    }
}
}




