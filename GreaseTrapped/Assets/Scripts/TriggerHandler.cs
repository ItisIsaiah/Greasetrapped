using UnityEngine;

public class TriggerHandler : MonoBehaviour
{
    public MinigameManager mainScript; // Reference to the main script


    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Ready");
    }
    private void OnTriggerStay(Collider other)
    {
        if (mainScript != null)
        {
            mainScript.HandleTrigger(other);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (mainScript != null)
        {
            mainScript.HandleLeaving(other);
        }
    }
}
