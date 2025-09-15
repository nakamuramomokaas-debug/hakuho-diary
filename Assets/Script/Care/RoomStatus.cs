using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomStatus : MonoBehaviour
{
    public struct MyRoomStatus
    {
        public int vivid;//100エゴー協調0
        public int simple;//100自立ー依存0
        public int autmatic; //100外交的ー内向的0
        public int cool;//100直観ー理性0
        public int active;//100温厚ー好戦的0
    }

    MyRoomStatus status;

    void Start()
    {

    }
}
