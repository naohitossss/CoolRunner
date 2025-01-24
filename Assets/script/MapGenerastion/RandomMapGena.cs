using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public struct Bliuding 
{
    int num;
    GameObject bliudingObject;
}

public class RandomMapGena : MonoBehaviour
{
    [SerializeField] private List<Bliuding> bliudings = new List<Bliuding>();
    [SerializeField] private List<GameObject> obstacles = new List<GameObject>();
    [SerializeField] private GameObject streatObj;
    [SerializeField] private GameObject grassObj;


    private List<List<int>> mapList =  new List<List<int>>();
    private int buildMapNum = 0;
    private int streatCount = 3;
    private int startMapRow = 24;
    private float streatObjLegth;
    private float streatObjTall;
    private Vector3 setStreatPos;
    private Vector3 setGrassPos;

    void Awake()
    {
        
        
    }

    void Start()
    {
        setStreatPos = Vector3.zero + new Vector3(0f, 0f, streatObjLegth / 2);
        //setGrassPos = Vector3.zero + new Vector3(0f,

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void InitMapData()
    {
        for(int row = 0;row < startMapRow;row++)
        {
            Instantiate(streatObj, setStreatPos, Quaternion.identity);
            setStreatPos += new Vector3(0f,0f,streatObjLegth / 2);


            for (int col = 0;col < streatCount;col++)
            {
                List<int> mapRow = new List<int>();
                mapRow.Add(Random.Range(1,4));
                for(int i = mapRow[0]; i <= streatCount;i++)
                {
                    mapRow[i] = 0;
                }
                mapList.Add(mapRow);    
            }
            
        }
    }
}
