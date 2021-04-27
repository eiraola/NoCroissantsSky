using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunck 
{
    public Material cubeMaterial;
    public Block[,,] chunckData;
    public GameObject chunck;

    public Chunck(Vector3 position, Material c) {
        chunck = new GameObject(World.BuildChunkName(position));
        chunck.transform.position = position;
        cubeMaterial = c;
        BuildChunk();
    }
    void  BuildChunk() {
        chunckData = new Block[World.chunkSize, World.chunkSize, World.chunkSize];


        for (int z = 0; z < World.chunkSize; z++) {
            for (int y = 0; y < World.chunkSize; y++)
            {
                for (int x = 0; x < World.chunkSize; x++)
                {
                    
                    Vector3 pos = new Vector3(x, y, z);

                    int worldX = (int)(x + chunck.transform.position.x);
                    int worldY = (int)(y + chunck.transform.position.y);
                    int worldZ = (int)(z + chunck.transform.position.z);

                    if (worldY <= Utils.GenerateHeight(worldX, worldZ))
                        chunckData[x, y, z] = new Block(Block.BlockType.DIRT, pos, chunck.gameObject, this);
                    else
                    chunckData[x, y, z] = new Block(Block.BlockType.AIR, pos, chunck.gameObject, this);
                   
                }
            }
           
        }
    }
    public void DrawChunk() {
        for (int z = 0; z < World.chunkSize; z++)
        {
            for (int y = 0; y < World.chunkSize; y++)
            {
                for (int x = 0; x < World.chunkSize; x++)
                {
                    
                    chunckData[x, y, z].Draw();

                }
            }

        }
        CombineQuads();
    }
    void CombineQuads()
    {

        MeshFilter[] meshFilters = chunck.GetComponentsInChildren<MeshFilter>();
        CombineInstance[] combine = new CombineInstance[meshFilters.Length];
        int i = 0;
        while (i < meshFilters.Length)
        {
            combine[i].mesh = meshFilters[i].sharedMesh;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
            i++;
        }

        MeshFilter mf = (MeshFilter)chunck.AddComponent(typeof(MeshFilter));
        mf.mesh = new Mesh();

        mf.mesh.CombineMeshes(combine);

        MeshRenderer renderer = chunck.gameObject.AddComponent(typeof(MeshRenderer)) as MeshRenderer;
        renderer.material = cubeMaterial;

        foreach (Transform quad in chunck.transform)
        {
           GameObject.Destroy(quad.gameObject);
        }

    }
}
