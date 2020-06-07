﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public float totalDuration = 3.0f;
    public float loopduration;
    public float rangeX = 0.15f;
    public float rangeY = 0.6f;

    void Start()
    {
        Destroy(gameObject, 3f);
    }

    void Update()
    {
        float r = Mathf.Sin((Time.time / loopduration) * (2 * Mathf.PI)) * 0.5f + 0.25f;
        float g = Mathf.Sin((Time.time / loopduration + 0.33333333f) * 2 * Mathf.PI) * 0.5f + 0.25f;
        float b = Mathf.Sin((Time.time / loopduration + 0.66666667f) * 2 * Mathf.PI) * 0.5f + 0.25f;
        float correction = 1 / (r + g + b);
        r *= correction;
        g *= correction;
        b *= correction;
        GetComponent<Renderer>().material.SetVector("_ChannelFactor", new Vector4(r, g, b, 0));
        GetComponent<Renderer>().material.SetVector("_Range", new Vector4(rangeX, rangeY, 0, 1));
    }
}
