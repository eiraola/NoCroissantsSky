using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour
{
    public Material textureAtlas;
    public static int columnHeight = 16;
    public static int chunkSize = 16;
    public static Dictionary<string, Chunck> chunks;

    public static string BuildChunkName(Vector3 v) {
        return (int)v.x + "-" + (int)v.y + "-" + (int)v.z;
    }
    IEnumerator BuildChunkColumn() {
        for (int i = 0; i < columnHeight; i++) {
            Vector3 chunckPosition = new Vector3(this.transform.position.x, i * chunkSize, this.transform.position.z);
            Chunck c = new Chunck(chunckPosition,textureAtlas);
            c.chunck.transform.parent = this.transform;
            chunks.Add(c.chunck.name,c);
            print("xD");
        }
        foreach(KeyValuePair<string, Chunck> c in chunks){
            c.Value.DrawChunk();
            yield return null;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        chunks = new Dictionary<string, Chunck>();
        this.transform.position = Vector3.zero;
        this.transform.rotation = Quaternion.identity;
        StartCoroutine(BuildChunkColumn());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
