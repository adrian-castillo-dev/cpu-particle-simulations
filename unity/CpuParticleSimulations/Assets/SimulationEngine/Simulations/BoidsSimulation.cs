using UnityEngine;
using SimulationEngine.Core;

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

                // Loop thru every other boid
                for (int j = 0; j < boidsCount; j++)
                {
                    if (i == j) continue;

                    Particle otherBoid = particles[j];

                    float distance = Vector3.Distance(boid.position, otherBoid.position);
                    if (distance < protectedRange)
                    {
                        close += boid.position - otherBoid.position;
                    }
                }

                boid.velocity += close * avoidFactor;
                
                particles[i] = boid;
                UpdateParticleVisuals(particles);
            }
            
        }
        
        private void UpdateParticleVisuals(Particle[] particles) {
            for (int i = 0; i < boidsCount; i++) {
                particles[i].position += particles[i].velocity * Time.deltaTime;
                particles[i].gameObject.transform.position = particles[i].position;
   
            }
        
        }
    }
}