using UnityEngine;
using SimulationEngine.Core;
using Unity.Mathematics;

namespace SimulationEngine.Simulations
{
    public class BoidsSimulation : MonoBehaviour
    {
        [SerializeField] private int boidsCount;
        [SerializeField] private float boidSize;
        [SerializeField] private float simulationSize;
        [SerializeField] private GameObject boidPrefab;

        [SerializeField] private float protectedRange;
        [SerializeField] private float visualRange;
        [SerializeField] private float avoidFactor;
        [SerializeField] private float matchingFactor;
        [SerializeField] private float centeringFactor;

        [SerializeField] private float maxSpeed;
        [SerializeField] private float minSpeed;

        [SerializeField] private float turnFactor;
        
        private Particle[] particles;
        private ParticleManager particleManager;

        private void Start()
        {
            particleManager = new ParticleManager();
            particles = particleManager.InitializeParticles(boidsCount, simulationSize, boidSize, boidPrefab);
        }

        private void Update()
        {
            for (int i = 0; i < boidsCount; i++)
            {
                Particle boid = particles[i];
                
                Vector3 close = Vector3.zero;
                Vector3 averageVelocity = Vector3.zero;
                Vector3 averagePosition = Vector3.zero;
                float neighboringBoids = 0;

                // Loop through every other boid
                for (int j = 0; j < boidsCount; j++)
                {
                    if (i == j) continue;

                    Particle otherBoid = particles[j];

                    float distance = Vector3.Distance(boid.position, otherBoid.position);
                    
                    if (distance < protectedRange)
                    {
                        close += boid.position - otherBoid.position;
                    }

                    if (distance < visualRange)
                    {
                        averageVelocity += otherBoid.velocity;
                        averagePosition += otherBoid.position;
                        neighboringBoids += 1;
                    }
                }

                if (neighboringBoids > 0)
                {
                    averageVelocity /= neighboringBoids;
                    averagePosition /= neighboringBoids;
                }

                boid.velocity += close * avoidFactor;
                boid.velocity += (averageVelocity - boid.velocity) * matchingFactor;
                boid.velocity += (averagePosition - boid.position) * centeringFactor;

                float margin = simulationSize;

                if (boid.position.x < -margin) {
                    boid.velocity.x += turnFactor;
                }
                if (boid.position.x > margin) {
                    boid.velocity.x -= turnFactor;
                }
                
                if (boid.position.y < -margin) {
                    boid.velocity.y += turnFactor;
                }
                if (boid.position.y > margin) {
                    boid.velocity.y -= turnFactor;
                }
                
                if (boid.position.z < -margin) {
                    boid.velocity.z += turnFactor;
                }
                if (boid.position.z > margin) {
                    boid.velocity.z -= turnFactor;
                }
                

                float speed = math.sqrt(boid.velocity.x*boid.velocity.x + boid.velocity.y*boid.velocity.y + boid.velocity.z*boid.velocity.z);

                if (speed > maxSpeed)
                {
                    boid.velocity = (boid.velocity / speed) * maxSpeed;
                }

                if (speed < minSpeed)
                {
                    boid.velocity = (boid.velocity / speed) * minSpeed;
                }
                
                particles[i] = boid;
            }
            
            UpdateParticleVisuals(particles);

        }
        
        private void UpdateParticleVisuals(Particle[] particles) {
            for (int i = 0; i < boidsCount; i++) {
                particles[i].position += particles[i].velocity * Time.deltaTime;
                particles[i].gameObject.transform.position = particles[i].position;
                particles[i].gameObject.transform.rotation = Quaternion.LookRotation(particles[i].velocity);
            }
        
        }
    }
}