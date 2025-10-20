using NUnit.Framework;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public string[] nextRoom;
    public int currentRoomIndex;

    void Start()
    {
        if(nextRoom == null)
        {
            Debug.LogWarning("There's no element in the nextRoom array!");
        }
    }

    public void UpdateRoom()
    {
        currentRoomIndex++;
    }
}
