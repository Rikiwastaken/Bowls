using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    [SerializeField] private SkinnedMeshRenderer EnemyMainBodyModel;

    [SerializeField] private GameObject EnemyModel;

    [SerializeField] private Color EnemyColor;

    [SerializeField] private Light EnemyLight;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Setup();
    }



    public void Setup()
    {

        if (EnemyColor != null)
        {

            // We get the eye model and then replace it

            Material[] materials = EnemyMainBodyModel.GetComponent<SkinnedMeshRenderer>().materials;

            Material originialmat = null;
            for (int i = 0; i < materials.Length; i++)
            {
                if (materials[i].name.ToLower().Contains("eye"))
                {
                    originialmat = materials[i];
                }
            }

            if (originialmat != null)
            {

                Material newmat = new Material(originialmat);
                newmat.color = EnemyColor;
                newmat.SetColor("_EmissionColor", EnemyColor);

                ApplyMatListToChildren(EnemyModel.transform, newmat, originialmat);
            }

            EnemyLight.color = EnemyColor;
        }
    }

    // This function replaces a material with another material on the transform if it has a renderer, and then it calls itself for every child, which essentially changes the material for the whole model
    private void ApplyMatListToChildren(Transform transform, Material mat, Material OriginalMat)
    {
        if (transform.GetComponent<Renderer>())
        {

            Renderer Renderer = transform.GetComponent<Renderer>();

            Material[] materials = Renderer.materials;

            List<Material> newmats = new List<Material>();

            for (int i = 0; i < materials.Length; i++)
            {
                if (materials[i] == OriginalMat)
                {
                    newmats.Add(mat);
                }
                else
                {
                    newmats.Add(materials[i]);
                }
            }

            Renderer.SetMaterials(newmats);
        }

        foreach (Transform Child in transform)
        {
            ApplyMatListToChildren(Child, mat, OriginalMat);
        }

    }

}
