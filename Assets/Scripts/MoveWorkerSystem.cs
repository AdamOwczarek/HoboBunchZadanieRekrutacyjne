using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Mono.Cecil;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Vector3 = UnityEngine.Vector3;

public class MoveWorkerSystem : MonoBehaviour
{
    public List<Vector3> extractorList = new List<Vector3>();
    public List<Vector3> productionList = new List<Vector3>();
    public  List<Vector3> warehouseList = new List<Vector3>();

    private bool firstExtractor = true;
    private bool firstProd = true;
    private bool firstWarehouse = true;
    
    public IEnumerator WaitForProd(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
    }

    private enum CarriedResourceEnum
    { 
        Wood, 
        Chair,
        Null
    }

    private CarriedResourceEnum carriedResource = CarriedResourceEnum.Null;
    
    public GameResourceSO woodResourceSO;
    public GameResourceSO chairResourceSO;
    public GameResourcesList resourcesList;
    [SerializeField]
    public Vector3 destination;
    [SerializeField][Range(0,3)]
    float speed=.8f;
    private Vector3 fastestExtraction;
    private Vector3 fastestProd;
    private Vector3 fastestWarehouse;
    private string message = $"Carrying: " + CarriedResourceEnum.Null;
    [SerializeField]
    public TextMesh messageTextBox;
   
    void OnMouseOver()
    {
        messageTextBox.text = message;
    }
    void OnMouseExit()
    {
        messageTextBox.text = null;
    }
     
     void FixedUpdate()
     {
         if (firstExtractor&&extractorList.Count==1)
        {
            fastestExtraction = extractorList[0];
            destination = fastestExtraction;
            firstExtractor = false;
        }
        if (firstProd&&productionList.Count==1)
        {
            fastestProd = productionList[0];
            firstProd = false;
            destination = fastestProd;
        }
        if (firstWarehouse&&warehouseList.Count==1)
        {
            fastestWarehouse = warehouseList[0];
            firstWarehouse = false;
            destination = fastestProd;
        }
            
        this.transform.position = Vector3.Lerp(transform.position, destination, speed * Time.deltaTime);
        
    }
    
   public void OnTriggerEnter(Collider collision)
    {
        bool hitProd = collision.gameObject.CompareTag($"ProductionBuilding");
        bool hitWarehouse = collision.gameObject.CompareTag($"Warehouse");
        bool hitExtract = collision.gameObject.CompareTag($"ExtractionBuilding");
       
        
        if (carriedResource == CarriedResourceEnum.Wood & hitProd)
        {
            GameResourcesList gameResourcesClass = collision.gameObject.GetComponent<GameResourcesList>();
            gameResourcesClass.Add(woodResourceSO, 1);
            this.resourcesList.TryUse(woodResourceSO,1);
            carriedResource = CarriedResourceEnum.Null;
            message = $"Carrying: " + carriedResource;
            if (gameResourcesClass.TryUse(chairResourceSO,1))
            {
                this.resourcesList.Add(chairResourceSO, 1);
                carriedResource = CarriedResourceEnum.Chair;
                message = $"Carrying: " + carriedResource;
                foreach (Vector3 warehousePosition in warehouseList)
                {
                    if ((Vector3.Distance(this.transform.position, warehousePosition) <
                         (Vector3.Distance(this.transform.position, fastestWarehouse))))
                    {
                        fastestWarehouse = warehousePosition;
                    }
                }

                destination = fastestWarehouse;
            }
            else
            {
                foreach (Vector3 extractorPosition in extractorList)
                {
                    if ((Vector3.Distance(this.transform.position, extractorPosition) <
                         (Vector3.Distance(this.transform.position, fastestExtraction))))
                    {
                        fastestExtraction = extractorPosition;
                    }
                }

                destination = fastestExtraction;
            }
           
        }
        else if (carriedResource == CarriedResourceEnum.Chair & hitWarehouse)
        {
            GameResourcesList gameResourcesClass = collision.gameObject.GetComponent<GameResourcesList>();
            gameResourcesClass.Add(chairResourceSO, 1);
            this.resourcesList.TryUse(chairResourceSO,1);
            carriedResource = CarriedResourceEnum.Null;
            message = $"Carrying: " + carriedResource;
            foreach (Vector3 extractorPosition in extractorList)
            {
                if ((Vector3.Distance(this.transform.position, extractorPosition) <
                     (Vector3.Distance(this.transform.position, fastestExtraction))))
                {
                    fastestExtraction = extractorPosition;
                }
            }

            destination = fastestExtraction;
        }
        else if (carriedResource == CarriedResourceEnum.Null & hitExtract)
        {
            GameResourcesList gameResourcesClass = collision.gameObject.GetComponent<GameResourcesList>();
            bool canTake = gameResourcesClass.TryUse(woodResourceSO, 1);
           if (!canTake)
           {
               StartCoroutine(WaitForProd(5.0f));
           }
           gameResourcesClass.TryUse(woodResourceSO, 1);
           this.resourcesList.Add(woodResourceSO,1);
           carriedResource = CarriedResourceEnum.Wood;
           message = $"Carrying: " + carriedResource;
            foreach (Vector3 prodPosition in productionList)
            {
                if ((Vector3.Distance(this.transform.position, prodPosition) <
                     (Vector3.Distance(this.transform.position, fastestProd))))
                {
                    fastestProd = prodPosition;
                }
            }

            destination = fastestProd;
          
        }
        else
        {
            if ((carriedResource == CarriedResourceEnum.Chair) & (hitProd || hitExtract))
            {
                destination = fastestWarehouse;
            }
            destination = fastestExtraction;
        }
    }
}
