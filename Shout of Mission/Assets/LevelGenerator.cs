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
  private MapModule[][] northConnections;
  private MapModule[][] eastConnections;
  private MapModule[][] southConnections;
  private MapModule[][] westConnections;

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
    MapModule[] noSouthConnectors = new MapModule[] { MapModule.EW, MapModule.NE, MapModule.WN, MapModule.WNE };
    northConnections = new MapModule[][] {
      noSouthConnectors, // empty
      southConnectors, // NS
      noSouthConnectors, // EW
      southConnectors, // NE
      noSouthConnectors, // ES
      noSouthConnectors, // SW
      southConnectors, // WN
      southConnectors, // NES
      noSouthConnectors, // ESW
      southConnectors, // SWN
      southConnectors, // WNE
      southConnectors // NESW
    };

    MapModule[] westConnectors = new MapModule[] { MapModule.EW, MapModule.SW, MapModule.WN, MapModule.ESW, MapModule.SWN, MapModule.WNE, MapModule.NESW };
    MapModule[] noWestConnectors = new MapModule[] { MapModule.NS, MapModule.NE, MapModule.ES, MapModule.NES };
    eastConnections = new MapModule[][] {
      noWestConnectors, // empty
      noWestConnectors, // NS
      westConnectors, // EW
      westConnectors, // NE
      westConnectors, // ES
      noWestConnectors, // SW
      noWestConnectors, // WN
      westConnectors, // NES
      westConnectors, // ESW
      noWestConnectors, // SWN
      westConnectors, // WNE
      westConnectors // NESW
    };

    MapModule[] northConnectors = new MapModule[] { MapModule.NS, MapModule.NE, MapModule.WN, MapModule.NES, MapModule.SWN, MapModule.WNE, MapModule.NESW };
    MapModule[] noNorthConnectors = new MapModule[] { MapModule.EW, MapModule.ES, MapModule.SW, MapModule.ESW };
    southConnections = new MapModule[][] {
      noNorthConnectors, // empty
      northConnectors, // NS
      noNorthConnectors, // EW
      noNorthConnectors, // NE
      northConnectors, // ES
      northConnectors, // SW
      noNorthConnectors, // WN
      northConnectors, // NES
      northConnectors, // ESW
      northConnectors, // SWN
      noNorthConnectors, // WNE
      northConnectors // NESW
    };

    MapModule[] eastConnectors = new MapModule[] { MapModule.EW, MapModule.NE, MapModule.ES, MapModule.NES, MapModule.ESW, MapModule.WNE, MapModule.NESW };
    MapModule[] noEastConnectors = new MapModule[] { MapModule.NS, MapModule.SW, MapModule.WN, MapModule.SWN };
    westConnections = new MapModule[][] {
      noEastConnectors, // empty
      noEastConnectors, // NS
      eastConnectors, // EW
      noEastConnectors, // NE
      noEastConnectors, // ES
      eastConnectors, // SW
      eastConnectors, // WN
      noEastConnectors, // NES
      eastConnectors, // ESW
      eastConnectors, // SWN
      eastConnectors, // WNE
      eastConnectors // NESW
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

    Random.InitState(System.DateTime.Now.Millisecond);
    for (int r = 1; r < rows - 1; r++) {
      for (int c = 1; c < cols - 1; c++) {
        List<MapModule> valid = GetValidMapModules (r, c);
        map[r, c] = (int)valid[UnityEngine.Random.Range (0, valid.Count)];
      }
    }
  }

  List<MapModule> GetValidMapModules (int row, int col) {
    List<MapModule> valid = new List<MapModule>();

    for (int i = 0; i < modules.Length; i++) {
      MapModule m = modules[i];
      if (ArrayContains(northConnections[i], map[row - 1, col]) &&
          ArrayContains(eastConnections[i], map[row, col + 1]) &&
          ArrayContains(southConnections[i], map[row + 1, col]) &&
          ArrayContains(westConnections[i], map[row, col - 1])) {
        valid.Add(m);
      }
    }

    if (valid.Count == 0) {
      valid.Add(MapModule.Empty);
    }

    return valid;
  }

  bool ArrayContains(MapModule[] array, int x) {
    foreach (MapModule m in array) {
      if (m == (MapModule)x) {
        return true;
      }
    }
    return false;
  }

  void DisplayMap () {
    string output = "";
    for (int r = 0; r < rows; r++) {
      for (int c = 0; c < cols; c++) {
        switch(map[r,c]) {
          case 0: // empty
            output += " ";
            break;
          case 1: // NS
            output += "\u2502";
            break;
          case 2: // EW
            output += "\u2500";
            break;
          case 3: // NE
            output += "\u2514";
            break;
          case 4: // ES
            output += "\u250c";
            break;
          case 5: // SW
            output += "\u2510";
            break;
          case 6: // WN
            output += "\u2518";
            break;
          case 7: // NES
            output += "\u251c";
            break;
          case 8: // ESW
            output += "\u252c";
            break;
          case 9: // SWN
            output += "\u2524";
            break;
          case 10: // WNE
            output += "\u2534";
            break;
          case 11: // NESW
            output += "\u253c";
            break;
          default:
            output += "X";
            break;
        }
      }
      output += "\n";
    }
    Debug.Log(output);
  }
}
