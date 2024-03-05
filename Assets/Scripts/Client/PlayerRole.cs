using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRole : NetworkBehaviour
{
    [SyncVar] public int Role;
}
