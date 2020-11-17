using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.Events;

public class Unit : NetworkBehaviour {
    [SerializeField] private UnityEvent onSelected = null;
    [SerializeField] private UnityEvent onDeselected = null;

    #region server
    #endregion

    #region client
    [Client]
    public void Select () {
        if (!hasAuthority) { return; }
        onSelected?.Invoke ();
    }

    [Client]
    public void Deselect () {
        if (!hasAuthority) { return; }
        onDeselected?.Invoke ();
    }
    #endregion
}