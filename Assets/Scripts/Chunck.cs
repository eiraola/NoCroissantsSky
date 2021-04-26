using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunck : MonoBehaviour
{
    public Material cubeMaterial;
    public Block[,,] chunckData;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(BuildChunk(16,16,16));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator BuildChunk(int sizeX, int sizeY, int sizeZ) {
        chunckData = new Block[sizeX, sizeY, sizeZ];


        for (int z = 0; z < sizeZ; z++) {
            for (int y = 0; y < sizeY; y++)
            {
                for (int x = 0; x < sizeX; x++)
                {
                    Vector3 pos = new Vector3(x, y, z);
                    if (Random.Range(0, 100) < 50)
                    chunckData[x, y, z] = new Block(Block.BlockType.DIRT, pos, this.gameObject, cubeMaterial);
                    else
                    chunckData[x, y, z] = new Block(Block.BlockType.AIR, pos, this.gameObject, cubeMaterial);

                }
            }
           
        }
        for (int z = 0; z < sizeZ; z++)
        {
            for (int y = 0; y < sizeY; y++)
            {
                for (int x = 0; x < sizeX; x++)
                {
                    chunckData[x, y, z].Draw();
                  
                }
            }

        }
        CombineQuads();
        yield return null;

    }
    void CombineQuads()
    {

        MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();
        CombineInstance[] combine = new CombineInstance[meshFilters.Length];
        int i = 0;
        while (i < meshFilters.Length)
        {
            combine[i].mesh = meshFilters[i].sharedMesh;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
            i++;
        }

        MeshFilter mf = (MeshFilter)this.gameObject.AddComponent(typeof(MeshFilter));
        mf.mesh = new Mesh();

        mf.mesh.CombineMeshes(combine);

        MeshRenderer renderer = this.gameObject.AddComponent(typeof(MeshRenderer)) as MeshRenderer;
        renderer.material = cubeMaterial;

        foreach (Transform quad in this.transform)
        {
            Destroy(quad.gameObject);
        }

    }
}
