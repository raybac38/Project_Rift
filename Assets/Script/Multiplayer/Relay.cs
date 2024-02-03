using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;

public class Relay : MonoBehaviour
{
    String hostCode = null;
    private async void Start()
    {
        await UnityServices.InitializeAsync();
        AuthenticationService.Instance.SignedIn += () =>
        {
            Debug.Log("Signed in" +  AuthenticationService.Instance.PlayerId);
        };

        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }

    public async void CreateRelay(MultiplayerInterface multiplayerInterface)
    {
        try{
            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(3);
            hostCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);

            NetworkManager.Singleton.GetComponent<UnityTransport>().SetHostRelayData(
                allocation.RelayServer.IpV4,
                (ushort)allocation.RelayServer.Port,
                allocation.AllocationIdBytes,
                allocation.Key,
                allocation.ConnectionData
            );
            NetworkManager.Singleton.StartHost();
        }catch(RelayServiceException e)
        {
            Debug.Log(e);
            hostCode = null;
        }
        multiplayerInterface.WriteHostCode(hostCode);

    }

    public async void JoinRelay(String joincode, MultiplayerInterface multiplayerInterface)
    {
        hostCode = joincode;
        try{

            JoinAllocation joinAllocation = await RelayService.Instance.JoinAllocationAsync(hostCode);
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetClientRelayData(
                joinAllocation.RelayServer.IpV4,
                (ushort)joinAllocation.RelayServer.Port,
                joinAllocation.AllocationIdBytes,
                joinAllocation.Key,
                joinAllocation.ConnectionData,
                joinAllocation.HostConnectionData
            );

            NetworkManager.Singleton.StartClient();
        }catch(RelayServiceException e)
        {
            Debug.Log(e);
            hostCode = null;
        }
        multiplayerInterface.WriteHostCode(hostCode);
    }

    public string GetHostCode()
    {
        return hostCode;
    }
}
