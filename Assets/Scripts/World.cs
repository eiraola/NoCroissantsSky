using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class World : MonoBehaviour
{
    public GameObject player;
    public Material textureAtlas;
    public static int columnHeight = 16;
    public static int chunkSize = 16;
    public static int worldSize = 10;
    public static int radius = 1;
    public static Dictionary<string, Chunck> chunks;
    public Slider loadingAmount;
    public Camera cam;
    public Button playButton;
    bool firstBuild = true;
    bool building = false;


    public void StartBuild() {
        StartCoroutine(BuildWorld());
    }
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
    IEnumerator BuildWorld()
    {
        building = true;
        int posx = (int)Mathf.Floor(player.transform.position.x / chunkSize);
        int posz = (int)Mathf.Floor(player.transform.position.z / chunkSize);
        float totalChuncks = (Mathf.Pow(radius * 2 + 1, 2) * columnHeight ) * 2;
        int processCount = 0;
        for (int z = - radius; z <= radius; z++)
        {
            for (int x = -radius; x <= radius; x++)
            {
                for (int y = 0; y < columnHeight; y++)
                {
                    Chunck c;
                    Vector3 chunckPosition = new Vector3((x+posx)*chunkSize, y * chunkSize, (z+posz)*chunkSize);
                    string n = BuildChunkName(chunckPosition);
                    print("oatata");
                    if (chunks.TryGetValue(n, out c))
                    {
                        c.status = Chunck.ChunkStatus.KEEP;
                        break;
                    }
                    else {
                        c = new Chunck(chunckPosition, textureAtlas);
                        c.chunck.transform.parent = this.transform;
                        chunks.Add(c.chunck.name, c);
                    }

                    if (firstBuild) {
                        processCount++;
                        loadingAmount.value = processCount / totalChuncks * 100;    
                    }
                    yield return null;

                }
            }
        }
       
        foreach (KeyValuePair<string, Chunck> c in chunks)
        {
            if (c.Value.status == Chunck.ChunkStatus.DRAW) {
                c.Value.DrawChunk();
                c.Value.status = Chunck.ChunkStatus.KEEP;
            }
            c.Value.status = Chunck.ChunkStatus.DONE;
            if (firstBuild) {
                processCount++;
                loadingAmount.value = processCount / totalChuncks * 100;
            }
           
            yield return null;
        }
        if (firstBuild) { 
            player.SetActive(true);
            cam.gameObject.SetActive(false);
            playButton.gameObject.SetActive(false);
            loadingAmount.gameObject.SetActive(false);
            firstBuild = false;
        }
        building = false;

    }
    // Start is called before the first frame update
    void Start()
    {
        firstBuild = true;
        player.SetActive(false);
        chunks = new Dictionary<string, Chunck>();
        this.transform.position = Vector3.zero;
        this.transform.rotation = Quaternion.identity;
        StartCoroutine(BuildWorld());
    }

    // Update is called once per frame
    void Update()
    {
        if (!building && !firstBuild) {
            StartCoroutine(BuildWorld());
        
        }
        
    }
}
