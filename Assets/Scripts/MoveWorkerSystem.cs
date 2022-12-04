using System.Collections;
using System.Collections.Generic;
using Mono.Cecil;
using Unity.VisualScripting;
using UnityEngine;

public class MoveWorkerSystem : MonoBehaviour
{
    private enum CarriedResourceEnum
    { 
        Wood, 
        Chair,
        Null
    }
    public GameResourceSO woodResourceSO;
    public GameResourceSO chairResourceSO;
    public GameResourcesList resourcesList;
    
    private Transform direction;

    private CarriedResourceEnum carriedResource = CarriedResourceEnum.Null;
    
    // Start is called before the first frame update
    void Start()
    {
        DetermineDirection();
        Move();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    void DetermineDirection()
    {
        
    }
    
    void Move()
    {
        
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
            if (gameResourcesClass.TryUse(chairResourceSO,1))
            {
                this.resourcesList.Add(chairResourceSO, 1);
                //change direction to Warehouse
            }
            else
            {
                //change direction to Extract
            }
           
        }
        else if (carriedResource == CarriedResourceEnum.Chair & hitWarehouse)
        {
            GameResourcesList gameResourcesClass = collision.gameObject.GetComponent<GameResourcesList>();
            gameResourcesClass.Add(chairResourceSO, 1);
            this.resourcesList.TryUse(chairResourceSO,1);
            //change direction to Extract
        }
        else if (carriedResource == CarriedResourceEnum.Null & hitExtract)
        {
            GameResourcesList gameResourcesClass = collision.gameObject.GetComponent<GameResourcesList>();
            gameResourcesClass.TryUse(woodResourceSO, 1);
            //wait until You its produced
            this.resourcesList.Add(woodResourceSO,1);
            //change direction to prod
          
        }
    }
}
