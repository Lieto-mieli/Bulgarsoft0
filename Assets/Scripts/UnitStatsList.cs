using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class UnitStatsList : MonoBehaviour
{
    // moveSpeed 0, hitPoints 1, attackDamage 2, attackRange 3, attackCooldown 4, attackEndlag 5, defence 6, cost 7
    public static float[] guard1 = new float[] { 2f, 10f, 2f, 2f, 1.2f, 0.2f, 0f, 100f, 0.4f };   // 0
    public static float[] enemy1 = new float[] { 3f, 3f, 1f, 2f, 2.5f, 0.4f, 0f, 0f, 0.5f };      // 1
    public static float[] fent = new float[] { 0f, 100f, 0f, 0f, 99f, 99f, 1f, 500f, 1f };      // 2
    public static float[] rangedEnemy = new float[] { 3f, 3f, 1f, 8f, 2.5f, 0.4f, 0f, 0f, 0.5f };      // 4
    public static string guard1Image = "playershitteri-removebg";
    public static string enemy1Image = "FentFiend(1)";
    public static string fentImage = "Fentanyl";
    public static string rangedImage = "sotilas_1";
    public static string guard1Desc = "t‰‰ on esimerkki";
    public static string enemy1Desc = "this is a placeholder";
    public static string fentDesc = "PLACEHOLDER PLACEHOLDER! PLACEHOLDER...";
    public static string rangedDesc = "PLACEHOLDER PLACEHOLDER! PLACEHOLDER...";
    public static List<string> IDList = new() { "guard1", "enemy1", "fent", "rangedEnemy" };
    public static List<float[]> unitStats = new() { guard1, enemy1, fent, rangedEnemy };
    public static List<string> unitImages = new() { guard1Image, enemy1Image, fentImage, rangedImage };
    public static List<string> unitDescriptions = new() { guard1Desc, enemy1Desc, fentDesc, rangedDesc };
}