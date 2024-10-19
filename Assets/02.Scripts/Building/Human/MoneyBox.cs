using UnityEngine;

public class MoneyBox : MonoBehaviour
{
    private void Start()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.TryGetComponent(out WorkerHuman worker))
        {

        }
    }
}
