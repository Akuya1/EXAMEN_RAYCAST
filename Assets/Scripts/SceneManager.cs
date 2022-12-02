using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField]private LayerMask rayLayer;
    [SerializeField]private LayerMask firstSceneLayer;
    [SerializeField]private LayerMask secondSceneLayer;
    [SerializeField]private LayerMask thirdSceneLayer;

    void LoadScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        if(Physics.Raycast(transform.position, transform.forward, out hit, 20f, rayLayer))
        {
            Vector3 hitPosition = hit.point;
            float hitDistance = hit.distance;
            string hitName = hit.transform.name;

        }
        if(Input.GetButtonDown("Fire1"))
        {
            
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit2;
            Debug.Log("hit");
            if(Physics.Raycast(ray,out hit2))
            {
                transform.position = new Vector3(hit2.point.x, transform.position.y, hit2.point.z);
                if(hit2.transform.gameObject.layer == 6)
                {
                    LoadScene(1);
                }
                else if(hit2.transform.gameObject.layer == 7)
                {
                    LoadScene(2);
                }
                else if(hit2.transform.gameObject.layer == 8)
                {
                  LoadScene(3);
                }
    
            }
        }
    }  
}