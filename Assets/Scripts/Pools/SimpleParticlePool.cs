using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tools.Pool;
using System;

public class SimpleParticlePool : MonoBehaviour
{
    GenericPoolManager<ParticleSystem> pool;
    [SerializeField] ParticleSystem particle_Model;
    [SerializeField] Transform parent;
    [SerializeField] string ParticleName;

    readonly Vector3 FAR = new Vector3(int.MaxValue, int.MaxValue, int.MaxValue);

    private void Awake()
    {
        pool = new GenericPoolManager<ParticleSystem>(Create, TurnOn, TurnOff, ParticleName, true, 10);
    }

    public void Play(Vector3 position)
    {
        var particle = pool.Get();
        particle.gameObject.transform.position = position;
        particle.Play();
        StartCoroutine(CheckIfIsDeath(particle));
    }

    IEnumerator CheckIfIsDeath(ParticleSystem particle)
    {
        while (particle.isEmitting)
        {
            yield return new WaitForEndOfFrame();
        }
        pool.Return(particle);
        yield break;
    }

    void OnParticleDeath(ParticleSystem part)
    {
        part.Stop();
        pool.Return(part);
    }

    void TurnOn(ParticleSystem obj) => obj.gameObject.SetActive(true);
    void TurnOff(ParticleSystem obj) => obj.gameObject.SetActive(false);

    ParticleSystem Create(object obj)
    {
        return Instantiate(particle_Model, parent.transform.position, parent.transform.rotation, parent);
    }
}
