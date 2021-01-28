using System.Collections;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class FireBombController : AbstractBombController
{

    public ParticleSystem fire;
    public ParticleSystem smoke;
    public Light2D lightForFly; // свет во время полета фаербола
    public Light2D lightForExplosion; // свет во время полета фаербола
    [Tooltip("сколько по времени работает вспышка, когда снаряд попал в здание")]
    public float flashTime = 0.3f; 
    [Tooltip("яркость, с которой горит горючая смесь на здании")]
    public float burningBrightness = 0.5f;  

    public override void ThrowBomb(Vector2 destination)
    {
        lightForExplosion.gameObject.SetActive(false);
        lightForFly.gameObject.SetActive(true);
        base.ThrowBomb(destination);
        fire.gameObject.SetActive(true);
        smoke.gameObject.SetActive(true);
        bomb.transform.rotation = Quaternion.Euler(0, 0, 25);
    }
    
    protected override void BombExplode()
    {
        lightForFly.gameObject.SetActive(false);
        lightForExplosion.gameObject.SetActive(true);
        fire.gameObject.SetActive(false);
        smoke.gameObject.SetActive(false);
        base.BombExplode();
        StartCoroutine(FlashBomb());
    }
    
    IEnumerator FlashBomb()
    {
        yield return new WaitForSeconds(flashTime);
        lightForExplosion.intensity = burningBrightness;
    }
}
