using System.Collections;
using System.Collections.Generic;
using Mono.Cecil;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class MoveWorkerSystem : MonoBehaviour
{
    public List<Vector3> extractorList = new List<Vector3>();
    public List<Vector3> productionList = new List<Vector3>();
    public  List<Vector3> warehouseList = new List<Vector3>();
    
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

    [SerializeField] public GameObject buildingContainer;
    
   //private Transform direction;
   // private Transform extractionTransform;
   // private Transform productionTransform;
    //private Transform warehouseTransform;
   
    
    // Start is called before the first frame update
    void Start()
    {
        
        FindBuildings();
        Move();
    }

    // Update is called once per frame
    void Update()
    {
       
        
    }
    
    void FindBuildings()
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
