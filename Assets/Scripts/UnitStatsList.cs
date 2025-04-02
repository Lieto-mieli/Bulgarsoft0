using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class UnitStatsList : MonoBehaviour
{
    // moveSpeed 0, hitPoints 1, attackDamage 2, attackRange 3, attackCooldown 4, attackEndlag 5, defence 6, cost 7, size 8
    public static float[] guard1 = new float[] { 2f, 10f, 2f, 2f, 1.2f, 0.2f, 0f, 100f, 0.4f };   // 0
    public static float[] enemy1 = new float[] { 3f, 3f, 1f, 2f, 2.5f, 0.4f, 0f, 0f, 0.4f };      // 1
    public static float[] fent = new float[] { 0f, 100f, 0f, 0f, 99f, 99f, 1f, 300f, 1f };      // 2
    public static float[] rangedEnemy = new float[] { 2f, 3f, 4f, 6f, 1.25f, 0.4f, 0f, 0f, 0.4f };      // 3
    public static float[] mortar = new float[] { 1f, 5f, 9f, 16f, 4.5f, 1.25f, 0f, 700f, 0.6f };      // 4
    public static string guard1Image = "playershitteri-removebg";
    public static string enemy1Image = "FentFiend(1)";
    public static string fentImage = "TOWER";
    public static string rangedImage = "sotilas_1";
    public static string mortarImage = "mortar_troop";
    public static string guard1Desc = "a Basic unit capable of doing minor damage to enemies, useful in smaller engagements but inefficient against more advanced units";
    public static string enemy1Desc = "this is a placeholder";
    public static string fentDesc = "a Guard tower which works as a distraction for the enemy, and can boost the attack rate of nearby units.";
    public static string rangedDesc = "PLACEHOLDER PLACEHOLDER! PLACEHOLDER...";
    public static string mortarDesc = "Able to bombard enemies with precision and power, but struggles up close due to friendly fire.";
    public static List<string> IDList = new() { "guard1", "enemy1", "tower", "rangedEnemy", "mortar" };
    public static List<float[]> unitStats = new() { guard1, enemy1, fent, rangedEnemy, mortar };
    public static List<string> unitImages = new() { guard1Image, enemy1Image, fentImage, rangedImage, mortarImage };
    public static List<string> unitDescriptions = new() { guard1Desc, enemy1Desc, fentDesc, rangedDesc, mortarDesc };
}