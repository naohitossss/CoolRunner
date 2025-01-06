using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RandomMapGena : MonoBehaviour
{
    private List<List<int>> mapList =  new List<List<int>>();
    private int buildMapNum = 0;
    private int streatCount = 3;
    private int startMapRow = 24;

    void Awake()
    {
        
        
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void InitMapData()
    {
        for(int row = 0;row < startMapRow;row++)
        {
            for(int col = 0;col < streatCount;col++)
            {
                List<int> mapRow = new List<int>();
                mapRow.Add(Random.Range(0,3));
                switch (mapRow[0])
                {
                    case 0:
                        for(int i = 1;i <= streatCount;i++)
                        {
                            mapRow[i] = 0;
                        }
                        break;
                    case 1:
                        for(int i = 2;i <= streatCount;i++)
                        {
                            mapRow[i] = 0;
                        }
                        break;
                    case 2:
                        for(int i = 3;i <= streatCount;i++)
                        {
                            mapRow[i] = 0;
                        }
                        break;
                }
                mapList.Add(mapRow);    
            }
            
        }
    }
}
