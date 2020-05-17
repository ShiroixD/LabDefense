using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class VolumericExplosion : MonoBehaviour
{
    private Camera _camera;
    private Renderer _renderer;
    private Texture3D _noiseTexture;
    private float _start_time;
    private uint kResolutionX;
    private uint kResolutionY;
    public int NoiseTextureDim = 32;
    public Vector3 kNoiseAnimationSpeed = new Vector3(0.0f, 0.02f, 0.0f);
    public float kNoiseInitialAmplitude = 3.0f;
    public uint kMaxNumSteps = 256;
    public uint kNumHullSteps = 2;
    public float kStepSize = 0.04f;
    public uint kNumOctaves = 4;
    public uint kNumHullOctaves = 2;
    public float kSkinThicknessBias = 0.6f;
    public float kTessellationFactor = 16;
    private float g_MaxSkinThickness;
    private float g_MaxNoiseDisplacement;
    public bool g_EnableHullShrinking = true;
    public float g_EdgeSoftness = 0.05f;
    public float g_NoiseScale = 0.04f;
    private Vector4 g_ProjectionParams;
    private Vector4 g_ScreenParams;
    public float g_ExplosionRadius = 4.0f;
    public float g_DisplacementAmount = 1.75f;
    public Vector2 g_UvScaleBias = new Vector2(2.1f, 0.35f);
    public float g_NoiseAmplitudeFactor = 0.4f;
    public float g_NoiseFrequencyFactor = 3.0f;
    public Texture2D GradientTexRO;

    void Start()
    {
        _camera = Camera.main;
        _renderer = GetComponent<Renderer>();
        kResolutionX = (uint)_camera.scaledPixelWidth;
        kResolutionY = (uint)_camera.pixelHeight;
        _start_time = Time.time;
        LoadResources();
    }

    void Update()
    {
        UpdateExplosionParams();
        //ShaderFieldsTest();
    }

    void LoadResources()
    {
        SetTextures("Assets/Resources/noise_32x32x32.dat");
        UpdateExplosionParams();
    }

    void SetTextures(string path)
    {
        _noiseTexture = LoadTexture3dFromTextFile(path);
        _renderer.material.SetTexture("_NoiseVolumeRO", _noiseTexture);
        _renderer.material.SetTexture("_GradientTexRO", GradientTexRO);
    }

    Texture3D LoadTexture3dFromTextFile(string path)
    {
        int noiseTextureDim = NoiseTextureDim * NoiseTextureDim * NoiseTextureDim;
        ushort maxNoiseValue = 0, minNoiseValue = 0xFFFF;
        ushort[] noiseValues = new ushort[noiseTextureDim];
        
        using (StreamReader reader = new StreamReader(path))
        {
            int linePos = 0;
            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();
                ushort noiseValue;
                ushort.TryParse(line, out noiseValue);
                maxNoiseValue = (ushort)Mathf.Max(maxNoiseValue, noiseValue);
                minNoiseValue = (ushort)Mathf.Min(minNoiseValue, noiseValue);
                noiseValues[linePos] = noiseValue;
                linePos++;
            }
        }

        Texture3D resultTexture = new Texture3D(NoiseTextureDim, NoiseTextureDim, NoiseTextureDim, TextureFormat.R16, true);
        resultTexture.filterMode = FilterMode.Bilinear;
        resultTexture.wrapMode = TextureWrapMode.Repeat;
        resultTexture.SetPixelData(noiseValues, 0);
        resultTexture.Apply(updateMipmaps: true);

        // Calculate the maximum possible displacement from noise based on our 
        //  fractal noise parameters.  This is used to ensure our explosion primitive 
        //  fits in our base sphere.
        float largestAbsoluteNoiseValue = Mathf.Max(Mathf.Abs(maxNoiseValue), Mathf.Abs(minNoiseValue));
        g_MaxNoiseDisplacement = 0;
        for (uint i = 0; i < kNumOctaves; i++)
        {
            g_MaxNoiseDisplacement += largestAbsoluteNoiseValue * kNoiseInitialAmplitude * Mathf.Pow(g_NoiseAmplitudeFactor, (float)i);
        }

        // Calculate the skin thickness, which is amount of displacement to add 
        //  to the geometry hull after shrinking it around the explosion primitive.
        g_MaxSkinThickness = 0;
        for (uint i = kNumHullOctaves; i < kNumOctaves; i++)
        {
            g_MaxSkinThickness += largestAbsoluteNoiseValue * kNoiseInitialAmplitude * Mathf.Pow(g_NoiseAmplitudeFactor, (float)i);
        }

        // Add a little bit extra to account for under-tessellation.  This should be
        //  fine tuned on a per use basis for best performance.
        g_MaxSkinThickness += kSkinThicknessBias;

        return resultTexture;
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
        float A = _camera.farClipPlane / (_camera.farClipPlane - _camera.nearClipPlane);
        float B = (-_camera.farClipPlane * _camera.nearClipPlane) / (_camera.farClipPlane - _camera.nearClipPlane);
        float C = (_camera.farClipPlane - _camera.nearClipPlane);
        float D = _camera.nearClipPlane;
        g_ProjectionParams = new Vector4(A, B, C, D);
        g_ScreenParams = new Vector4((float)kResolutionX, (float)kResolutionY, 1.0f / (float)kResolutionX, 1.0f / (float)kResolutionY);
        float time = Time.time - _start_time;

        _renderer.material.SetMatrix("g_WorldToViewMatrix", view);
        _renderer.material.SetMatrix("g_ViewToProjectionMatrix", projection);
        _renderer.material.SetMatrix("g_ProjectionToViewMatrix", invProjection);
        _renderer.material.SetMatrix("g_WorldToProjectionMatrix", viewProjection);
        _renderer.material.SetMatrix("g_ProjectionToWorldMatrix", viewProjectionInv);
        _renderer.material.SetMatrix("g_ViewToWorldMatrix", invView);

        _renderer.material.SetVector("g_EyePositionWS", new Vector3(cameraPos.x, cameraPos.y, cameraPos.z));
        _renderer.material.SetFloat("g_NoiseAmplitudeFactor", g_NoiseAmplitudeFactor);

        _renderer.material.SetVector("g_EyeForwardWS", new Vector3(cameraForward.x, cameraForward.y, cameraForward.z));
        _renderer.material.SetFloat("g_NoiseScale", g_NoiseScale);

        _renderer.material.SetVector("g_ProjectionParams", g_ProjectionParams);

        _renderer.material.SetVector("g_ScreenParams", g_ScreenParams);

        _renderer.material.SetVector("g_ExplosionPositionWS", new Vector3(explosionPosition.x, explosionPosition.y, explosionPosition.z));
        _renderer.material.SetFloat("g_ExplosionRadiusWS", g_ExplosionRadius);

        _renderer.material.SetVector("g_NoiseAnimationSpeed", new Vector3(kNoiseAnimationSpeed.x, kNoiseAnimationSpeed.y, kNoiseAnimationSpeed.z));
        _renderer.material.SetFloat("g_Time", time);

        _renderer.material.SetFloat("g_EdgeSoftness", g_EdgeSoftness);
        _renderer.material.SetFloat("g_NoiseFrequencyFactor", g_NoiseFrequencyFactor);
        _renderer.material.SetInt("g_PrimitiveIdx", (int)PrimitiveType.kPrimitiveSphere);
        _renderer.material.SetFloat("g_Opacity", 1.0f);

        _renderer.material.SetFloat("g_DisplacementWS", g_DisplacementAmount);
        _renderer.material.SetFloat("g_StepSizeWS", kStepSize);
        _renderer.material.SetInt("g_MaxNumSteps", (int)kMaxNumSteps);
        _renderer.material.SetFloat("g_NoiseInitialAmplitude", kNoiseInitialAmplitude);

        _renderer.material.SetVector("g_UvScaleBias", g_UvScaleBias);
        _renderer.material.SetFloat("g_InvMaxNoiseDisplacement", 1.0f / g_MaxNoiseDisplacement);
        _renderer.material.SetInt("g_NumOctaves", (int)kNumOctaves);

        _renderer.material.SetFloat("g_SkinThickness", g_MaxSkinThickness);
        _renderer.material.SetInt("g_NumHullOctaves", (int)kNumHullOctaves);
        _renderer.material.SetInt("g_NumHullSteps", g_EnableHullShrinking ? (int)kNumHullSteps : 0);
        _renderer.material.SetFloat("g_TessellationFactor", kTessellationFactor);
    }

    void ShaderFieldsTest()
    {
        TexturesTest();
        MatricesTest();
        ParametersTests();
    }

    void TexturesTest()
    {
        Texture3D g_NoiseVolumeRO_test = (Texture3D)_renderer.material.GetTexture("_NoiseVolumeRO");
        var g_NoiseVolumeRO_pixels_test = g_NoiseVolumeRO_test.GetPixels();
        Texture2D g_GradientTexRO_test = (Texture2D)_renderer.material.GetTexture("_GradientTexRO");
        var g_GradientTexRO_pixels_test = g_GradientTexRO_test.GetPixels();
    }

    void MatricesTest()
    {
        var g_WorldToViewMatrix_test = _renderer.material.GetMatrix("g_WorldToViewMatrix");
        var g_ViewToProjectionMatrix_test = _renderer.material.GetMatrix("g_ViewToProjectionMatrix");
        var g_ProjectionToViewMatrix_test = _renderer.material.GetMatrix("g_ProjectionToViewMatrix");
        var g_WorldToProjectionMatrix_test = _renderer.material.GetMatrix("g_WorldToProjectionMatrix");
        var g_ProjectionToWorldMatrix_test = _renderer.material.GetMatrix("g_ProjectionToWorldMatrix");
        var g_ViewToWorldMatrix_test = _renderer.material.GetMatrix("g_ViewToWorldMatrix");
    }

    void ParametersTests()
    {
        var g_EyePositionWS_test = _renderer.material.GetVector("g_EyePositionWS");
        var g_NoiseAmplitudeFactor_test = _renderer.material.GetFloat("g_NoiseAmplitudeFactor");
        var g_EyeForwardWS_test = _renderer.material.GetVector("g_EyeForwardWS");
        var g_NoiseScale_test = _renderer.material.GetFloat("g_NoiseScale");

        var g_ProjectionParams_test = _renderer.material.GetVector("g_ProjectionParams");
        var g_ScreenParams_test = _renderer.material.GetVector("g_ScreenParams");

        var g_ExplosionPositionWS_test = _renderer.material.GetVector("g_ExplosionPositionWS");
        var g_ExplosionRadiusWS_test = _renderer.material.GetFloat("g_ExplosionRadiusWS");

        var g_NoiseAnimationSpeed_test = _renderer.material.GetVector("g_NoiseAnimationSpeed");
        var g_Time_test = _renderer.material.GetFloat("g_Time");

        var g_EdgeSoftness_test = _renderer.material.GetFloat("g_EdgeSoftness");
        var g_NoiseFrequencyFactor_test = _renderer.material.GetFloat("g_NoiseFrequencyFactor");
        var g_PrimitiveIdx_test = _renderer.material.GetInt("g_PrimitiveIdx");
        var g_Opacity_test = _renderer.material.GetFloat("g_Opacity");

        var g_DisplacementWS_test = _renderer.material.GetFloat("g_DisplacementWS");
        var g_StepSizeWS_test = _renderer.material.GetFloat("g_StepSizeWS");
        var g_MaxNumSteps_test = _renderer.material.GetInt("g_MaxNumSteps");
        var g_NoiseInitialAmplitude_test = _renderer.material.GetFloat("g_NoiseInitialAmplitude");

        var g_UvScaleBias_test = _renderer.material.GetVector("g_UvScaleBias");
        var g_InvMaxNoiseDisplacement_test = _renderer.material.GetFloat("g_InvMaxNoiseDisplacement");
        var g_NumOctaves_test = _renderer.material.GetInt("g_NumOctaves");

        var g_SkinThickness_test = _renderer.material.GetFloat("g_SkinThickness");
        var g_NumHullOctaves_test = _renderer.material.GetInt("g_NumHullOctaves");
        var g_NumHullSteps_test = _renderer.material.GetInt("g_NumHullSteps");
        var g_TessellationFactor_test = _renderer.material.GetFloat("g_TessellationFactor");
    }
}
