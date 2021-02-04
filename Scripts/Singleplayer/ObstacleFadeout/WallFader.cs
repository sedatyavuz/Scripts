using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class WallFader : MonoBehaviour
{

    private struct MaterialInfo
    {
        public Material material;
        public bool isTransparentMaterial;
    }
    private List<MaterialInfo> materials = new List<MaterialInfo>();

    private bool isTransparent;

    private const float transparency = 0.4f;

    void Awake()
    {
        Renderer[] renderers = GetComponentsInChildren<Renderer>();

        foreach (Renderer renderer in renderers)
        {
            for (int i = 0; i < renderer.materials.Length; i++)
            {
                Material mat = renderer.materials[i];
                MaterialInfo matInfo;
                matInfo.material = mat;

                if (mat.GetFloat("_Surface") == 0)
                    matInfo.isTransparentMaterial = false;
                else
                    matInfo.isTransparentMaterial = true;

                materials.Add(matInfo);
            }
        }
    }

    public IEnumerator CheckVisibility()
    {
        yield return new WaitForEndOfFrame();
        MakeNormal();
    }
    public void MakeTransparent()
    {
        if (!isTransparent)
        {
            isTransparent = true;
            foreach (MaterialInfo item in materials)
            {
                if(!item.isTransparentMaterial)
                    MaterialController.MakeItTransparent(item.material);

                Color color = item.material.GetColor("_BaseColor");
                item.material.SetColor("_BaseColor", new Color(color.r, color.g, color.b, transparency));
            }
        }

        StopAllCoroutines();
        StartCoroutine("CheckVisibility");
    }
    public void MakeNormal()
    {
        if (isTransparent)
        {
            isTransparent = false;
            foreach (MaterialInfo item in materials)
            {
                if (!item.isTransparentMaterial)
                    MaterialController.MakeItOpaque(item.material);

                Color color = item.material.GetColor("_BaseColor");
                item.material.SetColor("_BaseColor", new Color(color.r, color.g, color.b, 1f));
            }
        }
    }
}
