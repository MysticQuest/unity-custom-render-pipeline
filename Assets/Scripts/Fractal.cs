using UnityEngine;
using System.Collections;

public class Fractal : MonoBehaviour
{

    public Mesh mesh;
    public Material material;
    public int maxDepth;
    public float childScale;

    private int depth;

    private static Vector3[] childDirections = {
        Vector3.up,
        Vector3.right,
        Vector3.left,
        Vector3.forward,
        Vector3.back,
        Vector3.down
    };

    private static Quaternion[] childOrientations = {
        Quaternion.identity,
        Quaternion.Euler(0f, 0f, -90),
        Quaternion.Euler(0f, 0f, 90),
        Quaternion.Euler(90, 0f, 0f),
        Quaternion.Euler(-90, 0f, 0f),
        Quaternion.Euler(0f, 0f, -180f)
    };

    private IEnumerator CreateChildren()
    {
        int directions = 0;
        if (depth > 0) { directions = childDirections.Length - 1; }
        else { directions = childDirections.Length; }

        for (int i = 0; i < directions; i++)
        {
            yield return new WaitForSeconds(Random.Range(0.1f, 0.5f));
            new GameObject("Fractal Child").AddComponent<Fractal>().
                Initialize(this, i);
        }
    }

    private void Start()
    {
        gameObject.AddComponent<MeshFilter>().mesh = mesh;
        gameObject.AddComponent<MeshRenderer>().material = material;
        GetComponent<MeshRenderer>().material.color =
        Color.Lerp(Color.white, Color.yellow, (float)depth / maxDepth);
        if (depth < maxDepth)
        {
            StartCoroutine(CreateChildren());
        }
    }

    private void Initialize(Fractal parent, int childIndex)
    {
        mesh = parent.mesh;
        material = parent.material;
        maxDepth = parent.maxDepth;
        depth = parent.depth + 1;
        childScale = parent.childScale;
        transform.parent = parent.transform;
        transform.localScale = Vector3.one * childScale;
        transform.localPosition = childDirections[childIndex] * (0.5f + 0.5f * childScale);
        transform.localRotation = childOrientations[childIndex];
    }
}