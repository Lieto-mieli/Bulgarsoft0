using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.UI;

public class FentAbilities : MonoBehaviour
{
    public GameObject fent;
    public GameObject ability1button;
    public GameObject ability2button;
    bool ability1isactive = false;
    bool ability2isactive = false;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update() // if the ability is active and the cooldown is over, make ability available again
    {
        if (ability1isactive == true)
        {
            if (fent.GetComponent<Fent>().curAbility1Cooldown <= 0 && ability1button.GetComponent<Button>().interactable == false)
            {
                ability1button.GetComponent<Button>().interactable = true;
                ability1isactive = false;
            }
        }
        if (ability2isactive == true)
        {
            if (fent.GetComponent<Fent>().curAbility2Cooldown <= 0 && ability2button.GetComponent<Button>().interactable == false)
            {
                ability2button.GetComponent<Button>().interactable = true;
                ability2isactive = false;
            }
        }
    }
    public void Ability1() //activate ability of guard tower, this is called with UI Button
    {
        fent.GetComponent<Fent>().CallToArms();
        ability1button.GetComponent<Button>().interactable = false;
        ability1isactive = true;
    }

    public void Ability2()
    {
        fent.GetComponent<Fent>().CallToBombs();
        ability2button.GetComponent<Button>().interactable = false;
        ability2isactive = true;
    }
}
