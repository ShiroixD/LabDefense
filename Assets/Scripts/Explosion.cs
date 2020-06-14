using System.Collections;
using System.Collections.Generic;
using System.Security.Policy;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public float totalDuration = 3.0f;
    public float loopduration;
    public float rangeX = 0.15f;
    public float rangeY = 0.6f;
    public bool ifInstanced = false;
    public int ExplosionValue = 10;
    List<Matrix4x4> matrices;
    void Start()
    {
        Destroy(gameObject, 3f);
        matrices = new List<Matrix4x4>();

        if (ifInstanced)
        {
            for (int i = 0; i < ExplosionValue; i++)
            {
                float x = this.transform.position.x + Random.Range(-3.5f, 3.5f);
                float y = this.transform.position.y + Random.Range(-2.0f, 2.5f);
                float z = this.transform.position.z + Random.Range(-3.0f, 3.0f);

                matrices.Add(Matrix4x4.TRS(new Vector3(x, y, z), Quaternion.identity, this.transform.localScale));
            }
        }
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

        if (ifInstanced)
        {
            Graphics.DrawMeshInstanced(this.GetComponent<MeshFilter>().sharedMesh, 0, GetComponent<MeshRenderer>().material, matrices);
        }
    }
}
