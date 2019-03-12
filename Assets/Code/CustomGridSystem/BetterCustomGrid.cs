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
    public LayerMask ghostBuildingMask;
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
            foreach(Node n in grid){
                    bool theresGhost = (Physics.CheckSphere(n.worldPosition, nodeRadius, ghostBuildingMask));
                    n.theresGhost = theresGhost;

                    bool theresBuilding = (Physics.CheckSphere(n.worldPosition, nodeRadius, buildingMask));
                    n.theresBuilding = theresBuilding;

                    if((Physics.CheckSphere(n.worldPosition, nodeRadius, mouseMask))){
                        n.walkable = false;
                        if(Input.GetMouseButtonDown(0) && MainCamera.GetComponent<MouseOnGrid>().CanPlace(n.worldPosition, nodeRadius) && !n.isWater && !n.theresBuilding){
                            if(previeuwObjects.Count >= 1){
                                foreach(GameObject g in previeuwObjects){
                                    Destroy(g);
                                }
                                previeuwObjects.Clear();
                            }
                            previeuwObjects.Clear();
                            MainCamera.GetComponent<MouseOnGrid>().PlaceBuilding(n.worldPosition.x, n.worldPosition.z);
                            n.clickedOn = true;
                        }
                    } else {
                        n.walkable = true;
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

                if(n.theresBuilding){
                    Gizmos.color = Color.black;
                }

                if(n.theresGhost){
                    Gizmos.color = Color.magenta;
                }

                if(!n.walkable){
                    Gizmos.color = Color.red;
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
                if(n.isCloseToWater && !n.theresBuilding && !n.hasMineral){
                    GameObject newObj = Instantiate(previeuwObject, new Vector3(n.worldPosition.x, transform.position.y, n.worldPosition.z), transform.rotation);
                    newObj.transform.parent = previeuwsParent.transform;
                    previeuwObjects.Add(newObj);
                }
            }
            break;
            
            //Farm
            case 2:
            if(previeuwObjects.Count >= 1){
                foreach(GameObject g in previeuwObjects){
                    Destroy(g);
                }
                previeuwObjects.Clear();
            }
            

            foreach(Node n in grid){
                if(!n.isCloseToWater && !n.theresBuilding && !n.hasMineral){
                    GameObject newObj = Instantiate(previeuwObject, new Vector3(n.worldPosition.x, transform.position.y, n.worldPosition.z), transform.rotation);
                    newObj.transform.parent = previeuwsParent.transform;
                    previeuwObjects.Add(newObj);
                }
            }
            break;

            //Pod
            case 3:
            if(previeuwObjects.Count >= 1){
                foreach(GameObject g in previeuwObjects){
                    Destroy(g);
                }
                previeuwObjects.Clear();
            }
            

            foreach(Node n in grid){
                if(!n.isCloseToWater && !n.theresBuilding && !n.hasMineral){
                    GameObject newObj = Instantiate(previeuwObject, new Vector3(n.worldPosition.x, transform.position.y, n.worldPosition.z), transform.rotation);
                    newObj.transform.parent = previeuwsParent.transform;
                    previeuwObjects.Add(newObj);
                }
            }
            break;

            //Mine
            case 4:
            if(previeuwObjects.Count >= 1){
                foreach(GameObject g in previeuwObjects){
                    Destroy(g);
                }
                previeuwObjects.Clear();
            }
            

            foreach(Node n in grid){
                if(!n.theresBuilding && n.hasMineral){
                    GameObject newObj = Instantiate(previeuwObject, new Vector3(n.worldPosition.x, transform.position.y, n.worldPosition.z), transform.rotation);
                    newObj.transform.localScale = new Vector3(newObj.transform.localScale.x , newObj.transform.localScale.y * 3 , newObj.transform.localScale.z);
                    newObj.transform.parent = previeuwsParent.transform;
                    previeuwObjects.Add(newObj);
                }
            }
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