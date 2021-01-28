using UnityEngine;
using Random = UnityEngine.Random;

public class BuildingFire : MonoBehaviour
{
    public float minScale = 0.2f;
    public float maxScale = 0.5f;

    private void Start()
    {
        float scale = Random.Range(minScale, maxScale);
        transform.localScale = new Vector3(scale, scale, scale);
    }

}