using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Device;
using UnityEngine.UI;

public class Selector : MonoBehaviour
{
    public new Camera camera;
    public Guard1AI guard1Out;
    public GameObject[] guardOutlineList;
    public GameObject buildingCamera;
    public Fent fentOut;
    public GameObject buildingMenu;
    public GameObject fentMenu;
    public GameObject guardMenu;
    public List<GameObject> currentlySelected;
    public GameObject buildingSelected;
    public GameObject selectArrow;
    public GameObject maalausBox;
    public List<Material> cameraMatList;
    public List<RenderTexture> cameraTexList;
    public List<GameObject> cameraList;
    public List<GameObject> cameraImageList;
    public Material exampleMaterial;
    public RenderTexture exampleTexture;
    public GameObject cameraImageParent;
    public GameObject cameraParent;
    Vector2 startPos;
    float tempX;
    float tempY;
    float tempZ;
    bool maalaa;
    // Start is called before the first frame update
    void Start()
    {
        camera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        maalaa = false;
    }
    void Update()
    {
        if (cameraList.Count != 0 && currentlySelected.Count != 0)
        {
            foreach(GameObject cam in cameraList)
            {
                Vector3 ben = currentlySelected[cameraList.IndexOf(cam)].transform.position;
                cam.GetComponent<Camera>().transform.position = new Vector3(ben.x,ben.y,0);
            }
        }
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
                //lopeta maalaus
                ClearSelect();
                DeselectBuilding();
                maalaa = false;
                Vector2 currentPos = camera.ScreenToWorldPoint(Input.mousePosition);
                Vector2 center = (startPos + currentPos) / 2;
                Vector3 scale = currentPos - startPos;
                scale.x = Mathf.Abs(scale.x);
                scale.y = Mathf.Abs(scale.y);
                scale.z = Mathf.Abs(scale.z);
                Collider2D[] results = Physics2D.OverlapBoxAll(center, scale, 0);
                int tempGuardCount = 0;
                GameObject tempBuilding = null;
                foreach (Collider2D collider in results)
                {
                    if (collider.gameObject.CompareTag("Guard"))
                    {
                        AddObject(collider.gameObject);
                        tempGuardCount++;
                    }
                    if (collider.gameObject.CompareTag("Building"))
                    {
                        if (tempBuilding == null)
                        {
                            tempBuilding = collider.gameObject;
                        }
                    }
                }
                //Debug.Log($"{tempGuardCount},{tempBuilding}");
                if(tempGuardCount == 0 && tempBuilding != null)
                {
                    SelectBuilding(tempBuilding);
                }
                maalausBox.SetActive(false);
            }
            else
            {
                //pidä maalaus yllä
                Vector2 currentPos = camera.ScreenToWorldPoint(Input.mousePosition);
                Vector2 center = (startPos + currentPos) / 2;
                Vector3 scale = (currentPos - startPos) * 45;
                scale.x = Mathf.Abs(scale.x);
                scale.y = Mathf.Abs(scale.y);
                scale.z = Mathf.Abs(scale.z);
                maalausBox.GetComponent<RectTransform>().sizeDelta = scale;
                //maalausBox.transform.localScale = scale/4;
                maalausBox.transform.position = camera.WorldToScreenPoint(center);
            }
        }
        if (Input.GetMouseButtonDown(0) && Input.mousePosition.x > 290)
        {
            //aloita maalaus
            maalaa = true;
            maalausBox.SetActive(true);
            startPos = camera.ScreenToWorldPoint(Input.mousePosition);
            Vector2 currentPos = camera.ScreenToWorldPoint(Input.mousePosition);
            Vector2 center = (startPos + currentPos) / 2;
            Vector3 scale = (currentPos - startPos) * 45;
            scale.x = Mathf.Abs(scale.x);
            scale.y = Mathf.Abs(scale.y);
            scale.z = Mathf.Abs(scale.z);
            maalausBox.GetComponent<RectTransform>().sizeDelta = scale;
            //maalausBox.transform.localScale = scale / 4;
            maalausBox.transform.position = center;
        }
        foreach (SelectSquareEdge thing in maalausBox.GetComponentsInChildren<SelectSquareEdge>())
        {
            if (thing.which == SelectSquareEdge.edge.N)
            {
                thing.temp = thing.trans.localScale;
                thing.temp.x = thing.rectTransform.sizeDelta.x + 2;
                thing.temp.y = 1;
                thing.trans.localScale = thing.temp;
            }
            if (thing.which == SelectSquareEdge.edge.E)
            {
                thing.temp = thing.trans.localScale;
                thing.temp.y = thing.rectTransform.sizeDelta.y + 2;
                thing.temp.x = 1;
                thing.trans.localScale = thing.temp;
            }
            if (thing.which == SelectSquareEdge.edge.S)
            {
                thing.temp = thing.trans.localScale;
                thing.temp.x = -thing.rectTransform.sizeDelta.x - 2;
                thing.temp.y = 1;
                thing.trans.localScale = thing.temp;
            }
            if (thing.which == SelectSquareEdge.edge.W)
            {
                thing.temp = thing.trans.localScale;
                thing.temp.y = -thing.rectTransform.sizeDelta.y - 2;
                thing.temp.x = 1;
                thing.trans.localScale = thing.temp;
            }
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
        //Debug.Log("ben");
        currentlySelected.Add(guard);
        if (guard.TryGetComponent<Guard1AI>(out guard1Out) == true)
        {
            GameObject outline = Instantiate(guardOutlineList[0], guard.transform.position, new Quaternion());
            outline.GetComponent<Outline>().guardToFollow = guard;
        }
        UpdateCameraPortraits();
    }
    public void RemoveObject(GameObject guard)
    {
        currentlySelected.Remove(guard);
        UpdateCameraPortraits();
    }
    public void SelectBuilding(GameObject building)
    {
        //Debug.Log("ben");
        ClearSelect();
        buildingSelected = building;
        UpdateCameraPortraits();
    }
    public void DeselectBuilding()
    {
        //Debug.Log("ben");
        buildingSelected = null;
        UpdateCameraPortraits();
    }
    public void ClearSelect()
    {
        currentlySelected.Clear();
    }
    public void UpdateCameraPortraits()
    {
        buildingMenu.SetActive(false);
        buildingCamera.SetActive(false);
        fentMenu.SetActive(false);
        guardMenu.SetActive(false);
        if (buildingSelected != null)
        {
            buildingMenu.SetActive(true);
            buildingCamera.SetActive(true);
            if (buildingSelected.TryGetComponent<Fent>(out fentOut))
            {
                fentMenu.SetActive(true);
                fentMenu.GetComponent<FentAbilities>().fent = buildingSelected;
            }
        }
        else
        {
            guardMenu.SetActive(true);
            ScorchedEarth(cameraMatList);
            ScorchedEarth(cameraTexList);
            ScorchedEarth(cameraList);
            ScorchedEarth(cameraImageList);
            cameraMatList.Clear();
            cameraTexList.Clear();
            cameraList.Clear();
            cameraImageList.Clear();
            foreach (GameObject selected in currentlySelected)
            {
                RenderTexture tempTex = new RenderTexture(exampleTexture);
                Material tempMat = new Material(exampleMaterial);
                GameObject tempCam = new GameObject();
                GameObject tempImage = new GameObject();
                tempCam.AddComponent<Camera>();
                tempCam.GetComponent<Camera>().targetTexture = tempTex;
                tempCam.GetComponent<Camera>().orthographic = true;
                tempCam.GetComponent<Camera>().orthographicSize = 1.0f;
                tempCam.GetComponent<Camera>().nearClipPlane = 0;
                tempCam.transform.parent = cameraParent.transform;
                tempMat.mainTexture = tempTex;
                tempImage.AddComponent<Image>().material = tempMat;
                int tempX = currentlySelected.IndexOf(selected)-(Convert.ToInt16(currentlySelected.IndexOf(selected)/3)*3);
                int tempY = Convert.ToInt16(currentlySelected.IndexOf(selected) / 3);
                tempX = -33 + (tempX * 33);
                tempY = 132 - (tempY * 33);
                tempImage.transform.position = new Vector2(tempX, tempY);
                tempImage.transform.SetParent(cameraImageParent.transform, false);
                tempImage.GetComponent<RectTransform>().sizeDelta = new Vector2(30, 30);
                cameraMatList.Add(tempMat);
                cameraTexList.Add(tempTex);
                cameraList.Add(tempCam);
                cameraImageList.Add(tempImage);
            }
        }
    }

    private void ScorchedEarth(List<Material> list)
    {
        foreach (Material obj in list)
        {
            Destroy(obj);
        }
    }
    private void ScorchedEarth(List<RenderTexture> list)
    {
        foreach (RenderTexture obj in list)
        {
            Destroy(obj);
        }
    }
    private void ScorchedEarth(List<Camera> list)
    {
        foreach (Camera obj in list)
        {
            Destroy(obj);
        }
    }
    private void ScorchedEarth(List<GameObject> list)
    {
        foreach (GameObject obj in list)
        {
            Destroy(obj);
        }
    }
}
