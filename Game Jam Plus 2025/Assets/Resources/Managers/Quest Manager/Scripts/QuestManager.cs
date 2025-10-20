using NUnit.Framework;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    [SerializeField] string[] nextRoom;

    void Start()
    {
        if(nextRoom == null)
        {
            Debug.LogWarning("There's no element in the nextRoom array!");
        }
    }

    void Update()
    {
        
    }
}
