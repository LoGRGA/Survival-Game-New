using UnityEngine;

public class CrescentWave : MonoBehaviour
{
    private float radius = 2f;
    private float thickness = 0.5f;
    private float speed;
    private float lifetime;
    private int damage;
    private LayerMask layerMask;
    private Rigidbody rb;
    private bool hitTarget = false;

    public void Initialize(float waveSpeed, float waveLifetime, int waveDamage, LayerMask hittableLayer)
    {
        speed = waveSpeed;
        lifetime = waveLifetime;
        damage = waveDamage;
        layerMask = hittableLayer;

        // Create the mesh dynamically
        CreateCrescentMesh();

        // Add Rigidbody for movement
        rb = gameObject.AddComponent<Rigidbody>();
        rb.useGravity = false;
        rb.velocity = transform.forward * speed;

        transform.gameObject.layer = 9;

        Physics.IgnoreLayerCollision(9, 0);
        Physics.IgnoreLayerCollision(9, 1);
        Physics.IgnoreLayerCollision(9, 2);
        Physics.IgnoreLayerCollision(9, 3);
        Physics.IgnoreLayerCollision(9, 4);
        Physics.IgnoreLayerCollision(9, 5);
        Physics.IgnoreLayerCollision(9, 7);
        Physics.IgnoreLayerCollision(9, 8);

        // Destroy after lifetime
        Destroy(gameObject, lifetime);
    }

    private void CreateCrescentMesh()
    {
        MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();

        Mesh mesh = new Mesh();

        // Vertices
        Vector3[] vertices = new Vector3[8];
        vertices[0] = new Vector3(-radius, thickness, 0);
        vertices[1] = new Vector3(-radius, -thickness, 0);
        vertices[2] = new Vector3(radius, thickness, 0);
        vertices[3] = new Vector3(radius, -thickness, 0);
        vertices[4] = new Vector3(-radius / 2, thickness / 2, 0);
        vertices[5] = new Vector3(-radius / 2, -thickness / 2, 0);
        vertices[6] = new Vector3(radius / 2, thickness / 2, 0);
        vertices[7] = new Vector3(radius / 2, -thickness / 2, 0);

        // Triangles
        int[] triangles = new int[]
        {
            0, 4, 1, // Left outer part
            1, 4, 5,
            4, 6, 5, // Middle
            5, 6, 7,
            6, 2, 7, // Right outer part
            7, 2, 3
        };

        // UVs for texturing (optional)
        Vector2[] uvs = new Vector2[vertices.Length];
        for (int i = 0; i < uvs.Length; i++)
        {
            uvs[i] = new Vector2(vertices[i].x, vertices[i].y);
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;

        mesh.RecalculateNormals();
        meshFilter.mesh = mesh;

        meshRenderer.material = new Material(Shader.Find("Standard")) { color = Color.blue };

        // Add a Collider (e.g., BoxCollider or SphereCollider)
        BoxCollider collider = gameObject.AddComponent<BoxCollider>();

        // Ensure the collider is not a trigger
        collider.isTrigger = false;


    }

    private void OnCollisionEnter(Collision c)
    {
        if (hitTarget)
            return;
        else
            hitTarget = true;

        // Collision logic
        if (c.gameObject.transform.TryGetComponent<EnemyBehaviour>(out EnemyBehaviour enemy))
        {
            enemy.TakeDamage(damage);
            Destroy(gameObject);
            Debug.Log("Damage applied to: " + c.gameObject.name);
        }
        
    }
}