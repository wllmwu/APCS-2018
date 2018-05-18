using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour {
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
  private Dictionary<MapModule, MapModule> addSouthConnectors;
  private Dictionary<MapModule, MapModule> addWestConnectors;
  private Dictionary<MapModule, MapModule> addNorthConnectors;
  private Dictionary<MapModule, MapModule> addEastConnectors;

  public int rows = 10;
  public int cols = 10;
  private MapModule[,] map;
  private int spawnRow = 0;
  private int spawnCol = 0;
  public int moduleSize = 20;
  
  public GameObject spawnRoomPrefab;
  public GameObject[] NSPrefabs;
  public GameObject[] EWPrefabs;
  public GameObject[] NEPrefabs;
  public GameObject[] ESPrefabs;
  public GameObject[] SWPrefabs;
  public GameObject[] WNPrefabs;
  public GameObject[] NESPrefabs;
  public GameObject[] ESWPrefabs;
  public GameObject[] SWNPrefabs;
  public GameObject[] WNEPrefabs;
  public GameObject[] NESWPrefabs;

  public GameObject player;
  public bool spawnsPlayer = true;

  void Awake () {
    InitializeModules();
    SetUpMap();
  }
  
	// Use this for initialization
	void Start () {
    AssembleMap();
    if (spawnsPlayer) SpawnPlayer();
  }
	
	// Update is called once per frame
	void Update () {}

  private void InitializeModules () {
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

    addSouthConnectors = new Dictionary<MapModule, MapModule>() {
      { MapModule.EW, MapModule.ESW },
      { MapModule.NE, MapModule.NES },
      { MapModule.WN, MapModule.SWN },
      { MapModule.WNE, MapModule.NESW }
    };
    addWestConnectors = new Dictionary<MapModule, MapModule>() {
      { MapModule.NS, MapModule.SWN },
      { MapModule.NE, MapModule.WNE },
      { MapModule.ES, MapModule.ESW },
      { MapModule.NES, MapModule.NESW }
    };
    addNorthConnectors = new Dictionary<MapModule, MapModule>() {
      { MapModule.EW, MapModule.WNE },
      { MapModule.ES, MapModule.NES },
      { MapModule.SW, MapModule.SWN },
      { MapModule.ESW, MapModule.NESW }
    };
    addEastConnectors = new Dictionary<MapModule, MapModule>() {
      { MapModule.NS, MapModule.NES },
      { MapModule.SW, MapModule.ESW },
      { MapModule.WN, MapModule.WNE },
      { MapModule.SWN, MapModule.NESW }
    };
  }

  private void SetUpMap() {
    InitializeMap();
    PickSpawnRoomLocation();
    FixDeadEnds();
    DisplayMap();
  }

  private void InitializeMap () {
    map = new MapModule[rows, cols];
    for (int c = 0; c < cols; c++) {
      map[0, c] = MapModule.Border;
      map[rows - 1, c] = MapModule.Border;
    }
    for (int r = 1; r < rows - 1; r++) {
      map[r, 0] = MapModule.Border;
      map[r, cols - 1] = MapModule.Border;
    }

    Random.InitState(System.DateTime.Now.Millisecond);
    for (int r = 1; r < rows - 1; r++) {
      for (int c = 1; c < cols - 1; c++) {
        List<MapModule> valid = GetValidMapModules (r, c);
        map[r, c] = valid[Random.Range (0, valid.Count)];
      }
    }
  }

  private List<MapModule> GetValidMapModules (int row, int col) {
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

  private bool ArrayContains(MapModule[] array, MapModule x) {
    foreach (MapModule m in array) {
      if (m == x) {
        return true;
      }
    }
    return false;
  }

  private void PickSpawnRoomLocation() {
    List<int> rowValues = new List<int>();
    List<int> colValues = new List<int>();
    for (int r = 2; r < rows - 2; r++) {
      for (int c = 2; c < cols - 2; c++) {
        if (map[r, c] == MapModule.NESW) {
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

  private void FixDeadEnds() {
    for (int r = 1; r < rows - 1; r++) {
      for (int c = 1; c < cols - 1; c++) {
        int looseEnds = CountLooseEnds(r, c);
        if (map[r, c] == MapModule.Empty && looseEnds > 0) {
          TieLooseEnds(r, c, looseEnds);
        }
      }
    }
  }

  private int CountLooseEnds(int r, int c) {
    int looseEnds = 0;
    looseEnds += ArrayContains(southConnectors, map[r - 1, c]) ? 1 : 0;
    looseEnds += ArrayContains(westConnectors, map[r, c + 1]) ? 1 : 0;
    looseEnds += ArrayContains(northConnectors, map[r + 1, c]) ? 1 : 0;
    looseEnds += ArrayContains(eastConnectors, map[r, c - 1]) ? 1 : 0;
    return looseEnds;
  }

  private void TieLooseEnds(int r, int c, int looseEnds) {
    MapModule newModule = MapModule.Empty;
    if (looseEnds == 4) {
      newModule = MapModule.NESW;
    }
    else {
      bool north = ArrayContains(southConnectors, map[r - 1, c]);
      bool east = ArrayContains(westConnectors, map[r, c + 1]);
      bool south = ArrayContains(northConnectors, map[r + 1, c]);
      bool west = ArrayContains(eastConnectors, map[r, c - 1]);
      if (looseEnds == 1) {
        MapModule newLooseEnd = MapModule.Empty;
        if (!north && addSouthConnectors.TryGetValue((MapModule)map[r - 1, c], out newLooseEnd)) {
          map[r - 1, c] = newLooseEnd;
          north = true;
        }
        if (!east && addWestConnectors.TryGetValue((MapModule)map[r, c + 1], out newLooseEnd)) {
          map[r, c + 1] = newLooseEnd;
          east = true;
        }
        if (!south && addNorthConnectors.TryGetValue((MapModule)map[r + 1, c], out newLooseEnd)) {
          map[r + 1, c] = newLooseEnd;
          south = true;
        }
        if (!west && addEastConnectors.TryGetValue((MapModule)map[r, c - 1], out newLooseEnd)) {
          map[r, c - 1] = newLooseEnd;
          west = true;
        }
      }
      if (north && east) {
        if (south) {
          if (west) newModule = MapModule.NESW;
          else newModule = MapModule.NES;
        }
        else if (west) newModule = MapModule.WNE;
        else newModule = MapModule.NE;
      }
      else if (east && south) {
        if (west) newModule = MapModule.ESW;
        else newModule = MapModule.ES;
      }
      else if (south && west) {
        if (north) newModule = MapModule.SWN;
        else newModule = MapModule.SW;
      }
      else if (west && north) {
        newModule = MapModule.WN;
      }
      else if (north && south) {
        newModule = MapModule.NS;
      }
      else if (east && west) {
        newModule = MapModule.EW;
      }
    }
    map[r, c] = newModule;
  }

  private void DisplayMap () {
    string output = "";
    for (int r = 0; r < rows; r++) {
      for (int c = 0; c < cols; c++) {
        if (r == spawnRow && c == spawnCol) {
          output += "\u2588";
        }
        else {
          switch(map[r,c]) {
            case MapModule.Empty:
              output += "O";
              break;
            case MapModule.NS:
              output += "\u2502";
              break;
            case MapModule.EW:
              output += "\u2500";
              break;
            case MapModule.NE:
              output += "\u2514";
              break;
            case MapModule.ES:
              output += "\u250c";
              break;
            case MapModule.SW:
              output += "\u2510";
              break;
            case MapModule.WN:
              output += "\u2518";
              break;
            case MapModule.NES:
              output += "\u251c";
              break;
            case MapModule.ESW:
              output += "\u252c";
              break;
            case MapModule.SWN:
              output += "\u2524";
              break;
            case MapModule.WNE:
              output += "\u2534";
              break;
            case MapModule.NESW:
              output += "\u253c";
              break;
            default: // border
              output += "X";
              break;
          }
        }
      }
      output += "\n";
    }
    output += "Spawn: row = " + spawnRow + ", col = " + spawnCol;
    Debug.Log(output);
  }

  private void AssembleMap() {
    for (int r = 1; r < rows - 1; r++) {
      for (int c = 1; c < cols - 1; c++) {
        GameObject prefab = spawnRoomPrefab;
        if (r != spawnRow || c != spawnCol) {
          int x = 1;
          switch(map[r,c]) {
            case MapModule.NS:
              x = Random.Range(0, NSPrefabs.Length);
              prefab = NSPrefabs[x];
              break;
            case MapModule.EW:
              x = Random.Range(0, EWPrefabs.Length);
              prefab = EWPrefabs[x];
              break;
            case MapModule.NE:
              x = Random.Range(0, NEPrefabs.Length);
              prefab = NEPrefabs[x];
              break;
            case MapModule.ES:
              x = Random.Range(0, ESPrefabs.Length);
              prefab = ESPrefabs[x];
              break;
            case MapModule.SW:
              x = Random.Range(0, SWPrefabs.Length);
              prefab = SWPrefabs[x];
              break;
            case MapModule.WN:
              x = Random.Range(0, WNPrefabs.Length);
              prefab = WNPrefabs[x];
              break;
            case MapModule.NES:
              x = Random.Range(0, NESPrefabs.Length);
              prefab = NESPrefabs[x];
              break;
            case MapModule.ESW:
              x = Random.Range(0, ESWPrefabs.Length);
              prefab = ESWPrefabs[x];
              break;
            case MapModule.SWN:
              x = Random.Range(0, SWNPrefabs.Length);
              prefab = SWNPrefabs[x];
              break;
            case MapModule.WNE:
              x = Random.Range(0, WNEPrefabs.Length);
              prefab = WNEPrefabs[x];
              break;
            case MapModule.NESW:
              x = Random.Range(0, NESWPrefabs.Length);
              prefab = NESWPrefabs[x];
              break;
            default:
              continue;
          }
        }
        Instantiate(prefab, new Vector3(r * moduleSize, 0, c * moduleSize), prefab.transform.rotation);
      }
    }
  }

  private void SpawnPlayer() {
    player.transform.position = new Vector3(spawnRow * moduleSize, 5, spawnCol * moduleSize);
  }
}
