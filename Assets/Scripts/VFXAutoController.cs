using UnityEngine;

public class VFXAutoController : MonoBehaviour
{
    [SerializeField] private bool autoDestroy = true;
    [SerializeField] private float destroyDelay = 1;
    [Space] [SerializeField] private bool randomOffset = true;
    [SerializeField] private bool randomRotation = true;

    [Header("Random Position")] [SerializeField]
    private Vector2 xOffsetRange = new(-0.3f, 0.3f);

    [SerializeField] private Vector2 yOffsetRange = new(-0.3f, 0.3f);

    private void Start()
    {
        ApplyRandomOffset();
        ApplyRandomRotation();

        if (autoDestroy)
            Destroy(gameObject, destroyDelay);
    }

    private void ApplyRandomOffset()
    {
        if (!randomOffset)
            return;

        var xOffset = Random.Range(xOffsetRange.x, xOffsetRange.y);
        var yOffset = Random.Range(yOffsetRange.x, yOffsetRange.y);

        transform.position += new Vector3(xOffset, yOffset);
    }

    private void ApplyRandomRotation()
    {
        if (!randomRotation)
            return;

        var angle = Random.Range(0, 360);
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}