using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumericExplosion : MonoBehaviour
{
    private Camera _camera;
    private Renderer _renderer;
    public Vector3 kNoiseAnimationSpeed = new Vector3(0.0f, 0.02f, 0.0f);
    public float kNoiseInitialAmplitude = 3.0f;
    public uint kMaxNumSteps = 256;
    public uint kNumHullSteps = 2;
    public float kStepSize = 0.04f;
    public uint kNumOctaves = 4;
    public uint kNumHullOctaves = 2;
    public float kSkinThicknessBias = 0.6f;
    public float kTessellationFactor = 16;
    public float g_MaxSkinThickness;
    public float g_MaxNoiseDisplacement;
    public bool g_EnableHullShrinking = true;
    public float g_EdgeSoftness = 0.05f;
    public float g_NoiseScale = 0.04f;
    public float g_ExplosionRadius = 4.0f;
    public float g_DisplacementAmount = 1.75f;
    public Vector2 g_UvScaleBias = new Vector2(2.1f, 0.35f);
    public float g_NoiseAmplitudeFactor = 0.4f;
    public float g_NoiseFrequencyFactor = 3.0f;

    void Start()
    {
        _camera = Camera.main;
        _renderer = GetComponent<Renderer>();
    }

    void Update()
    {
        UpdateExplosionParams();
    }

    void UpdateExplosionParams()
    {
        Matrix4x4 view = _camera.worldToCameraMatrix;
        Matrix4x4 projection = _camera.projectionMatrix;
        Matrix4x4 invProjection = projection.inverse;
        Matrix4x4 viewProjection = view * projection;
        Matrix4x4 viewProjectionInv = viewProjection.inverse;
        Matrix4x4 invView = view.inverse;
        Vector3 cameraPos = _camera.transform.position;
        Vector3 cameraForward = _camera.transform.forward;
        Vector3 explosionPosition = transform.position;

        _renderer.material.SetMatrix("g_WorldToViewMatrix", view);
        _renderer.material.SetMatrix("g_ViewToProjectionMatrix", projection);
        _renderer.material.SetMatrix("g_ProjectionToViewMatrix", invProjection);
        _renderer.material.SetMatrix("g_WorldToProjectionMatrix", viewProjection);
        _renderer.material.SetMatrix("g_ProjectionToWorldMatrix", viewProjectionInv);
        _renderer.material.SetMatrix("g_ViewToWorldMatrix", invView);
        _renderer.material.SetVector("g_EyePositionWS", new Vector4(cameraPos.x, cameraPos.y, cameraPos.z, 0.0f));
        _renderer.material.SetFloat("g_NoiseAmplitudeFactor", g_NoiseAmplitudeFactor);
        _renderer.material.SetVector("g_EyeForwardWS", new Vector4(cameraForward.x, cameraForward.y, cameraForward.z, 0.0f));
        _renderer.material.SetFloat("g_NoiseScale", g_NoiseScale);
        _renderer.material.SetVector("g_ExplosionPositionWS", new Vector4(explosionPosition.x, explosionPosition.y, explosionPosition.z, 0.0f));
        _renderer.material.SetFloat("g_ExplosionRadius", g_ExplosionRadius);
        _renderer.material.SetVector("g_NoiseAnimationSpeed", new Vector4(kNoiseAnimationSpeed.x, kNoiseAnimationSpeed.y, kNoiseAnimationSpeed.z, 0.0f));
        _renderer.material.SetFloat("g_EdgeSoftness", g_EdgeSoftness);
        _renderer.material.SetFloat("g_NoiseFrequencyFactor", g_NoiseFrequencyFactor);
        _renderer.material.SetInt("g_PrimitiveIdx", (int)PrimitiveType.kPrimitiveSphere);
        _renderer.material.SetFloat("g_Opacity", 1.0f);
        _renderer.material.SetFloat("g_DisplacementWS", g_DisplacementAmount);
        _renderer.material.SetFloat("g_StepSizeWS", kStepSize);
        _renderer.material.SetInt("g_MaxNumSteps", (int)kMaxNumSteps);
        _renderer.material.SetVector("g_UvScaleBias", new Vector4(g_UvScaleBias.x, g_UvScaleBias.y, 0.0f, 0.0f));
        _renderer.material.SetFloat("g_NoiseInitialAmplitude", kNoiseInitialAmplitude);
        _renderer.material.SetFloat("g_InvMaxNoiseDisplacement", 1.0f / g_MaxNoiseDisplacement);
        _renderer.material.SetInt("g_NumOctaves", (int)kNumOctaves);
        _renderer.material.SetFloat("g_SkinThickness", g_MaxSkinThickness);
        _renderer.material.SetInt("g_NumHullOctaves", (int)kNumHullOctaves);
        _renderer.material.SetInt("g_NumHullSteps", g_EnableHullShrinking ? (int)kNumHullSteps : 0);
        _renderer.material.SetFloat("g_TessellationFactor", kTessellationFactor);
    }
}
