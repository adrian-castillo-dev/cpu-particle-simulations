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
        
        private Particle[] particles;
        private ParticleManager particleManager;

        private void Start()
        {
            particleManager = new ParticleManager();
            particles = particleManager.InitializeParticles(boidsCount, simulationSize, boidSize, boidPrefab);
        }

        private void Update()
        {
            
        }
    }
}