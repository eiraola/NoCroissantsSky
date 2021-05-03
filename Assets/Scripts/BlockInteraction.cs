using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockInteraction : MonoBehaviour
{

    public GameObject cam;
   
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       if( Input.GetMouseButtonDown(0)){
            RaycastHit hit;
            if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, 10)) {
                Vector3 hitBlok = hit.point - hit.normal / 2.0f;

                int x = (int)(Mathf.Round(hitBlok.x) - hit.collider.gameObject.transform.position.x);
                int y = (int)(Mathf.Round(hitBlok.y) - hit.collider.gameObject.transform.position.y);
                int z = (int)(Mathf.Round(hitBlok.z) - hit.collider.gameObject.transform.position.z);

                Chunck hitc;
                if (World.chunks.TryGetValue(hit.collider.gameObject.name, out hitc) && hitc.chunckData[x, y, z].HitBlock()) {

                    print("actualizar");
               
                List<string> updates = new List<string>();

                float thisChunkx = hitc.chunck.transform.position.x;
                float thisChunky = hitc.chunck.transform.position.y;
                float thisChunkz = hitc.chunck.transform.position.z;

                //updates.Add(hit.collider.gameObject.name);

                if (x == 0)
                    updates.Add(World.BuildChunkName(new Vector3(thisChunkx - World.chunkSize, thisChunky, thisChunkz)));
                if(x == World.chunkSize -1)
                    updates.Add(World.BuildChunkName(new Vector3(thisChunkx + World.chunkSize, thisChunky, thisChunkz)));
                if (y == 0)
                    updates.Add(World.BuildChunkName(new Vector3(thisChunkx , thisChunky - World.chunkSize, thisChunkz)));
                if (y == World.chunkSize - 1)
                    updates.Add(World.BuildChunkName(new Vector3(thisChunkx , thisChunky + World.chunkSize, thisChunkz)));
                if (z == 0)
                    updates.Add(World.BuildChunkName(new Vector3(thisChunkx, thisChunky , thisChunkz - World.chunkSize)));
                if (z == World.chunkSize - 1)
                    updates.Add(World.BuildChunkName(new Vector3(thisChunkx, thisChunky , thisChunkz + World.chunkSize)));

                foreach (string s in  updates )
                {
                    Chunck c;
                    if (World.chunks.TryGetValue(s, out c))
                    {

                        DestroyImmediate(c.chunck.GetComponent<MeshFilter>());
                        DestroyImmediate(c.chunck.GetComponent<MeshRenderer>());
                        DestroyImmediate(c.chunck.GetComponent<MeshCollider>());
                        DestroyImmediate(c.chunck.GetComponent<Collider>());
                        c.chunckData[x, y, z].setType(Block.BlockType.AIR);
                        c.DrawChunk();
                    }
                }
                }





            }
        }
        
    }
}
