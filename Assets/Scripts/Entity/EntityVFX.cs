using System.Collections;
using UnityEngine;

public class EntityVFX : MonoBehaviour
{
    [Header("On Taking Damage VFX")] [SerializeField]
    private Material onDamageMaterial;

    [SerializeField] private float vfxDuration = 0.2f;

    private Material originalMaterial;
    private SpriteRenderer spriteRenderer;
    private Coroutine onDamageCoroutine;

    [Header("On Doing Damage VFX")] [SerializeField]
    private GameObject hitVfx;

    [SerializeField] private Color hitVfxColor = Color.white;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalMaterial = spriteRenderer.material;
    }

    public void CreateOnHitVFX(Transform target)
    {
        var hitVfxGo = Instantiate(hitVfx, target.position, Quaternion.identity);
        hitVfxGo.GetComponentInChildren<SpriteRenderer>().color = hitVfxColor;
    }

    public void OnDamageVFX()
    {
        if (onDamageCoroutine != null)
        {
            StopCoroutine(onDamageCoroutine);
        }

        onDamageCoroutine = StartCoroutine(OnDamageVFXCoroutine());
    }

    private IEnumerator OnDamageVFXCoroutine()
    {
        spriteRenderer.material = onDamageMaterial;

        yield return new WaitForSeconds(vfxDuration);
        spriteRenderer.material = originalMaterial;
    }
}