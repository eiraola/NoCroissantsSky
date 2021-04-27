
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block 
{
    enum CubeSide { BOTTOM, TOP, LEFT, RIGHT, FRONT, BACK };
    public enum BlockType { GRASS, DIRT, STONE, AIR };
    BlockType bType;
    Chunck owner;
    public bool isSolid;
    GameObject parent;
    public Material cubeMaterial;
    Vector3 position;
    Vector2[,] blockUVs = { 
        /*GrassTOP*/        { new Vector2(0.125f, 0.375f), new Vector2(0.1875f, 0.375f),
                             new Vector2(0.125f, 0.4375f), new Vector2(0.1875f, 0.4375f)},
        /*Grass*/            { new Vector2(0.1875f, 0.9375f), new Vector2(0.25f, 0.9375f),
                             new Vector2(0.1875f, 1.0f), new Vector2(0.25f, 1.0f)},
        /*DIRT*/             { new Vector2(0.125f, 0.9375f), new Vector2(0.1875f, 0.9375f),
                             new Vector2(0.125f, 1.0f), new Vector2(0.1875f, 1.0f)},
        /*Stone*/            { new Vector2(0, 0.875f), new Vector2(0.0625f, 0.875f),
                             new Vector2(0.0f, 0.9375f), new Vector2(0.0625f, 0.9375f)},
    };
    // Start is called before the first frame update
    public Block(BlockType b, Vector3 pos, GameObject p, Chunck c) {
        bType = b;
        parent = p;
        position = pos;
        owner = c;
        isSolid = true;
        if (bType == BlockType.AIR) {
            isSolid = false;
        }
       
    
    }
    void CreateQuad(CubeSide side)
    {
        Mesh mesh = new Mesh();
        mesh.name = "ScriptedMesh";

        Vector3[] vertices = new Vector3[4];
        Vector3[] normals = new Vector3[4];
        Vector2[] uvs = new Vector2[4];
        int[] triangles = new int[6];
        Vector2 uv00 = new Vector2(0.0f, 0.0f);
        Vector2 uv10 = new Vector2(1.0f, 0.0f);
        Vector2 uv01 = new Vector2(0.0f, 1.0f);
        Vector2 uv11 = new Vector2(1.0f, 1.0f);
        if (bType == BlockType.GRASS && side == CubeSide.TOP)
        {
            uv00 = blockUVs[0, 0];
            uv10 = blockUVs[0, 1];
            uv01 = blockUVs[0, 2];
            uv11 = blockUVs[0, 3];
        }
        else if (bType == BlockType.GRASS && side == CubeSide.BOTTOM)
        {
            uv00 = blockUVs[(int)BlockType.DIRT + 1, 0];
            uv10 = blockUVs[(int)BlockType.DIRT + 1, 1];
            uv01 = blockUVs[(int)BlockType.DIRT + 1, 2];
            uv11 = blockUVs[(int)BlockType.DIRT + 1, 3];
        }
        else
        {
            uv00 = blockUVs[(int)bType + 1, 0];
            uv10 = blockUVs[(int)bType + 1, 1];
            uv01 = blockUVs[(int)bType + 1, 2];
            uv11 = blockUVs[(int)bType + 1, 3];
        }


        Vector3 p0 = new Vector3(-0.5f, -0.5f, 0.5f);
        Vector3 p1 = new Vector3(0.5f, -0.5f, 0.5f);
        Vector3 p2 = new Vector3(0.5f, -0.5f, -0.5f);
        Vector3 p3 = new Vector3(-0.5f, -0.5f, -0.5f);
        Vector3 p4 = new Vector3(-0.5f, 0.5f, 0.5f);
        Vector3 p5 = new Vector3(0.5f, 0.5f, 0.5f);
        Vector3 p6 = new Vector3(0.5f, 0.5f, -0.5f);
        Vector3 p7 = new Vector3(-0.5f, 0.5f, -0.5f);

        vertices = new Vector3[] { p4, p5, p1, p0 };
        normals = new Vector3[] { Vector3.forward,
                                  Vector3.forward,
                                  Vector3.forward,
                                  Vector3.forward};
        triangles = new int[] { 3, 1, 0, 3, 2, 1 };
        uvs = new Vector2[] { uv11, uv01, uv00, uv10 };


        switch (side)
        {
            case CubeSide.BOTTOM:
                vertices = new Vector3[] { p0, p1, p2, p3 };
                normals = new Vector3[] { Vector3.down,
                                  Vector3.down,
                                  Vector3.down,
                                  Vector3.down};
                triangles = new int[] { 3, 1, 0, 3, 2, 1 };
                uvs = new Vector2[] { uv11, uv01, uv00, uv10 };

                break;
            case CubeSide.TOP:
                vertices = new Vector3[] { p7, p6, p5, p4 };
                normals = new Vector3[] { Vector3.up,
                                  Vector3.up,
                                  Vector3.up,
                                  Vector3.up};
                triangles = new int[] { 3, 1, 0, 3, 2, 1 };
                uvs = new Vector2[] { uv11, uv01, uv00, uv10 };
                break;
            case CubeSide.LEFT:
                vertices = new Vector3[] { p7, p4, p0, p3 };
                normals = new Vector3[] { Vector3.left,
                                  Vector3.left,
                                  Vector3.left,
                                  Vector3.left};
                triangles = new int[] { 3, 1, 0, 3, 2, 1 };
                uvs = new Vector2[] { uv11, uv01, uv00, uv10 };
                break;
            case CubeSide.RIGHT:
                vertices = new Vector3[] { p5, p6, p2, p1 };
                normals = new Vector3[] { Vector3.right,
                                  Vector3.right,
                                  Vector3.right,
                                  Vector3.right};
                triangles = new int[] { 3, 1, 0, 3, 2, 1 };
                uvs = new Vector2[] { uv11, uv01, uv00, uv10 };
                break;
            case CubeSide.FRONT:
                vertices = new Vector3[] { p4, p5, p1, p0 };
                normals = new Vector3[] { Vector3.forward,
                                  Vector3.forward,
                                  Vector3.forward,
                                  Vector3.forward};
                triangles = new int[] { 3, 1, 0, 3, 2, 1 };
                uvs = new Vector2[] { uv11, uv01, uv00, uv10 };
                break;
            case CubeSide.BACK:
                vertices = new Vector3[] { p6, p7, p3, p2 };
                normals = new Vector3[] { Vector3.back,
                                  Vector3.back,
                                  Vector3.back,
                                  Vector3.back};
                triangles = new int[] { 3, 1, 0, 3, 2, 1 };
                uvs = new Vector2[] { uv11, uv01, uv00, uv10 };
                break;
        }
        mesh.vertices = vertices;
        mesh.normals = normals;
        mesh.uv = uvs;
        mesh.triangles = triangles;

        mesh.RecalculateBounds();
        GameObject quad = new GameObject("Quad");
        quad.transform.position = position;
        quad.transform.parent = parent.transform;
        MeshFilter meshFilter = (MeshFilter)quad.AddComponent(typeof(MeshFilter));
        meshFilter.mesh = mesh;
       // MeshRenderer renderer = quad.AddComponent(typeof(MeshRenderer)) as MeshRenderer;
        //renderer.material = cubeMaterial;


    }
    public void Draw()
    {  if (bType == BlockType.AIR) return ;
        if(!HasSolidNeighbour((int)position.x ,(int)position.y +1, (int)position.z))
        CreateQuad(CubeSide.TOP);
        if (!HasSolidNeighbour((int)position.x, (int)position.y-1, (int)position.z))
            CreateQuad(CubeSide.BOTTOM);
        if (!HasSolidNeighbour((int)position.x-1, (int)position.y, (int)position.z ))
            CreateQuad(CubeSide.LEFT);
        if (!HasSolidNeighbour((int)position.x+1, (int)position.y, (int)position.z))
            CreateQuad(CubeSide.RIGHT);
        if (!HasSolidNeighbour((int)position.x , (int)position.y , (int)position.z + 1))
            CreateQuad(CubeSide.FRONT);
        if (!HasSolidNeighbour((int)position.x, (int)position.y, (int)position.z - 1))
            CreateQuad(CubeSide.BACK);
    }
    int ConverBlokIndexToLocal(int i) {
        if (i == -1)
            i = World.chunkSize - 1;
        else if (i == World.chunkSize)
            i = 0;
        return i;
    }
    public bool HasSolidNeighbour(int x, int y, int z) {

        Block[,,] chuncks;
        if (x < 0 || x >= World.chunkSize ||
            y < 0 || y >= World.chunkSize ||
            z < 0 || z >= World.chunkSize)
        {
            Vector3 neighbourChunkPos = this.parent.transform.position + new Vector3((x - (int)position.x) * World.chunkSize,
                                                                                       (y - (int)position.y) * World.chunkSize,
                                                                                       (z - (int)position.z) * World.chunkSize);
            string nName = World.BuildChunkName(neighbourChunkPos);
            x = ConverBlokIndexToLocal(x);
            y = ConverBlokIndexToLocal(y);
            z = ConverBlokIndexToLocal(z);

            Chunck nChunk;
            if (World.chunks.TryGetValue(nName, out nChunk))
            {
                chuncks = nChunk.chunckData;
            }
            else
            {
                return false;
            }
        }
        else
            chuncks = owner.chunckData;
        try
        {
            return chuncks[x, y, z].isSolid;
        }
        catch (System.Exception ex) { }
        return false;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
