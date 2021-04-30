using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Realtime.Messaging.Internal;

public class World : MonoBehaviour
{
    public GameObject player;
    public Material textureAtlas;
    public static int columnHeight = 16;
    public static int chunkSize = 16;
    public static int worldSize = 10;
    public static int radius = 5;
    public static ConcurrentDictionary<string, Chunck> chunks;
    public Slider loadingAmount;
    public Camera cam;
    public Button playButton;
    bool firstBuild = true;
    bool building = false;
    public static List<string> toRemove = new List<string>();
   
    public Vector3 lastPosBuild;


    public void StartBuild() {
        //StartCoroutine(BuildWorld());
    }
    public static string BuildChunkName(Vector3 v) {
        return (int)v.x + "-" + (int)v.y + "-" + (int)v.z;
    }
    void BuildChunkAt(int x, int y, int z) {
        Vector3 chunckPosition = new Vector3(x * chunkSize, y * chunkSize, z * chunkSize);
        string n = BuildChunkName(chunckPosition);
        Chunck c;
        if (!chunks.TryGetValue(n, out c)) {
            c = new Chunck(chunckPosition, textureAtlas);
            c.chunck.transform.parent = this.transform;
            chunks.TryAdd(c.chunck.name, c);
        }
    }
    IEnumerator BuildRecursiveWorld(int x, int y, int z, int rad) {
        rad--;
        if (rad == 0) yield break;

        BuildChunkAt(x, y, z - 1);
        StartCoroutine(BuildRecursiveWorld(x,y,z-1,rad));
        yield return null;

        BuildChunkAt(x, y, z + 1);
        StartCoroutine(BuildRecursiveWorld(x, y , z + 1, rad));
        yield return null;

        BuildChunkAt(x, y -1, z);
        StartCoroutine(BuildRecursiveWorld(x, y - 1, z , rad));
        yield return null;

        BuildChunkAt(x, y + 1, z);
        StartCoroutine(BuildRecursiveWorld(x, y + 1, z, rad));
        yield return null;

        BuildChunkAt(x + 1, y , z);
        StartCoroutine(BuildRecursiveWorld(x + 1, y , z, rad));
        yield return null;

        BuildChunkAt(x - 1, y , z);
        StartCoroutine(BuildRecursiveWorld(x -1, y , z, rad));
        yield return null;
    }

    IEnumerator DrawChuncks() {
        foreach (KeyValuePair<string, Chunck> c in chunks) {
            if (c.Value.status == Chunck.ChunkStatus.DRAW) {
                c.Value.status = Chunck.ChunkStatus.KEEP;
                c.Value.DrawChunk();
               
            }
            if (c.Value.chunck && Vector3.Distance(player.transform.position, c.Value.chunck.transform.position) > radius * chunkSize) {
                toRemove.Add(c.Key);
            }
            yield return null;
        }
    }
    IEnumerator RemoveOldChuncks() {
        for (int i = 0; i < toRemove.Count; i++) {
            string n = toRemove[i];
            Chunck c;
            if (chunks.TryGetValue(n, out c)) {
                Destroy(c.chunck);
                c.save();
                chunks.TryRemove(n, out c);
                yield return null ;
            }
        }
    }
  

    public void BuildNearPlayer() {
        StopCoroutine("BuildRecursiveWorld");
        StartCoroutine(BuildRecursiveWorld((int)(player.transform.position.x / chunkSize),
                                           (int)(player.transform.position.y / chunkSize),
                                           (int)(player.transform.position.z / chunkSize), radius));
    }
    void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 30;
        Vector3 ppos = player.transform.position;
        player.transform.position = new Vector3(ppos.x, Utils.GenerateHeight(ppos.x, ppos.z) + 1, ppos.z);
        lastPosBuild = player.transform.position;
        player.SetActive(false);
        firstBuild = true;
        chunks = new ConcurrentDictionary<string, Chunck>();
        this.transform.position = Vector3.zero;
        this.transform.rotation = Quaternion.identity;
       
        cam.gameObject.SetActive(false);
        playButton.gameObject.SetActive(false);
        loadingAmount.gameObject.SetActive(false);
        BuildChunkAt((int)(player.transform.position.x/chunkSize),
                     (int)(player.transform.position.y / chunkSize),
                     (int)(player.transform.position.z / chunkSize));
        StartCoroutine(DrawChuncks());
        StartCoroutine(BuildRecursiveWorld((int)(player.transform.position.x/chunkSize),
                                           (int)(player.transform.position.y / chunkSize),
                                           (int)(player.transform.position.z / chunkSize), radius));
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 movement = lastPosBuild - player.transform.position;
        
        if (movement.magnitude > chunkSize)
        {
            lastPosBuild = player.transform.position;
            BuildNearPlayer();
        }
        if (!player.activeSelf) {
            player.SetActive(true);
            firstBuild = false;
        }
        StartCoroutine(DrawChuncks());
        StartCoroutine(RemoveOldChuncks());

    }
}
