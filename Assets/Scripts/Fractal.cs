using UnityEngine;
using System.Collections;

public class Fractal : MonoBehaviour
{

    public Mesh[] meshes;
    public Material material;
    public int maxDepth;
    public float childScale;
    public float spawnSproutProbability;
    public float spawnBranchProbability = 1;
    public float maxRotationSpeed;
    public float maxRotationFactor;

    private int depth;
    private float rotationSpeed;
    private Material[,] materials;

    private static Vector3[] childDirections = {
        Vector3.up,
        Vector3.right,
        Vector3.left,
        Vector3.forward,
        Vector3.back,
    };

    private static Quaternion[] childOrientations = {
        Quaternion.identity,
        Quaternion.Euler(0f, 0f, -90),
        Quaternion.Euler(0f, 0f, 90),
        Quaternion.Euler(90, 0f, 0f),
        Quaternion.Euler(-90, 0f, 0f),
    };

    private IEnumerator CreateChildren()
    {   
        DecayProbability();
        ShuffleArray(childDirections);

        float spawnBranchProbabilityLocal = (spawnBranchProbability * depth * 2);

        for (int i = 0; i < childDirections.Length; i++)
        {
            if (Random.value < spawnSproutProbability)
            {
                yield return new WaitForSeconds(Random.Range(0.1f, 0.5f));
                if (depth < 3) 
                { 
                    new GameObject("Fractal Child").AddComponent<Fractal>().Initialize(this, 0);
                    break;
                }
                else if (Random.value < spawnBranchProbabilityLocal)
                {
                    new GameObject("Fractal Child").AddComponent<Fractal>().Initialize(this, i);
                    spawnBranchProbabilityLocal *= (float)(.015f);
                }
            }
        }
    }

    private void Start()
    {
        if (materials == null)
        {
            InitializeMaterials();
        }

        GameObject gameobject = gameObject;
        gameobject.layer = LayerMask.NameToLayer("Default");

        rotationSpeed = Random.Range(depth * -maxRotationSpeed, maxRotationSpeed);
        transform.Rotate(Random.Range(-maxRotationFactor, maxRotationFactor), 0f, 0f);
        if (depth < maxDepth) 
        {
            gameObject.AddComponent<MeshFilter>().mesh = meshes[0];
        }
        else
        {
            gameObject.AddComponent<MeshFilter>().mesh = meshes[1];
        }
        
        gameObject.AddComponent<MeshRenderer>().material = materials[depth, Random.Range(0, 2)];
        if (depth < maxDepth)
        {
            StartCoroutine(CreateChildren());
        }
    }

    private void Update()
    {
        transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f);
    }


    private void Initialize(Fractal parent, int childIndex)
    {
        meshes = parent.meshes;
        materials = parent.materials;
        maxDepth = parent.maxDepth;
        depth = parent.depth + 1;
        childScale = parent.childScale;
        transform.parent = parent.transform;
        transform.localScale = Vector3.one * childScale;
        transform.localPosition = childDirections[childIndex] * (0.5f + 0.5f * childScale);
        transform.localRotation = childOrientations[childIndex];
        maxRotationSpeed = parent.maxRotationSpeed;
        maxRotationFactor = parent.maxRotationFactor;
        spawnSproutProbability = parent.spawnSproutProbability;
}


    private void InitializeMaterials()
    {
        materials = new Material[maxDepth + 1, 2];
        for (int i = 0; i <= maxDepth; i++)
        {
            float t = i / (maxDepth - 1f);
            t *= t;
            
            materials[i, 0] = new Material(material);
            materials[i, 0].color = Color.Lerp(Color.gray, Color.yellow, t);
            materials[i, 1] = new Material(material);
            materials[i, 1].color = Color.Lerp(Color.gray, Color.green, t);
        }
        materials[maxDepth, 0].color = Color.red;
        materials[maxDepth, 1].color = Color.red;
    }

    private void DecayProbability()
    {
        float spawnSproutProbabilityLocal = spawnSproutProbability;

        float[] k = new float[5] { 0.05f, 0.1f, 0.15f, 0.3f, 0.4f }; // decay rates
        int[] x = new int[5] { 1, 4, 7, 10, 13 }; // depths
        float m = 0.05f; // extra decay

        for (int i = 0; i < k.Length; i++)
        {
            if (depth <= x[i])
            {
                spawnSproutProbabilityLocal /= (1.0f + Mathf.Exp(-k[i] * (depth - x[i])));
                break;
            }
        }
        if (depth > x[4])
        {
            spawnSproutProbabilityLocal -= (depth - x[4]) * m;
        }
    }

    private void ShuffleArray(Vector3[] array)
    {
        // Shuffles directions except parent direction
        for (int i = 1; i < childDirections.Length; i++)
        {
            int randomIndex = Random.Range(i, childDirections.Length);
            Vector3 temp = childDirections[i];
            childDirections[i] = childDirections[randomIndex];
            childDirections[randomIndex] = temp;
        }
    }
}