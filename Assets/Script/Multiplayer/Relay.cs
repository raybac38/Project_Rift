using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;

public class Relay : MonoBehaviour
{
    String joincode;
    private async void Start()
    {
        await UnityServices.InitializeAsync();
        AuthenticationService.Instance.SignedIn += () =>
        {
            Debug.Log("Signed in" +  AuthenticationService.Instance.PlayerId);
        };

        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }

    public async void CreateRelay()
    {
        try{
            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(3);
            joincode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
        }catch(RelayServiceException e)
        {
            Debug.Log(e);
        }
        
    }

    public async void JoinRelay(String joincode)
    {
        try{
            await RelayService.Instance.JoinAllocationAsync(joincode);
        }catch(RelayServiceException e)
        {
            Debug.Log(e);
        }
    }
}
