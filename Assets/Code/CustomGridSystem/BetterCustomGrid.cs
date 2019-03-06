using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BetterCustomGrid : MonoBehaviour
{
    public GameObject MainCamera;
    [Range(2,100)]
    public int spawnMineralsOneIn = 2;
    public GameObject mineralsParent;
    public GameObject previeuwsParent;
    public GameObject[] mineralPrefabs;
    public LayerMask mouseMask;
    public LayerMask groundMask;
    public LayerMask closeToWaterMask;
    public LayerMask buildingMask;
    public Vector2 gridWorldSize;
    public float nodeRadius;
    public GameObject previeuwObject;
    private List<GameObject> previeuwObjects = new List<GameObject>();
    Node[,] grid;

    float nodeDiameter;
    int gridSizeX, gridSizeY;

    void Start(){
        nodeDiameter = nodeRadius*2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x/nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y/nodeDiameter);
        CreateGrid();
    }

    void CreateGrid(){
        grid = new Node[gridSizeX,gridSizeY];
        Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x/2 - Vector3.forward * gridWorldSize.y/2;

        for(int x = 0; x < gridSizeX; x++){
            for(int y = 0; y < gridSizeY; y++){
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
                bool walkable = !(Physics.CheckSphere(worldPoint, nodeRadius, mouseMask));
                bool isWater = !(Physics.CheckSphere(worldPoint, nodeRadius, groundMask));
                bool isCloseToWater = (Physics.CheckSphere(worldPoint, nodeRadius, closeToWaterMask));
                bool theresBuilding = (Physics.CheckSphere(worldPoint, nodeRadius, buildingMask));
                
                grid[x,y] = new Node(walkable, worldPoint);
                grid[x,y].isWater = isWater;
                grid[x,y].isCloseToWater = isCloseToWater;
                grid[x,y].theresBuilding = theresBuilding;
                if(Random.Range(0,spawnMineralsOneIn) == 1 && !grid[x,y].isWater){
                    grid[x,y].hasMineral = true;
                    GameObject mineral = Instantiate(mineralPrefabs[Random.Range(0,mineralPrefabs.Length)], worldPoint, this.transform.rotation);
                    mineral.transform.parent = mineralsParent.transform;
                }
            }
        }
    }

    void Update(){
        if(grid != null){
            for(int x = 0; x < gridSizeX; x++){
                for(int y = 0; y < gridSizeY; y++){
                    Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x/2 - Vector3.forward * gridWorldSize.y/2;
                    Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
                    bool theresBuilding = (Physics.CheckSphere(worldPoint, nodeRadius, buildingMask));
                    grid[x,y].theresBuilding = theresBuilding;

                    if((Physics.CheckSphere(grid[x,y].worldPosition, nodeRadius, mouseMask))){
                        grid[x,y].walkable = false;
                        if(Input.GetMouseButtonDown(0) && MainCamera.GetComponent<MouseOnGrid>().CanPlace(worldPoint, nodeRadius) && !grid[x,y].isWater && !grid[x,y].theresBuilding){
                            if(previeuwObjects.Count >= 1){
                                foreach(GameObject g in previeuwObjects){
                                    Destroy(g);
                                }
                                previeuwObjects.Clear();
                            }
                            previeuwObjects.Clear();
                            MainCamera.GetComponent<MouseOnGrid>().PlaceBuilding(grid[x,y].worldPosition.x, grid[x,y].worldPosition.z);
                            grid[x,y].clickedOn = true;
                        }
                    } else {
                        grid[x,y].walkable = true;
                    }
                }
            }
        }
    }

    void OnDrawGizmos(){
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));

        if(grid != null){
            foreach(Node n in grid){
                Gizmos.color = Color.white;

                if(n.hasMineral){
                    Gizmos.color = Color.blue;
                }

                

                if(n.isCloseToWater){
                    Gizmos.color = Color.yellow;
                }

                if(n.isWater){
                    Gizmos.color = Color.cyan;
                }

                if(n.clickedOn){
                    Gizmos.color = Color.green;
                }

                if(!n.walkable){
                    Gizmos.color = Color.red;
                }

                if(n.theresBuilding){
                    Gizmos.color = Color.black;
                }
                
                Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter-0.1f));
            }
        }
    }

    public void WhereToPlace(int bType){
        switch (bType){
            //Energy
            case 1:
            if(previeuwObjects.Count >= 1){
                foreach(GameObject g in previeuwObjects){
                    Destroy(g);
                }
                previeuwObjects.Clear();
            }
            

            foreach(Node n in grid){
                if(n.isCloseToWater && !n.theresBuilding){
                    GameObject newObj = Instantiate(previeuwObject, new Vector3(n.worldPosition.x, transform.position.y, n.worldPosition.z), transform.rotation);
                    newObj.transform.parent = previeuwsParent.transform;
                    previeuwObjects.Add(newObj);
                }
            }
            break;
            
            //Farm
            case 2:

            break;

            //Pod
            case 3:
            
            break;
        }
    }
}

//Shitty extra Code------------------------------------------------------------
//for(int x = 0; x < gridSizeX; x++){
//     for(int y = 0; y < gridSizeY; y++){
//         if((Physics.CheckSphere(grid[x,y].worldPosition, nodeRadius, mouseMask))){
//             grid[x,y].walkable = false;
//             if(Input.GetMouseButtonDown(0) && !grid[x,y].isWater){
//                 grid[x,y].clickedOn = true;
//             }
//         } else {
//             grid[x,y].walkable = true;
//         }
//     }
// }