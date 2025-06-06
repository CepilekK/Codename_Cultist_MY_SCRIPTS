using UnityEngine;

public class RandomScale : MonoBehaviour
{
    [SerializeField] private float scaleMin = 0.7f;
    [SerializeField] private float scaleMax = 1.6f;

    private void Start()
    {
        randomScale(scaleMin, scaleMax);
    }
    private void randomScale (float scaleMin, float scaleMax)
    {
        float scale = Random.Range(scaleMin, scaleMax);
        transform.localScale = new Vector3(scale, scale, scale);
    }
}
