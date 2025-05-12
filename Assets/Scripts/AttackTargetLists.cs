using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTargetLists : MonoBehaviour
{
    public List<GameObject> playerTargets = new List<GameObject>();
    public List<GameObject> enemyTargets = new List<GameObject>();
    public GameObject A7VBoss; //lisataan listiin
    // Start is called before the first frame update
    void Start()
    {
        enemyTargets.Add(A7VBoss); //a7v piti antaa uniikki tagi damagea varten ja manuaalisesti lis�t� listaan koska 
        //kaikki enemy tagin omaavat k�y liedon jonkun jutun l�pi joka ei toiminu t�ll� koska a7v ei oo t�ys inherit templatesta
    }

    // Update is called once per frame
    void Update()
    {
    }
}
