using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbstractBombController : MonoBehaviour
{
    public float damage = 1f;
    [Range(1, 10)]
    public float bombSpeed = 4.5f;
    public AnimationCurve curve;
    public GameObject bomb;
    public ParticleSystem explosion;

    
    protected bool flying = false;
    protected Vector2 start;
    protected Vector2 targetPosition;

    protected float flightDuration;
    protected float timeTravelled;

    protected float distanceTravelled;
    protected float distanceToTravel;


    public virtual void ThrowBomb(Vector2 destination)
    {
        explosion.gameObject.SetActive(false);
        start = transform.position;
        targetPosition = destination;
        distanceTravelled = 0;
        distanceToTravel = Vector2.Distance(transform.position, targetPosition);
        flightDuration = (Vector2.Distance(start, targetPosition)) / bombSpeed;
        timeTravelled = 0;
        flying = true;
        bomb.SetActive(true);
    }

    protected virtual void BombExplode()
    {
        bomb.SetActive(false);
        explosion.gameObject.SetActive(true);
        explosion.Play();
        flying = false;
        Destroy(gameObject, explosion.main.duration);
    }

    
    void Update()
    {
        if (!flying) return;
        //float step = speed * Time.deltaTime;
        //Vector2 newPos = Vector2.MoveTowards(transform.position, target, step);
        //newPos.y += curve.Evaluate(distanceTravelled / distanceToTravel);

        timeTravelled += Time.deltaTime;
        Vector2 newPos = Vector2.Lerp(start, targetPosition, timeTravelled / flightDuration);
        newPos.y += curve.Evaluate(timeTravelled / flightDuration);

        transform.position = newPos;


        //if (Vector2.Distance(transform.position, target) < 0.01f)
        if (timeTravelled > flightDuration)
        {
            BombExplode();
        }
        else
        {
            bomb.transform.Rotate ( Vector3.forward * ( -250 * Time.deltaTime ) );
        }
    }
}
