using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//responsible for spawning a random object on the ground
public class RandomCubeItem : MonoBehaviour
{
    public Material[] materials;
    public Mesh[] meshes;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<MeshFilter>().mesh = null;
        GetComponent<MeshFilter>().mesh = meshes[Random.Range(0, meshes.Length)];
        switch (GetComponent<MeshFilter>().mesh.name)
        {
            case "Cube Instance":
                {
                    GetComponent<BoxCollider>().enabled = true;
                    break;
                }
            case "Sphere Instance":
                {
                    GetComponent<SphereCollider>().enabled = true;
                    break;
                }
            case "Capsule Instance":
                {
                    GetComponent<CapsuleCollider>().enabled = true;
                    break;
                }
            default:
                {
                    GetComponent<BoxCollider>().enabled = true;
                    break;
                }
        }
        GetComponent<MeshRenderer>().material = materials[Random.Range(0, materials.Length)];
    }
}
