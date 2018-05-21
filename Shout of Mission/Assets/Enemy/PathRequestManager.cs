using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PathRequestManager : MonoBehaviour {

  Queue<PathRequest> requestQueue = new Queue<PathRequest>();
  PathRequest currentRequest;

  static PathRequestManager instance;
  Pathfinding pathfinding;
  bool isProcessing;

  void Awake() {
    instance = this;
    pathfinding = GetComponent<Pathfinding>();
  }

	public static void RequestPath(Vector3 pathStart, Vector3 pathEnd, Action<Vector3[], bool> callback) {
    PathRequest newRequest = new PathRequest(pathStart, pathEnd, callback);
    instance.requestQueue.Enqueue(newRequest);
    instance.TryProcessNext();
  }

  void TryProcessNext() {
    if (!isProcessing && requestQueue.Count > 0) {
      currentRequest = requestQueue.Dequeue();
      isProcessing = true;
      pathfinding.StartFindPath(currentRequest.pathStart, currentRequest.pathEnd);
    }
  }

  public void FinishProcessing(Vector3[] path, bool success) {
    currentRequest.callback(path, success);
    isProcessing = false;
    TryProcessNext();
  }

  struct PathRequest {
    public Vector3 pathStart;
    public Vector3 pathEnd;
    public Action<Vector3[], bool> callback;

    public PathRequest(Vector3 _start, Vector3 _end, Action<Vector3[], bool> _callback) {
      pathStart = _start;
      pathEnd = _end;
      callback = _callback;
    }
  }

}
