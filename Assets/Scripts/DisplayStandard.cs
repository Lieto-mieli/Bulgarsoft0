using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayStandard : MonoBehaviour
{
    public int Frequency = 90; //90 koska liekit naytti oudolle 60, cappi sen takia ettei vie liikaa resursseja ku pyorii 9 000 fps
    void Awake() //awake jotta ehtii inittaa ennen muita, voi tulla ongelmia jos start
    {
        QualitySettings.vSyncCount = 0; //muuten frame lockkaa 
        Application.targetFrameRate = Frequency; //aseta maaritetty taajuus
    }
    void Update()
    {
        if(Application.targetFrameRate != Frequency) //esta capin ylitys
        {
            Application.targetFrameRate = Frequency;
        }
    }
}
