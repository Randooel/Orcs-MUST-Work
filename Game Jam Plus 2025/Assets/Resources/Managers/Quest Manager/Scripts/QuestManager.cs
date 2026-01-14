using NUnit.Framework;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public string[] nextRoom;
    public int currentRoomIndex;

    [Space(10)]
    public bool canUpdateRoom;

    void Start()
    {
        if(nextRoom == null)
        {
            Debug.LogWarning("There's no element in the nextRoom array!");
        }
    }

    public void UpdateRoom()
    {
        if(canUpdateRoom)
        {
            canUpdateRoom = false;
            currentRoomIndex++;
        }
    }
}
