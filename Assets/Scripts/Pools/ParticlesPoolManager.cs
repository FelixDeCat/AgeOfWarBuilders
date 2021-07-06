using UnityEngine;

public class ParticlesPoolManager : MonoBehaviour
{
    public static ParticlesPoolManager instance;
    private void Awake() => instance = this;
    public SimpleParticlePool catapult;
    public SimpleParticlePool destroyTower;

    public static void Play_Catapult(Vector3 position) => instance.catapult.Play(position);
    public static void Play_DestroyTower(Vector3 position) => instance.destroyTower.Play(position);

}
