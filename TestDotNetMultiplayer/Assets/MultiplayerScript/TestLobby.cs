using System.Collections;
using System.Collections.Generic;
using Unity.Services.Authentication;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
public class TestLobby : MonoBehaviour
{

   private Lobby hostLobby;
   private float heartbeatTimer;

   private void Update()
   {
      HandleLobbyHeartbeat();
   }
   private async  void HandleLobbyHeartbeat()
   {
      if(hostLobby != null)
      heartbeatTimer -= Time.deltaTime;
      if (heartbeatTimer < 0f)
      {
         float heartbeatTimerMax = 15;
         heartbeatTimer = heartbeatTimerMax;
          await  LobbyService.Instance.SendHeartbeatPingAsync(hostLobby.Id);
      }
   
   }
    private async void Start()
    {
       await UnityServices.InitializeAsync();
       AuthenticationService.Instance.SignedIn +=   () =>
       {
        Debug.Log("Signed in"+ AuthenticationService.Instance.PlayerId);
        

       }    ;
await AuthenticationService.Instance.SignInAnonymouslyAsync();
    
    }
       public async void CreateLobby()
       {
         try
         {
            string lobbyName = "MyLobby";
            int maxPlayers = 1;
            Lobby lobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName,maxPlayers);
            Debug.Log("Created Lobby! "+ lobby.Name+""+lobby.MaxPlayers);
        
         }


         
         catch (LobbyServiceException e)
         {
            Debug.Log(e);
         }

       }
    
    public async void ListLobbies()
   { 
      try
      {
         QueryLobbiesOptions queryLobbiesOptions = new QueryLobbiesOptions
         {
            Count = 25,
            Filters = new List<QueryFilter> {
               new QueryFilter(QueryFilter.FieldOptions.AvailableSlots,"0",QueryFilter.OpOptions.GT)

            },
            Order = new List<QueryOrder>{new QueryOrder(false,QueryOrder.FieldOptions.Created)
            }
         };


      QueryResponse queryResponse = await Lobbies.Instance.QueryLobbiesAsync();
      Debug.Log("Lobbies found"+queryResponse.Results.Count);
      foreach ( Lobby lobby in queryResponse.Results)

      {
         Debug.Log(lobby.Name + ""+ lobby.MaxPlayers);
      }
   } 
   catch (LobbyServiceException e) 
   {Debug.Log(e);}
    }
  
}
