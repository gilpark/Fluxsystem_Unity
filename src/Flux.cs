using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Flux : MonoBehaviour {

	public static GameObject flux;
	public int _columns =1;
	public int _rows = 1;
	public bool debug = false;
	Cell[] tiles,temp_tiles; 
	//GameObject Goal = new GameObject("GOAL");
	private List<int> visitList;
	private bool run = false;

	//private float cell_width,cell_height,cell_depth;
	
	/*
		Pre-drawing the feild
	*/

	void _drawTiles(){
		//_temp_cells =  new List<Tile>();
		float cell_width = (float) (_columns * 0.1);
		float cell_height = (float) (_rows * 0.1);
		float feild_width = cell_width * _columns;
		float feild_height = cell_height * _rows;

	
			for (int id =0; id < _columns*_rows; id++){	
				int _x = id%_columns;
				int _z = id/_columns;
				
				float cell_x = Vector3.zero.x + (_x * cell_width);
				float cell_z = Vector3.zero.z + (_z * cell_height);
				_drawRect(cell_width, cell_height, cell_x, cell_z);
			}
			_drawBoundary(feild_width, feild_height, (float)(cell_width* 0.5));	
	}

	void _drawBoundary(float width, float height, float depth){
		Gizmos.color = new Color(0.9F,0.9F,0.0F,1.0F);
		//down
		Gizmos.DrawLine(new Vector3(0,-depth,0), new Vector3(width,-depth,0));
		Gizmos.DrawLine(new Vector3(width,-depth,0), new Vector3(width,-depth,height));
		Gizmos.DrawLine(new Vector3(width,-depth,height), new Vector3(0,-depth, height));
		Gizmos.DrawLine(new Vector3(0,-depth, height), new Vector3(0,-depth,0));

		//up
		Gizmos.DrawLine(new Vector3(0,depth,0), new Vector3(width,depth,0));
		Gizmos.DrawLine(new Vector3(width,depth,0), new Vector3(width,depth,height));
		Gizmos.DrawLine(new Vector3(width,depth,height), new Vector3(0,depth, height));
		Gizmos.DrawLine(new Vector3(0,depth, height), new Vector3(0,depth,0));
	}
    
	void _drawRect(float w, float h, float x, float z){
		//float x = Vector3.zero.x;
		float y = Vector3.zero.y;
		//float z = Vector3.zero.z;
		Gizmos.color = new Color(0.0F,0.8F,0.8F,0.2F);
		Gizmos.DrawLine(new Vector3(x, y, z), new Vector3(x + w, y, z));
		Gizmos.DrawLine(new Vector3(x + w, y, z), new Vector3(x + w, y, z + h));
		Gizmos.DrawLine(new Vector3(x + w, y, z + h), new Vector3(x, y, z + h));
		Gizmos.DrawLine(new Vector3(x, y, z + h), new Vector3(x, y, z));
	}
	
	/*
		Setup/initialization the feild
	*/

	void _initFeild(){

		tiles = new Cell[_columns * _rows]; //each cells
		//temp_tiles = new Cell[_columns * _rows];
		visitList = new List<int>();

		for(int id=0; id < _columns * _rows; id++){
			int _x = id%_columns;
			//int _y = 0; // for the later update in case of making 3d vector field
			int _z = id/_columns;

			float cell_width = (float)(_columns * 0.1 * transform.localScale.x); //x
			float cell_height = (float) (_rows * 0.1 * transform.localScale.z); //z
			float cell_depth = (float) (_columns * 0.1 * transform.localScale.y); //y

			float cell_x = Vector3.zero.x + (_x * cell_width);
			float cell_y = Vector3.zero.y; // for the later update in case of making 3d vector field
			float cell_z = Vector3.zero.z + (_z * cell_height);

			Vector3 cell_vec3 = new Vector3(cell_x,cell_y,cell_z);
			Vector3 cell_size = new Vector3(cell_width, cell_depth, cell_height);
			
			_createCells(_x, _z,id, cell_vec3, cell_size); //creating cells and adding to array

		}
	}

	/*
		Setup/initialization each cells
	*/
	void _createCells(int x, int y,int id, Vector3 pos, Vector3 size ){

		GameObject tile = new GameObject(id+"|" + x + "|"+y);
		tile.tag = "flux";

		tile.transform.parent = Flux.flux.transform;
		tile.transform.localPosition = new Vector3(0,0,0);

		BoxCollider tile_Collider = tile.AddComponent<BoxCollider>();
		Rigidbody tile_rigidbody = tile.AddComponent<Rigidbody>();
		tile_rigidbody.useGravity = false;
		tile_Collider.center = new Vector3(pos.x+(float)(size.x*0.5), pos.y, pos.z+(float)(size.z*0.5));
		tile_Collider.size = size;
		tile_Collider.isTrigger = true;
		
		tile.AddComponent<Cell>(); 

		Cell cell = tile.GetComponent<Cell>();
		tiles[id] = cell;
		cell.size = size;
		cell.id = id;
		cell.cols = _columns;
		cell.rows = _rows;
		_setNeighbors(id, cell);
		//copy tiles to temp
		//temp_tiles[id] = tiles[id];
		
	}

	void _setNeighbors(int id, Cell c){
		int column = id%_columns; 
	    int row = id/_columns;
	    if (column == 0) {
	      c.neighbors[1] = row * _columns + column+1;
	      c.neighbors[3] = -1; //left
	    } 
	    if (column == _columns-1) {
	      c.neighbors[1] = -1; //if right one is out of boundary
	      c.neighbors[3] = row * _columns + column-1;
	    } 
	    if (row == 0 ) {
	      c.neighbors[2] = -1; //if upper one is out of boundary
	      c.neighbors[0] = (row+1) * _columns + column;
	    } 
	    if (row == _rows-1) {
	      c.neighbors[2] = (row-1) * _columns + column;
	      c.neighbors[0] = -1; //down
	    } 
	    if (column > 0 && column < _columns-1) { // if they aer in the boundary
	      c.neighbors[1] = row * _columns + column+1;
	      c.neighbors[3] = row * _columns + column-1;
	    } 
	    if (row > 0 && row < _rows-1) {
	      c.neighbors[2] = (row-1) * _columns + column;
	      c.neighbors[0] = (row+1) * _columns + column;
	    }
	}
	
	
	//@ todo flux(id) --> reset the field --> for moving goals;
	void _Flux(int id){ //this is called when Goal-cell is triggered
		
			visitList.Add(id);
			while (visitList.Count != 0) {
		      int front = visitList[0]; //get the first one, we are always evalutating the first one
		      Cell target = tiles[front];
 			 
				_visitNeighbors(target); //Negative val 
 		
		       
		   	  visitList.RemoveAt(0);
		    }
		    visitList.Clear();//clear it every cycle;
			
		
	}
	void _visitNeighbors(Cell center) {
		
	    center.isPassable = false;
	  
	    int step = 1;
	    for (int i = 0; i < center.neighbors.Length; i++) {

	      if (center.neighbors[i] < 0 ) continue;      // check the boundary, OoB marked as -1
	     	//Cell t = temp_tiles[center.neighbors[i]];
	      Cell t = tiles[center.neighbors[i]];
	      if(t.block){
	      	center.neighbors[i] = -1;
//				t.GetComponent<Rigidbody> ().constraints = RigidbodyConstraints.FreezeAll;
//				t.GetComponent<BoxCollider> ().isTrigger = false;

	      }
	      if (!t.isPassable || t.isGoal)continue;
	      t.cost = center.cost + step; 
	      if (t.isPassable) {
	        t.isPassable=false;
	        visitList.Add(t.id);// add it self to the visitied list to find its neighbors
	      } 
	    }
  	}

  	void _calculateDirection(Cell center) {
  		//Cell center = (Cell) _center.GetComponent(typeof (Cell)); 
	    float X, Y;
	    float N=0, E=0, S=0, W = 0;
	    if (center.neighbors[0]>0){N = tiles[center.neighbors[0]].cost;}
	    else if(center.neighbors[0]<0){N = center.cost + 1;}
	   	
	   	if (center.neighbors[1]>0){E = tiles[center.neighbors[1]].cost;}
	    else if(center.neighbors[1]<0){E = center.cost + 1;}
	    
	    if (center.neighbors[2]>0){S = tiles[center.neighbors[2]].cost;}
	    else if(center.neighbors[2]<0){S = center.cost + 1;}
	   	
	   	if (center.neighbors[3]>0){W = tiles[center.neighbors[3]].cost;}
	    else if(center.neighbors[3]<0){W = center.cost + 1;}
		
		X = W-E;
	    Y = S-N;

	    //center.direction.set((-Y+X/2), (X+Y/2));
	    //center.direction.set((-Y+X/2+X/2), (X+Y/2+Y/2));
	    //check this article if interested
	    //http://www.math.uic.edu/coursepages/math210/labpages/lab7

	    if(center.block){
	    	center.direction =  new Vector3(0,0,0);
	    }else center.direction = new Vector3(X,0,Y); 
  }


	void Start () {
		flux = gameObject;
		_initFeild();//initialize feild
         StartCoroutine(WaitAndRun());
	}
	public IEnumerator WaitAndRun() {

		yield return new WaitForSeconds(3); //wait till all cells are ready
        
        print(" is WaitedAndRun " + Time.time);
        for(int id = 0; id < tiles.Length; id++){
        	
        	if(tiles[id].isGoal){
        		_Flux(id);
        		print("id : "+id +"is processing");
        	}
        	
        }
		foreach (Cell t in tiles){//calculate directions
			_calculateDirection(t);
		}
		run = true;
    }
	void Update () {

	}
	void OnDrawGizmos() {
		Gizmos.matrix = transform.localToWorldMatrix;
	 	if(!run){

	 		_drawTiles();
	 	}	
	}
}
