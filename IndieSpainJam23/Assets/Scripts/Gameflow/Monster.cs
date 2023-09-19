using UnityEngine;

public class Monster : MonoBehaviour
{
    private HauntedRoom m_room;

    public void SetHauntedRoom(HauntedRoom room)
    {
        m_room = room;
    }

    public void LeaveRoom()
    {
        m_room.StopHaunted();
    }
}
