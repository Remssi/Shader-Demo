using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// To use this effect: place this component to a gameobject with ShieldController component. Then place the gameobject with the components as a children of the gameobject you want to have force field.
// You can also use the prefab gameobject with the components, simply place it under the gameobject you want to have force field.
public class ShieldController : MonoBehaviour {

    private ShieldData shieldData;

    private GameObject particleControllerHolder;

    private Color setColor;

    private float setDistort;

    private float setSpeed;

    private float setLineDistort;

    private float setLineSpeed;

    private float setRimPower;

    private float setRimMag;

    private Color setRimColor;

    private float setShieldSize;

    private GameObject createdShield;

    private void OnEnable()
    {
        InitializeController();
        CreateShield();
        
        if (particleControllerHolder != null)
        {
            particleControllerHolder.SetActive(true);
        }
    }

    private void OnDisable()
    {
        if (particleControllerHolder != null)
        {
            particleControllerHolder.SetActive(false);
        }
    }

    private void Update()
    {
        SetShaderProperties();

        if (shieldData.particlesEnable == true && particleControllerHolder.activeInHierarchy == false)
        {
            particleControllerHolder.SetActive(true);
        }
        else if (shieldData.particlesEnable == false && particleControllerHolder.activeInHierarchy == true)
        {
            particleControllerHolder.SetActive(false);
        }

        ShieldSizeVariation();
    }

    private void InitializeController()
    {
        if (shieldData == null)
        {
            shieldData = GetComponentInParent<ShieldData>();
        }
    }

    // Creates the shield mesh based of its parent, if the required components are found in parent
    private void CreateShield()
    {
        if (createdShield == null)
        {
            if (GetComponentInParent<MeshFilter>() != null && GetComponentInParent<MeshRenderer>() != null)
            {
                createdShield = new GameObject("Shield");
                createdShield.transform.parent = transform;
                createdShield.transform.localPosition = Vector3.zero;
                createdShield.transform.localEulerAngles = Vector3.zero;
                createdShield.transform.localScale = Vector3.one;

                MeshFilter createdMeshFilter = createdShield.AddComponent<MeshFilter>();
                createdMeshFilter.mesh = GetComponentInParent<MeshFilter>().mesh;

                MeshRenderer createdMeshRenderer = createdShield.AddComponent<MeshRenderer>();
                createdMeshRenderer.materials = GetComponentInParent<MeshRenderer>().materials;

                Material[] shieldMaterials = createdMeshRenderer.materials;
                for (int i = 0; i < shieldMaterials.Length; i++)
                {
                    shieldMaterials[i] = shieldData.shieldMaterial;
                }
                createdMeshRenderer.materials = shieldMaterials;
            }
            else
            {
                Debug.LogWarning("Can't create shield, no MeshFilter or MeshRenderer found in gameobject or parent!");
            }
        }
    }

    // If value in shieldData has changed, save that value and set it in material
    private void SetShaderProperties()
    {
        if (setColor != shieldData.shieldColor)
        {
            setColor = shieldData.shieldColor;
            shieldData.shieldMaterial.SetColor("_Color", shieldData.shieldColor);
        }
        if (setDistort != shieldData.distortionMagnitude)
        {
            setDistort = shieldData.distortionMagnitude;
            shieldData.shieldMaterial.SetFloat("_Distort", shieldData.distortionMagnitude);
        }
        if (setSpeed != shieldData.distortionSpeed)
        {
            setSpeed = shieldData.distortionSpeed;
            shieldData.shieldMaterial.SetFloat("_Speed", shieldData.distortionSpeed);
        }
        if (setRimPower != shieldData.rimPower)
        {
            setRimPower = shieldData.rimPower;
            shieldData.shieldMaterial.SetFloat("_RimPower", shieldData.rimPower);
        }
        if (setRimMag != shieldData.rimMagnitude)
        {
            setRimMag = shieldData.rimMagnitude;
            shieldData.shieldMaterial.SetFloat("_RimMag", shieldData.rimMagnitude);
        }
        if (setRimColor != shieldData.rimColor)
        {
            setRimColor = shieldData.rimColor;
            shieldData.shieldMaterial.SetColor("_RimColor", shieldData.rimColor);
        }
        if (setShieldSize != shieldData.shieldSize)
        {
            setShieldSize = shieldData.shieldSize;
            shieldData.shieldMaterial.SetFloat("_ShieldSize", shieldData.shieldSize);
        }
        if (shieldData.distortionLineEnable == true)
        {
            if (setLineDistort != shieldData.lineDistortionMagnitude)
            {
                setLineDistort = shieldData.lineDistortionMagnitude;
                shieldData.shieldMaterial.SetFloat("_LineDistort", shieldData.lineDistortionMagnitude);
            }
            if (setLineSpeed != shieldData.lineDistortionSpeed)
            {
                setLineSpeed = shieldData.lineDistortionSpeed;
                shieldData.shieldMaterial.SetFloat("_LineSpeed", shieldData.lineDistortionSpeed);
            }
        }
        else
        {
            if (shieldData.shieldMaterial.GetFloat("_LineDistort") != 0)
            {
                setLineDistort = 0;
                shieldData.shieldMaterial.SetFloat("_LineDistort", 0);
            }
        }
    }

    // Changes shield size by sin values, if variation is enabled in shieldData
    private void ShieldSizeVariation()
    {
        if (shieldData.variationEnable)
        {
            float variationDropAmount = shieldData.variationMaximum - shieldData.variationMinimum;
            shieldData.shieldSize = shieldData.variationMaximum - variationDropAmount * ((Mathf.Sin(Time.time * shieldData.variationSpeed) + 1) / 2);
        }
    }

    public void SetParticleControllerHolder(GameObject holder)
    {
        particleControllerHolder = holder;
    }
}
