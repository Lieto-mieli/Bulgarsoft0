using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.SceneTemplate;
using UnityEngine;
using static UnityEditor.Progress;

public class Selector : MonoBehaviour
{
    public Guard1AI guard1out;
    public GameObject[] guardOutlineList;
    public List<GameObject> currentlySelected;
    public GameObject selectArrow;
    float tempX;
    float tempY;
    float tempZ;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        UpdateSelect();
        if (currentlySelected.Count != 0)
        {
            selectArrow.SetActive(true);
            Vector3 temp = new Vector3(tempX, tempY + 0.5f, tempZ);
            selectArrow.transform.position = temp;
        }
        else { selectArrow.SetActive(false); }
    }
    public void UpdateSelect()
    {
        tempX = 0f;
        tempY = 0f;
        tempZ = 0f;
        foreach (var item in currentlySelected)
        {
            tempX += item.transform.position.x;
            tempY += item.transform.position.y;
            tempZ += item.transform.position.z;
        }
        tempX = tempX / currentlySelected.Count;
        tempY = tempY / currentlySelected.Count;
        tempZ = tempZ / currentlySelected.Count;
    }
    public void addObject(GameObject guard)
    {
        Debug.Log("ben");
        currentlySelected.Add(guard);
        if (guard.TryGetComponent<Guard1AI>(out guard1out) == true)
        {
            GameObject outline = Instantiate(guardOutlineList[0], guard.transform.position, new Quaternion());
            outline.GetComponent<Outline>().guardToFollow = guard;
        }
    }
    public void ClearSelect()
    {
        currentlySelected.Clear();
    }
}
