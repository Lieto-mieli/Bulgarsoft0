using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class A7VFlameThrowers : MonoBehaviour
{
    /* HUOMIO, EN POLLAA TaSSa LIEKINHEITIN PARTIKKELEIDEN COLLISONEJA KOSKA NIIDEN POLLAAMINEN TaSTa ON TURHAN MONIMUTKAISTA JA VAATII SUORITUSKYKYa */
    /* Ongelma korjattu movesin tan koodin vaan niihi projectile systeemeihin*/

    public ParticleSystem fThrower;
    public DisplayStandard Display;
    public void Awake()
    {
        fThrower = GetComponent<ParticleSystem>(); //hae liekinheitin komponentit
        fThrower.gameObject.SetActive(false); //deaktivoi aluksi
    }
    private void OnParticleCollision(GameObject other) //unityn inbuilt particle collision juttu 
    {
        //Debug.Log("TOIMII AHH PRE");
        if (other.CompareTag("Guard")) //jos osuu puolustajiin
        {
            //Debug.Log(Display.Frequency+ " " + 5f/Display.Frequency);
            other.gameObject.GetComponent<GuardAITemplate>().hitPoints -= (5f / Display.Frequency); //drainaa puolustajien hp, siten että tekee total 5 damagea sekunnissa
        }
    }
    public void Engage() //metodi jota kutsutaan mainin kautta jos halutaan kaynnistaa liekinheittimet
    {
        //Debug.Log("engage");
        fThrower.gameObject.SetActive(true); //enable liekinheittimet (kaikki)
    }
    public void DisEngage() //metodi jota kutsutaan mainin kautta jos halutaan kaynnistaa liekinheittimet
    {
        fThrower.gameObject.SetActive(false); //enable liekinheittimet (kaikki)
    }
}
