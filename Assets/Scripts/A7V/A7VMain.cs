using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static GuardAITemplate;

public class A7VMain : MonoBehaviour
{
    public A7VFlameThrowers ThrowerScript;
    public A7VMainGun MainGunScript;
    public DisplayStandard Display;
    public float moveSpeed;
    public float hitPoints;
    public int displayableHitPoints;
    public float maxHitPoints;
    public float size;
    public float AttackCooldown;
    public float phaseCooldown; //vaiha rpm phasen mukaan
    public GameObject DeploymentGameobject;
    public TextMeshPro hpCount;
    public TextMeshPro SecondPhaseText;
    public Vector3 curPos;
    public Vector3 tarPos;
    public A7VState state;
    public Fent TowerRef;
    public A7VFlameThrowers[] Throwers = new A7VFlameThrowers[5]; //luo lista kaikista liekinheittimista
    public enum A7VState //statet lahestymista ja tuhoamista varten
    {
        movingToPosition,
        FirstPhase,
        SecondPhase
    }
    void Start()
    {
        SecondPhaseText.gameObject.SetActive(false);
        DeploymentGameobject.gameObject.SetActive(false);
        state = A7VState.movingToPosition; //spawnatessa liiku
        tarPos.x = TowerRef.personalCurPos.x;//tornin sijainti
        tarPos.y = TowerRef.personalCurPos.y; //hae tornin pos fiksummin sen scriptista
        AttackCooldown = 0f;
        moveSpeed = UnitStatsList.unitStats[5][0]; //importtaa statslistista olennaiset parametrit
        hitPoints = UnitStatsList.unitStats[5][1];
        maxHitPoints = hitPoints;
        size = UnitStatsList.unitStats[5][8];
        MainGunScript = GetComponentInChildren<A7VMainGun>();
        for (int i = 0; i < Throwers.Length; i++) //kay lapi kuuteen kaikki liekinheittimet ja hae niiden scriptit
        {
            Throwers[i] = transform.Find("Flamethrower" + (i + 1))?.GetComponent<A7VFlameThrowers>(); //hae jokanen yksittainen liekinheitin ja ala löyda nulleja
        }
        foreach (var Thrower in Throwers) //kay jokainen liekinheitinlapi ja aktivoi keycode jalkee
        {
            Thrower?.Engage(); //jos null ala suorita
        }
    }
    void DeployTroops() //deployment metodi
    {
        DeploymentGameobject.gameObject.SetActive(true);
    }
    void Update()
    {
        displayableHitPoints = (int)Math.Ceiling(hitPoints); //displayable hp = hp mutta kokonaislukuina repairia varten
        hpCount.text = displayableHitPoints + " / " + maxHitPoints; //display hp bossin ppaalla
        curPos = transform.position; //updatee sijaintia jotta tiedetaan kuinka lahella ollaan

        if(hitPoints <= 0)
        {
            Destroy(gameObject);
        }
        else if(hitPoints <= maxHitPoints / 2)
        {
            phaseCooldown = 4;
            state = A7VState.SecondPhase;//vaihda cooldowni puolikkaaseen aiemmasta kun 1/3 hp jalella ja vaihda tokaan phaseen
        }

        if(state == A7VState.movingToPosition && Vector2.Distance((Vector2)curPos, tarPos) < 9) //kun kohteessa
        {
            state = A7VState.FirstPhase; //alota tuhoaminen
            DeployTroops();
            phaseCooldown = 8f; //alota cooldown 8sta ja laske 4aan tokassa phasessa
            foreach (var Thrower in Throwers) //kay jokainen liekinheitinlapi ja aktivoi keycode jalkee
            {
                Thrower?.DisEngage(); //jos null ala suorita
            }
        }
        else if(state == A7VState.movingToPosition) //state vaihto ku lahella /\
        {
            transform.position = Vector2.MoveTowards(curPos, tarPos, moveSpeed * Time.deltaTime); //liiku jos ei kohteessa viela
        }
        else if(state == A7VState.FirstPhase) //kutsu maingunin kautta tuhoamisen alotus
        {
            if (AttackCooldown > 0f) //cooldowni
            {
                AttackCooldown = AttackCooldown - Time.deltaTime; 
            }
            else if (AttackCooldown <= 0f)
            {
                MainGunScript?.DestroyBase(); //ettei null ja kutsu
                AttackCooldown = phaseCooldown; //8sta neljaan
            }
        }
        else if(state == A7VState.SecondPhase)
        {
            SecondPhaseText.gameObject.SetActive(true); //display repair teksti
            hitPoints += (1f / Display.Frequency);//repairaa noin 1hp sekunnissa
        }
    }
}