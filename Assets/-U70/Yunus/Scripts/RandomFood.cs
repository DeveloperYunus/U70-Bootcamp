using UnityEngine;

public class RandomFood : MonoBehaviour
{
    public Mesh[] foodMesh;


    void Start()
    {
        GetComponent<MeshFilter>().mesh = foodMesh[Random.Range(0, foodMesh.Length)];
    }
}
