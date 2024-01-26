using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event
{
    public enum EventType
    {
        Rain, WorldCup, AI
    }

    public static EventType MakeRandomEvent()
    {
        System.Random r = new System.Random();
        return ((EventType)(r.Next() % 3));
    }
}

//public class RainEvent
//{

//}

//public class WorldCupEvent
//{

//}

//public class 
