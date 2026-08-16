using UnityEngine;

[ExecuteAlways]
public class EyeGlow : MonoBehaviour
{
    public Renderer targetRenderer;
    public int materialIndex = 0;

    [ColorUsage(true, true)]
    public Color emissionColor = Color.cyan;

    [Range(0f, 20f)]
    public float intensity = 0f;

    private MaterialPropertyBlock block;

    void OnEnable()
    {
        UpdateEmission();
    }

    void Update()
    {
        UpdateEmission();
    }

    void OnValidate()
    {
        UpdateEmission();
    }

    void UpdateEmission()
    {
        if (targetRenderer == null)
            return;

        if (block == null)
            block = new MaterialPropertyBlock();

        targetRenderer.GetPropertyBlock(block, materialIndex);

        block.SetColor(
            "_EmissionColor",
            emissionColor * intensity
        );

        targetRenderer.SetPropertyBlock(block, materialIndex);
    }
}