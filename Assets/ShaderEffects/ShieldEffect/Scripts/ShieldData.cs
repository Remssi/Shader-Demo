using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// To use this effect: place this component to a gameobject with ShieldController component. Then place the gameobject with the components as a children of the gameobject you want to have force field.
// You can also use the prefab gameobject with the components, simply place it under the gameobject you want to have force field.
public class ShieldData : MonoBehaviour {

    [Header("Shield Settings")]

    [Tooltip("The material these settings affect to.")]
    public Material shieldMaterial;

    [Tooltip("Color of the shield.")]
    public Color shieldColor = new Color(0.6901961f, 0.791652f, 0.9803922f);

    [Tooltip("Adjusts how much the shield distorts objects behind it.")]
    public float distortionMagnitude = 0.25f;

    [Tooltip("Sets the speed of how fast the distortion changes position.")]
    public float distortionSpeed = 0.64f;

    [Tooltip("The base amount of rim.")]
    public float rimPower = 1.75f;

    [Tooltip("Magnifies the rim around the base amount.")]
    public float rimMagnitude = 1f;

    [Tooltip("Color of the rim.")]
    public Color rimColor = new Color(0.3450981f, 0.5716277f, 0.7254902f);

    [Range(0, 0.5f)]
    [Tooltip("Sets the shield size.")]
    public float shieldSize = 0.15f;


    [Header("Distortion Line Settings")]

    [Tooltip("Set to true to get a distortion line which passes through the shield.")]
    public bool distortionLineEnable = false;

    [Tooltip("Magnifies how much the line distorts objects.")]
    public float lineDistortionMagnitude = 2f;

    [Tooltip("Sets the speed of line.")]
    public float lineDistortionSpeed = 4f;


    [Header("Size Variation Settings")]

    [Tooltip("Set to true to get an effect which increases and decreases the size of the shield.")]
    public bool variationEnable = false;

    [Tooltip("Sets the speed of size variation.")]
    public float variationSpeed = 1f;

    [Tooltip("Minimum size of the shield during variation.")]
    public float variationMinimum = 0.2f;

    [Tooltip("Maximum size of the shield during variation.")]
    public float variationMaximum = 0.5f;


    [Header("Particle Settings")]

    [Tooltip("Set to true to get particles around the shield.")]
    public bool particlesEnable = true;

    [Tooltip("The gameobject used as particle.")]
    public GameObject particleGO;

    [Tooltip("Defines how many particles are around the shield.")]
    public int particleCount = 3;

    [Tooltip("x-axis of the particles movement.")]
    public float xAxis;

    [Tooltip("z-axis of the particles movement.")]
    public float zAxis;

    [Tooltip("Defines the amount of offset for particles' rotation center position.")]
    public Vector3 particlesOffset;

    [Tooltip("Defines the scale of the particles.")]
    public float particleScale = 0.5f;

    [Tooltip("Defines how many positions particles' lines have, which affect the length of the lines.")]
    public int particleLineLength = 75;

    [Tooltip("Defines how thick the line will be.")]
    public float particleLineWidth = 0.16f;

    [Tooltip("How long it takes to complete one orbit.")]
    public float orbitDuration = 3f;
}
