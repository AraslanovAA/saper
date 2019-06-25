using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultyplayerManager : Photon.PunBehaviour {

	private void Awake()
    {
        PhotonNetwork.ConnectUsingSettings("v0.2");
        print("попытка подключения");
    }
    public override void OnConnectedToMaster()
    {
        //после подключения к серверу пытаемся подключить к рандомной комнает(пока)
        PhotonNetwork.JoinRandomRoom();
        print("подключено к фотон");
    }
    public override void OnJoinedRoom()
    {
        //в случае успешного подключения к комнате
        PhotonNetwork.Instantiate("Camera", new Vector3(0, 1, 0), Quaternion.identity, 0);
        print("подключен к комнате");
    }
    public override void OnPhotonRandomJoinFailed(object[] codeAndMsg)
    {
        PhotonNetwork.CreateRoom("RoomName", new RoomOptions() { MaxPlayers = 2 }, TypedLobby.Default);
        print("комната создана");
    }
    public override void OnPhotonJoinRoomFailed(object[] codeAndMsg)
    {
        //все комнаты заняты либо комнат не существует
        //создаем сами комнату
        print("комната создана");
       // 
        
    }
}
