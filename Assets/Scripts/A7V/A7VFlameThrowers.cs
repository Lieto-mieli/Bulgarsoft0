using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class A7VFlameThrowers : MonoBehaviour
{
    /* HUOMIO, EN POLLAA TaSSa LIEKINHEITIN PARTIKKELEIDEN COLLISONEJA KOSKA NIIDEN POLLAAMINEN TaSTa ON TURHAN MONIMUTKAISTA JA VAATII SUORITUSKYKYa */
    /* Ongelma korjattu movesin tan koodin vaan niihi projectile systeemeihin*/

    public ParticleSystem fThrower;
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
            other.gameObject.GetComponent<GuardAITemplate>().hitPoints -= 0.05f; //drainaa puolustajien hp, sama periaate kun bulletilla kaytannössa
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
