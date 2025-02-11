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
    public GameObject buildingSelected;
    public GameObject selectArrow;
    float tempX;
    float tempY;
    float tempZ;
    bool maalaa;
    // Start is called before the first frame update
    void Start()
    {
        maalaa = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (buildingSelected == null)
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
        else 
        {
            selectArrow.SetActive(true);
            Vector3 temp = new Vector3(buildingSelected.transform.position.x, buildingSelected.transform.position.y + 1f, buildingSelected.transform.position.z);
            selectArrow.transform.position = temp;
        }
        //maalaus
        if (maalaa)
        {
            if (!Input.GetMouseButton(0))
            {
                maalaa = false;
                //lopeta maalaus
            }
            else
            {
                //pidä maalaus yllä
            }
        }
        if (Input.GetMouseButtonDown(0))
        {
            //aloita maalaus
            maalaa = true;
        }
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
        tempX /= currentlySelected.Count;
        tempY /= currentlySelected.Count;
        tempZ /= currentlySelected.Count;
    }
    public void AddObject(GameObject guard)
    {
        DeselectBuilding();
        //Debug.Log("ben");
        currentlySelected.Add(guard);
        if (guard.TryGetComponent<Guard1AI>(out guard1out) == true)
        {
            GameObject outline = Instantiate(guardOutlineList[0], guard.transform.position, new Quaternion());
            outline.GetComponent<Outline>().guardToFollow = guard;
        }
    }
    public void RemoveObject(GameObject guard)
    {
        currentlySelected.Remove(guard);
    }
    public void SelectBuilding(GameObject building)
    {
        //Debug.Log("ben");
        buildingSelected = building;
        ClearSelect();
    }
    public void DeselectBuilding()
    {
        //Debug.Log("ben");
        buildingSelected = null;
    }
    public void ClearSelect()
    {
        currentlySelected.Clear();
    }
    public void CreateCameras()
    {
        if (buildingSelected != null)
        {

        }
        else
        {

        }
    }
}
