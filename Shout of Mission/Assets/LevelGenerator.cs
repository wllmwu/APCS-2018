using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour {
  private int rows = 10;
  private int cols = 10;
  private int[,] map;
  private enum MapModule : int {
    Empty = 0,
    NS,
    EW,
    NE,
    ES,
    SW,
    WN,
    NES,
    ESW,
    SWN,
    WNE,
    NESW
  };
  private MapModule[] modules = { MapModule.Empty, MapModule.NS, MapModule.EW, MapModule.NE, 
    MapModule.ES, MapModule.SW, MapModule.WN, MapModule.NES, MapModule.ESW, MapModule.SWN, 
    MapModule.WNE, MapModule.NESW };
  private MapModule[,] upConnections;
  private MapModule[,] downConnections;
  private MapModule[,] leftConnections;
  private MapModule[,] rightConnections;

	// Use this for initialization
	void Start () {
		InitializeModules ();
    InitializeMap ();
    DisplayMap ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

  void InitializeModules () {
    //
  }

  void InitializeMap () {
    //
  }

  List<MapModule> getValidMapModules (int row, int col) {
    List<MapModule> valid = new List<MapModule>();

    for (int i = 0; i < modules.Length; i++) {
      MapModule m = modules[i];
    }

    return valid;
  }

  void DisplayMap () {
    //
  }
}
