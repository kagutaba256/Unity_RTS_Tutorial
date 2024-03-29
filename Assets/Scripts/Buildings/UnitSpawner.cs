﻿using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.EventSystems;

public class UnitSpawner : NetworkBehaviour, IPointerClickHandler {
    [SerializeField] private Health health = null;
    [SerializeField] private GameObject unitPrefab = null;
    [SerializeField] private Transform unitSpawnPoint = null;

    #region server

    public override void OnStartServer () {
        health.ServerOnDie += ServerHandleDie;
    }

    public override void OnStopServer () {
        health.ServerOnDie -= ServerHandleDie;
    }

    [Server]
    private void ServerHandleDie () {
        // TODO
        //NetworkServer.Destroy (gameObject); 
    }

    [Command]
    private void CmdSpawnUnit () {
        GameObject unitInstance = Instantiate (
            unitPrefab,
            unitSpawnPoint.position,
            unitSpawnPoint.rotation);
        NetworkServer.Spawn (unitInstance, connectionToClient);
    }

    #endregion

    #region client 

    [ClientCallback]
    public void OnPointerClick (PointerEventData eventData) {
        if (eventData.button != PointerEventData.InputButton.Left) { return; }
        if (!hasAuthority) { return; }
        CmdSpawnUnit ();
    }

    #endregion
}