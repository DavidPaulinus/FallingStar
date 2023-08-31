using UnityEngine;

public class DirtyEffect : MonoBehaviour
{
    public static DirtyEffect instance;

    private ParticleSystem particle;

    private void Awake()
    {
        instance = this;
        particle = GetComponent<ParticleSystem>();
    }

    public void PlayParticles(Transform posicao, float direcao)
    {
        var _direcaoParticula = particle.forceOverLifetime;

        transform.position = new Vector3(posicao.position.x, posicao.position.y, transform.position.z);
        _direcaoParticula.x = direcao * 3;

        particle.Play();
    }

    public void StopParticles()
    {
        particle.Stop();
    }
}
