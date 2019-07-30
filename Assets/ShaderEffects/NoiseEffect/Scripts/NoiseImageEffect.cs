using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Add this component to a gameobject with camera
public class NoiseImageEffect : MonoBehaviour
{
    public Material effectMaterial;

    [Header("Line movement settings")]

    [Tooltip("Set true to get lines that distort the image.")]
    public bool allowLineMovement = true;

    public float line1Speed = 0.8f;
    public float line2Speed = 0.4f;

    [Header("Violet green noise settings")]

    [Tooltip("Set true to get colourful noise image to pop up on the screen.")]
    public bool enableVioletGreen = true;

    [Tooltip("Width of the noise.")]
    [Range(64, 512)]
    public int width = 256;

    [Tooltip("Height of the noise.")]
    [Range(64, 512)]
    public int height = 256;

    [Tooltip("Scale of the noise.")]
    public float scale = 100f;

    [Tooltip("Minimum cooldown of the noise.")]
    public float violetGreenCooldownMin = 0.05f;
    [Tooltip("Maximum cooldown of the noise.")]
    public float violetGreenCooldownMax = 1f;

    [Tooltip("Magnitude of the noise.")]
    [Range(0.01f,1)]
    public float violetGreenMag = 1f;

    [Tooltip("Fadeout speed of the noise.")]
    public float violetGreenFadeoutSpeed = 1f;

    private float currentCooldown = 0;

    private Texture2D generatedTex;

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (!CheckMaterial())
            return;

        Graphics.Blit(source, destination, effectMaterial);
    }

    private bool CheckMaterial()
    {
        if (effectMaterial == null)
        {
            Debug.LogWarning("Effect material is null!");
            return false;
        }
        return true;
    }

    private void Update()
    {
        if (!CheckMaterial())
            return;

        effectMaterial.SetFloat("_OffsetValue", Random.Range(0f, 2f));

        //if (width != Screen.width || height != Screen.height)
        //{
        //    GetScreenSize();
        //}

        if (allowLineMovement)
        {

            effectMaterial.SetFloat("_LineDropSpeed", effectMaterial.GetFloat("_LineDropSpeed") + (line1Speed / 1000f));
            effectMaterial.SetFloat("_Line2DropSpeed", effectMaterial.GetFloat("_Line2DropSpeed") + (line2Speed / 1000f));
        }

        if (enableVioletGreen)
        {
            if (Time.time >= currentCooldown)
            {
                currentCooldown = Time.time + Random.Range(violetGreenCooldownMin, violetGreenCooldownMax);

                Destroy(generatedTex);
                GenerateVioletGreenTexture();
            }
            effectMaterial.SetFloat("_VioletGreenMag", Mathf.PingPong(Time.time * violetGreenFadeoutSpeed, 1f) * violetGreenMag);
        }
        else
        {
            if (effectMaterial.GetFloat("_VioletGreenMag") != 0)
            {
                effectMaterial.SetFloat("_VioletGreenMag", 0f);
            }
        }
    }

    private void OnEnable()
    {
        if (!CheckMaterial())
            return;

        effectMaterial.SetFloat("_LineDropSpeed", 0f);
        effectMaterial.SetFloat("_Line2DropSpeed", 0f);

        effectMaterial.SetFloat("_LineStartPos", 0.15f);
        effectMaterial.SetFloat("_Line2StartPos", 0f);

        if (generatedTex == null)
        {
            GenerateVioletGreenTexture();
        }

        //if (width != Screen.width || height != Screen.height)
        //{
        //    GetScreenSize();
        //}

        currentCooldown = Time.time + Random.Range(violetGreenCooldownMin, violetGreenCooldownMax);
    }

    //private void GetScreenSize()
    //{
    //    width = Screen.width;
    //    height = Screen.height;
    //}

    private void GenerateVioletGreenTexture()
    {
        generatedTex = new Texture2D(width, height);

        int clusterNumber = 0;
        int leaveAreaBlack = 0;
        bool leaveBlack = true;

        // check every 16x16 area if it will be black or violetgreen
        for (int x = 0; x < width; x += 16)
        {
            for (int y = 0; y < height; y += 16)
            {
                // if violetgreen is randomed, clusterNumber defines how many more violetgreens follow after the initial one
                if (clusterNumber == 0)
                {
                    leaveAreaBlack = Random.Range(0, 20);

                    if (leaveAreaBlack < 19)
                    {
                        leaveBlack = true;
                    }
                    else
                    {
                        clusterNumber = Random.Range(0, 7);
                        leaveBlack = false;
                    }
                }
                else
                {
                    leaveBlack = false;
                    clusterNumber--;
                }

                // fill the 16x16 with either black or violetgreen perlin noise
                for (int i = x; i < x + 16; i++)
                {
                    for (int j = y; j < y + 16; j++)
                    {

                        if (leaveBlack)
                        {
                            Color color = new Color(0, 0, 0);
                            generatedTex.SetPixel(i, j, color);
                        }
                        else
                        {
                            Color color = CalculateRandomAreaColor(x, y);
                            generatedTex.SetPixel(i, j, color);
                        }
                    }
                }
            }
        }

        generatedTex.Apply();
        effectMaterial.SetTexture("_VioletGreenTex", generatedTex);
    }

    private Color CalculateRandomAreaColor(int x, int y)
    {
        float xCoord = (float)x / width * scale;
        float yCoord = (float)y / height * scale;

        float sample = Mathf.PerlinNoise(xCoord, yCoord);

        if (Random.Range(0, 2) == 0)
        {
            return new Color(sample, 0, sample);
        }
        else
        {
            return new Color(0, sample, 0);
        }
    }
}
