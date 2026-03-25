using UnityEngine;
using SimulationEngine.Core;

namespace SimulationEngine.Core
{
    public class ParticleManager : MonoBehaviour
    {
        public Particle[] InitializeParticles(int n, float simSize, float particleRadius, GameObject particlePrefab, int types = 1)
        {
            GameObject particlesParent = new GameObject();
            particlesParent.name = "particles";
            
            Particle[] particles = new Particle[n];
            for (int i = 0; i < n; i++) {
                particles[i].position = Random.insideUnitSphere * simSize;
                particles[i].velocity = Vector3.zero;
                particles[i].type = Random.Range(0, types);
                particles[i].gameObject = Instantiate(particlePrefab, particles[i].position, Quaternion.identity);
                particles[i].gameObject.transform.localScale = Vector3.one * particleRadius;
                particles[i].gameObject.transform.parent = particlesParent.transform; // Initializes the particles inside 'particlesParent'
            
                float hue = (float)particles[i].type / (float)types;
                particles[i].gameObject.GetComponent<Renderer>().material.color = Color.HSVToRGB(hue, 1, 1);
            }

            return particles;
        }
        
    }
}