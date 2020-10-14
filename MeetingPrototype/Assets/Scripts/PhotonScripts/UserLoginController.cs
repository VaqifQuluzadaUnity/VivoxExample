using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

namespace Com.Vriendly.MeetingPrototype
{
    public class UserLoginController : MonoBehaviourPunCallbacks
    {
        #region Private Serialized Variables

        [SerializeField] GameObject LoginPanel;

        [SerializeField] GameObject WaitingPanel;

        [SerializeField] TMP_InputField UserNameInputField;

        [SerializeField] Button LoginButton;
        #endregion

        #region Private Variables

        /// <summary>
        /// This is the version number of game. The players with same version number can connect to same room(It is used for old and new versions of game)
        /// </summary>
        string GameVersion = "0.01";

        #endregion

        #region MonoBehaviour Callbacks


        private void Start()
        {
            PhotonNetwork.ConnectUsingSettings();
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// This method is used to connect to Photon Online Servers(or if connected connect to random room)
        /// We declared it public couze we can set it as an event in button
        /// </summary>

        #endregion

        #region MonoBehaviourPun Callbacks

        public override void OnConnected()
        {
            WaitingPanel.SetActive(false);      //we set the waiting panel inactive
            LoginPanel.SetActive(true);         //and Login panel active
            UserNameInputField.onValueChanged.AddListener(onUserNameInputFieldChanged);     //we set the event for UserNameInputField.
            LoginButton.onClick.AddListener(onLoginButtonClicked);                          //and event for login button
        }

        public override void OnConnectedToMaster()
        {
            //Using this we can synchronize PhotonNetwork.LoadLevel() with all connected users and master.
            //Only the master can call PhotonNetwork.LoadLevel() and using code below we can synch this loaded scene with other players.
            PhotonNetwork.AutomaticallySyncScene = true;
        }

        
        public override void OnDisconnected(DisconnectCause cause)
        {
            UserNameInputField.onValueChanged.RemoveListener(onUserNameInputFieldChanged);     //we set the event for UserNameInputField.
            LoginButton.onClick.RemoveListener(onLoginButtonClicked);                          //and event for login button

        }
        #endregion

        #region Private Methods

        private void onUserNameInputFieldChanged(string UserInput)          //Using this event we can control input field inputs and login button
        {
            LoginButton.interactable = !string.IsNullOrEmpty(UserNameInputField.text);      //If input field is empty we set login button non-interactable.
        }

        private void onLoginButtonClicked()
        {
            PhotonNetwork.LocalPlayer.NickName = UserNameInputField.text;

            TypedLobby defaultLobby = new TypedLobby("General", LobbyType.Default);

            PhotonNetwork.JoinLobby(defaultLobby);
        }

        #endregion
    }
}