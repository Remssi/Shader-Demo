using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleController : MonoBehaviour {

    private ShieldData shieldData;

    private Transform shieldObject;

    private Transform[] particleTransforms;

    private LineRenderer[] lineRenderers;

    private Vector3[][] particlePositions;

    private float[] orbitProgress;

    private int setParticleCount;

    private int setParticleLineLength;

    private int index = 0;

    private void OnEnable()
    {
        InitializeController();
        InitializeParticles();
        StartCoroutine(RotateParticles());
    }

    private void OnDisable()
    {
        StopAllCoroutines();

        ResetParticles();
    }

    private void Update()
    {
        if (setParticleCount != shieldData.particleCount || setParticleLineLength != shieldData.particleLineLength)
        {
            setParticleCount = shieldData.particleCount;
            StopAllCoroutines();
            ResetParticles();
            InitializeParticles();
            StartCoroutine(RotateParticles());
        }
    }

    // Creates empty gameobject which holds the particle controller for calculating local movement of the particles.
    private void InitializeController()
    {
        if (shieldData == null)
        {
            shieldData = GetComponentInParent<ShieldData>();

            shieldObject = transform.parent;

            GameObject holder = new GameObject("ParticleController: " + transform.parent.parent.name);

            transform.parent = holder.transform;

            transform.localScale = Vector3.one;

            transform.localPosition = new Vector3(shieldData.particlesOffset.x, shieldData.particlesOffset.y, shieldData.particlesOffset.z);

            shieldObject.GetComponent<ShieldController>().SetParticleControllerHolder(holder);
        }
    }

    private void ResetParticles()
    {
        for (int i = 0; i < particleTransforms.Length; i++)
        {
            Destroy(particleTransforms[i].gameObject);
        }
        particleTransforms = new Transform[0];

        lineRenderers = new LineRenderer[0];
        particlePositions = new Vector3[0][];
        for (int i = 0; i < particlePositions.Length; i++)
        {
            particlePositions[i] = new Vector3[0];
        }

        orbitProgress = new float[0];
    }

    private void InitializeParticles()
    {

        setParticleLineLength = shieldData.particleLineLength;

        if (shieldData.particleCount < 0)
        {
            shieldData.particleCount = 0;
        }
        if (shieldData.particleLineLength < 1)
        {
            shieldData.particleLineLength = 1;
            setParticleLineLength = shieldData.particleLineLength;
        }

        particleTransforms = new Transform[shieldData.particleCount];

        lineRenderers = new LineRenderer[particleTransforms.Length];
        particlePositions = new Vector3[particleTransforms.Length][];

        for (int i = 0; i < particlePositions.Length; i++)
        {
            particlePositions[i] = new Vector3[shieldData.particleLineLength];
        }

        orbitProgress = new float[shieldData.particleCount];

        for (int i = 0; i < shieldData.particleCount; i++)
        {
            particleTransforms[i] = Instantiate(shieldData.particleGO, transform).transform;

            lineRenderers[i] = particleTransforms[i].GetComponent<LineRenderer>();
            lineRenderers[i].positionCount = setParticleLineLength;

            orbitProgress[i] = (1f / shieldData.particleCount) * i;
        }
    }

    private Vector3 GetPos(float t)
    {
        float angle = Mathf.Deg2Rad * 360f * t;
        float y = Mathf.Sin(angle) * (shieldData.xAxis + shieldData.shieldMaterial.GetFloat("_ShieldSize"));
        float z = Mathf.Cos(angle) * (shieldData.zAxis + shieldData.shieldMaterial.GetFloat("_ShieldSize"));
        return new Vector3(0, y, z);
    }

    private void SetParticlePosition()
    {
        for (int i = 0; i < particleTransforms.Length; i++)
        {
            Vector3 orbitPos = GetPos(orbitProgress[i]);
            particleTransforms[i].localPosition = orbitPos;
            float particleScale;
            if (particleTransforms[i].localScale.x == 0)
            {
                particleScale = 0.01f;
            }
            else
            {
                particleScale = (1f / transform.parent.localScale.x) / particleTransforms[i].localScale.x;
            }

            for (int j = particlePositions[i].Length - 1; j > 0; j--)
            {
                particlePositions[i][j] = particlePositions[i][j - 1];
            }
            particlePositions[i][0] = particleTransforms[i].position - transform.position;

            index = 0;

            for (int j = 1; j < lineRenderers[i].positionCount; j++)
            {
                if (particlePositions[i][j] - particleTransforms[i].position != -particleTransforms[i].position)
                {
                    index = j;
                    lineRenderers[i].SetPosition(j, (particlePositions[i][j] - particleTransforms[i].position + transform.position) * particleScale);
                }
                else
                {
                    lineRenderers[i].SetPosition(j, (particlePositions[i][index] - particleTransforms[i].position + transform.position) * particleScale);
                }
            }
        }
    }

    private IEnumerator RotateParticles()
    {

        while(gameObject.activeInHierarchy && particleTransforms.Length > 0)
        {
            transform.parent.eulerAngles = Vector3.zero;
            transform.localEulerAngles = new Vector3(0f, 360f * orbitProgress[0] * particleTransforms.Length, 0f);

            if (shieldData.orbitDuration == 0)
            {
                shieldData.orbitDuration = 0.1f;
            }
            float orbitSpeed = 1f / shieldData.orbitDuration;

            for (int i = 0; i < particleTransforms.Length; i++)
            {
                orbitProgress[i] += Time.deltaTime * orbitSpeed;
                orbitProgress[i] %= 1f;
            }
            SetParticlePosition();

            for (int i = 0; i < particleTransforms.Length; i++)
            {
                particleTransforms[i].rotation = Quaternion.identity;
                particleTransforms[i].localScale = new Vector3(shieldData.particleScale, shieldData.particleScale, shieldData.particleScale);
            }

            transform.parent.position = shieldObject.position;
            transform.parent.localScale = shieldObject.lossyScale;
            transform.parent.eulerAngles = shieldObject.eulerAngles;
            transform.localPosition = new Vector3(shieldData.particlesOffset.x, shieldData.particlesOffset.y, shieldData.particlesOffset.z);

            for (int i = 0; i < lineRenderers.Length; i++)
            {
                lineRenderers[i].startWidth = shieldData.particleLineWidth * shieldObject.lossyScale.x;
            }

            yield return null;
        }
    }
}
