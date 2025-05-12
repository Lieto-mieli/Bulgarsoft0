using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A7VMainGun : MonoBehaviour
{
    public ParticleSystem mFlash;
    public GameObject projectile;
    public A7VMain mainRef;
    public void Start()
    {
        mFlash = GetComponentInChildren<ParticleSystem>(); //hae lapsesta muzzleflash effekjti
        mFlash.gameObject.SetActive(false); //disable alussa
        mainRef = GetComponentInParent<A7VMain>(); //reference parentin attackcooldowniin suuliekin takia
        Debug.Log(mFlash);
    }
    public void Update()
    {
        if (mainRef.AttackCooldown > (mainRef.phaseCooldown - 0.1f) && mainRef.state == A7VMain.A7VState.DestroyingBase) //0.1 sekunnin muzzleflash, attackcooldown kertoo paljoko jaljella ja phasecooldown kertoo koko cooldownin pituuden
        {
            mFlash.gameObject.SetActive(true);
        }
        else if (mainRef.AttackCooldown < (mainRef.phaseCooldown - 0.1f) && mainRef.state == A7VMain.A7VState.DestroyingBase)
        {
            mFlash.gameObject.SetActive(false);
        }
    }
    public void DestroyBase()
    {
        GameObject ammuttavaPanos = Instantiate(projectile, new Vector3(transform.position.x, transform.position.y, 1), Quaternion.identity); //luo kuula, suoraan kayttamalla
        //projektilea se ampuu vaan yhdesti
    }
}
