using Mirror;
using UnityEngine;

public class Ball_Controller : NetworkBehaviour
{
    [SyncVar][HideInInspector]
    public bool BallIsAttached = false;
}
