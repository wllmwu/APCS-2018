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
  private MapModule[] modules = new MapModule[] { MapModule.Empty, MapModule.NS, MapModule.EW, MapModule.NE, 
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
    MapModule[] southConnectors = new MapModule[] { MapModule.NS, MapModule.ES, MapModule.SW, MapModule.NES, MapModule.ESW, MapModule.SWN, MapModule.NESW };

    upConnections = new MapModule[,] {
      { MapModule.Empty, MapModule.EW, MapModule.NE, MapModule.WN, MapModule.WNE },
      { MapModule.NS, MapModule.ES, MapModule.SW, MapModule.NES, MapModule.ESW, MapModule.SWN, MapModule.NESW },
      { MapModule.Empty, MapModule.EW, MapModule.NE, MapModule.WN, MapModule.WNE },
      { MapModule. }
    };
  }

  void InitializeMap () {
    map = new int[rows, cols];
    for (int c = 0; c < cols; c++) {
      map[0, c] = 0;
      map[rows - 1, c] = 0;
    }
    for (int r = 1; r < rows - 1; r++) {
      map[r, 0] = 0;
      map[r, cols - 1] = 0;
    }

    Random.seed = System.DateTime.Now.Millisecond;
    for (int r = 1; r < rows - 1; r++) {
      for (int c = 1; c < cols - 1; c++) {
        List<MapModule> valid = GetValidMapModules (r, c);
        map[r, c] = valid[Random.Range (0, valid.Length)];
      }
    }
  }

  List<MapModule> GetValidMapModules (int row, int col) {
    List<MapModule> valid = new List<MapModule>();

    for (int i = 0; i < modules.Length; i++) {
      MapModule m = modules[i];
      if (upConnections[i].Contains(ToObject(map[row - 1, col])) &&
          downConnections[i].Contains(ToObject(map[row + 1, col])) &&
          leftConnections[i].Contains(ToObject(map[row, col - 1])) &&
          rightConnections[i].Contains(ToObject(map[row, col + 1]))) {
        valid.Add(m);
      }
    }

    if (valid.Length == 0) {
      valid.Add(MapModule.Empty);
    }

    return valid;
  }

  MapModule ToModule (int x) {
    return (MapModule)Enum.ToObject(typeof(MapModule), x);
  }

  void DisplayMap () {
    //
  }
}
