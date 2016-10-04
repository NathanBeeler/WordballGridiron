using UnityEngine;
using System.Collections;

public class SmokePuffScript : MonoBehaviour {

  private void OnReturnToReady(GameObject go)
  {
    Destroy(gameObject);
  }

  void OnEnable()
  {
    EventManager.StartListening("ReturnToReady", OnReturnToReady);
  }

  void OnDisable()
  {
    EventManager.StopListening("ReturnToReady", OnReturnToReady);
  }
}
