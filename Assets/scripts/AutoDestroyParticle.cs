using UnityEngine;
using System.Collections;

public class AutoDestroyParticle : MonoBehaviour {

    private ParticleSystem ps;

    public void Start()
    {
        ps = GetComponent<ParticleSystem>();
        Destroy(gameObject, ps.duration);
    }
}
