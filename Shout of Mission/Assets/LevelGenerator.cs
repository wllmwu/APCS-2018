using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour {
  private int rows = 10;
  private int cols = 10;
  private int[,] map;
  private int spawnRow = 0;
  private int spawnCol = 0;
  private enum MapModule : int {
    Empty = 0,
    NS, // 1
    EW, // 2
    NE, // 3
    ES, // 4
    SW, // 5
    WN, // 6
    NES, // 7
    ESW, // 8
    SWN, // 9
    WNE, // 10
    NESW, // 11
    Border // 12
  };
  private MapModule[] modules = new MapModule[] { MapModule.Empty, MapModule.NS, MapModule.EW, MapModule.NE, 
    MapModule.ES, MapModule.SW, MapModule.WN, MapModule.NES, MapModule.ESW, MapModule.SWN, 
    MapModule.WNE, MapModule.NESW, MapModule.Border };
  private MapModule[][] northConnections;
  private MapModule[][] eastConnections;
  private MapModule[][] southConnections;
  private MapModule[][] westConnections;
  private MapModule[] southConnectors;
  private MapModule[] noSouthConnectors;
  private MapModule[] westConnectors;
  private MapModule[] noWestConnectors;
  private MapModule[] northConnectors;
  private MapModule[] noNorthConnectors;
  private MapModule[] eastConnectors;
  private MapModule[] noEastConnectors;

	// Use this for initialization
	void Start () {
		InitializeModules();
    SetUpMap();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

  void SetUpMap() {
    InitializeMap();
    PickSpawnRoomLocation();
    FixDeadEnds();
    DisplayMap();
  }

  void InitializeModules () {
    southConnectors = new MapModule[] { MapModule.Empty, MapModule.NS, MapModule.ES, MapModule.SW, MapModule.NES, MapModule.ESW, MapModule.SWN, MapModule.NESW };
    noSouthConnectors = new MapModule[] { MapModule.Empty, MapModule.EW, MapModule.NE, MapModule.WN, MapModule.WNE, MapModule.Border };
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

    westConnectors = new MapModule[] { MapModule.Empty, MapModule.EW, MapModule.SW, MapModule.WN, MapModule.ESW, MapModule.SWN, MapModule.WNE, MapModule.NESW };
    noWestConnectors = new MapModule[] { MapModule.Empty, MapModule.NS, MapModule.NE, MapModule.ES, MapModule.NES, MapModule.Border };
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

    northConnectors = new MapModule[] { MapModule.Empty, MapModule.NS, MapModule.NE, MapModule.WN, MapModule.NES, MapModule.SWN, MapModule.WNE, MapModule.NESW };
    noNorthConnectors = new MapModule[] { MapModule.Empty, MapModule.EW, MapModule.ES, MapModule.SW, MapModule.ESW, MapModule.Border };
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

    eastConnectors = new MapModule[] { MapModule.Empty, MapModule.EW, MapModule.NE, MapModule.ES, MapModule.NES, MapModule.ESW, MapModule.WNE, MapModule.NESW };
    noEastConnectors = new MapModule[] { MapModule.Empty, MapModule.NS, MapModule.SW, MapModule.WN, MapModule.SWN, MapModule.Border };
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
      map[0, c] = (int)MapModule.Border;
      map[rows - 1, c] = (int)MapModule.Border;
    }
    for (int r = 1; r < rows - 1; r++) {
      map[r, 0] = (int)MapModule.Border;
      map[r, cols - 1] = (int)MapModule.Border;
    }

    Random.InitState(System.DateTime.Now.Millisecond);
    for (int r = 1; r < rows - 1; r++) {
      for (int c = 1; c < cols - 1; c++) {
        List<MapModule> valid = GetValidMapModules (r, c);
        map[r, c] = (int)(valid[Random.Range (0, valid.Count)]);
      }
    }
  }

  List<MapModule> GetValidMapModules (int row, int col) {
    List<MapModule> valid = new List<MapModule>();

    for (int i = 0; i < modules.Length - 1; i++) {
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

  void PickSpawnRoomLocation() {
    List<int> rowValues = new List<int>();
    List<int> colValues = new List<int>();
    for (int r = 2; r < rows - 2; r++) {
      for (int c = 2; c < cols - 2; c++) {
        if (map[r, c] == (int)MapModule.NESW) {
          rowValues.Add(r);
          colValues.Add(c);
        }
      }
    }

    if (rowValues.Count > 0) {
      int x = Random.Range(0, rowValues.Count);
      spawnRow = rowValues[x];
      spawnCol = colValues[x];
    }
    else {
      SetUpMap();
    }
  }

  void FixDeadEnds() {
    for (int r = 1; r < rows - 1; r++) {
      for (int c = 1; c < cols - 1; c++) {
        int looseEnds = CountLooseEnds(r, c);
        if (map[r, c] == (int)MapModule.Empty && looseEnds > 0) {
          TieLooseEnds(r, c, looseEnds);
        }
      }
    }
  }

  int CountLooseEnds(int r, int c) {
    int looseEnds = 0;
    looseEnds += ArrayContains(southConnectors, map[r - 1, c]) ? 1 : 0;
    looseEnds += ArrayContains(westConnectors, map[r, c + 1]) ? 1 : 0;
    looseEnds += ArrayContains(northConnectors, map[r + 1, c]) ? 1 : 0;
    looseEnds += ArrayContains(eastConnectors, map[r, c - 1]) ? 1 : 0;
    /*if (ArrayContains(southConnectors, map[r - 1, c])) {
      looseEnds++;
    }
    if (ArrayContains(westConnectors, map[r, c + 1])) {
      looseEnds++;
    }*/
    return looseEnds;
  }

  void TieLooseEnds(int r, int c, int looseEnds) {
    if (looseEnds == 4) {
      map[r, c] = (int)MapModule.NESW;
    }
    else {
      bool north = ArrayContains(southConnectors, map[r - 1, c]);
      bool east = ArrayContains(westConnectors, map[r, c + 1]);
      bool south = ArrayContains(northConnectors, map[r + 1, c]);
      bool west = ArrayContains(eastConnectors, map[r, c - 1]);
      if (looseEnds == 1) {
        //
      }
      else {
        if (north && east) {
          if (south) map[r, c] = (int)MapModule.NES;
          else if (west) map[r, c] = (int)MapModule.WNE;
          else map[r, c] = (int)MapModule.NE;
        }
        else if (east && south) {
          if (west) map[r, c] = (int)MapModule.ESW;
          else map[r, c] = (int)MapModule.ES;
        }
        else if (south && west) {
          if (north) map[r, c] = (int)MapModule.SWN;
          else map[r, c] = (int)MapModule.SW;
        }
        else if (west && north) {
          map[r, c] = (int)MapModule.WN;
        }
      }
    }
  }

  void DisplayMap () {
    string output = "";
    string numbers = "";
    for (int r = 0; r < rows; r++) {
      for (int c = 0; c < cols; c++) {
        numbers += "" + map[r,c] + " ";
        switch(map[r,c]) {
          case 0: // empty
            output += "O";
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
      numbers += "\n";
      output += "\n";
    }
    Debug.Log(numbers);
    Debug.Log(output);
  }
}
