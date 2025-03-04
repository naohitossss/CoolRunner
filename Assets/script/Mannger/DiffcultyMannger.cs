using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiffcultyMannger : MonoBehaviour
{
    [SerializeField] private float SwtichDiffculty = 0.7f;
    [SerializeField] private int easyProbability = 50;
    [SerializeField] private int  normalProbability = 80;    
    private UnityEngine.UI.Slider strokeBar;
    private GameMannger gameMannger;
    private RandomOBSGena randomOBSGena;

    void Start()
    {
        gameMannger = GetComponent<GameMannger>();
        randomOBSGena = GetComponent<RandomOBSGena>();
        strokeBar = gameMannger.GetHeatStrokeBar();
    }


    void Update()
    {
        //　熱中症ゲージの値によって難易度を変更
        if(randomOBSGena != null)
        {
            if( strokeBar.value / strokeBar.maxValue >= SwtichDiffculty)
            {
                randomOBSGena.SetGnenaOBSprobability(easyProbability);
            }
            else
            {
                randomOBSGena.SetGnenaOBSprobability(normalProbability);
            }
        }
        
    }
}
