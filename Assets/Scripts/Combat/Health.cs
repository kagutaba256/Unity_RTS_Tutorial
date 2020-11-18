using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class Health : NetworkBehaviour {
    [SerializeField] private int maxHealth = 100;
    [SyncVar (hook = nameof (HandleHealthUpdated))]
    private int currentHealth;

    public event Action ServerOnDie;

    public event Action<int, int> ClientOnHealthUpdated;

    #region server

    public override void OnStartServer () {
        currentHealth = maxHealth;
        UnitBase.ServerOnPlayerDie += ServerHandleOnPlayerDie;
    }

    public override void OnStopServer () {
        UnitBase.ServerOnPlayerDie -= ServerHandleOnPlayerDie;
    }

    [Server]
    public void DealDamage (int damageAmount) {
        if (currentHealth == 0) { return; }
        currentHealth = Mathf.Max (currentHealth - damageAmount, 0);
        if (currentHealth != 0) { return; }
        ServerOnDie?.Invoke ();
    }

    [Server]
    private void ServerHandleOnPlayerDie (int connectionId) {
        if (connectionId != connectionToClient.connectionId) { return; }
        DealDamage (currentHealth);
    }

    #endregion

    #region client 

    private void HandleHealthUpdated (int oldHealth, int newHealth) {
        ClientOnHealthUpdated?.Invoke (newHealth, maxHealth);
    }

    #endregion
}