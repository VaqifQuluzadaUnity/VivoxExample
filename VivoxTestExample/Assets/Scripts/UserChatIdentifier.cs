using System.ComponentModel;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VivoxUnity;

/// <summary>
/// Using this class we can set info of user visually to the UI.
/// </summary>
/// 
public class UserChatIdentifier : MonoBehaviour
{
    #region Private variables
    VivoxVoiceManager _voiceManagerInstance;        //Singleton reference of voice manager
    #endregion

    #region Serialized Private Variables

    [SerializeField] TextMeshProUGUI PlayerNameText;           //The Text component where players name goes

    [SerializeField] Image ChatStateImage;          //The Image where sprite states(Mute,unmute,speaking) will be shown

    [SerializeField] Sprite MutedImage, SpeakImage, StopSpeakImage;     //Sprites for Chat States

    #endregion

    #region Public variables

    public IParticipant Participant;    //Current Participant that is related with that chat identifier

    #endregion

    #region Getter_Setter Variables

    private bool isMuted;

    public bool IsMuted     //For controlling mute state of local and other users
    {
        get { return isMuted; }    //For security purpose we change values of isMuted with this GetterSetter method. 

        private set
        {
            if (Participant.IsSelf) //if this is our local user
            {
                _voiceManagerInstance.AudioInputDevices.Muted = value;      //we set the mute value of our local user
            }
            else      //if connected player
            {
                if (Participant.InAudio)        //if user connected to voice
                {
                    Participant.LocalMute = value;  //we set the value for connecte player
                }
            }
            isMuted = value;                        //We set the value to isMuted that we wil return
            UpdateChatStateImage();                 //And change the chat state image
        }
    }

    private bool isSpeaking;

    public bool IsSpeaking      //Using this variable we will detect speaking
    {
        get { return isSpeaking; }

        private set
        {
            if(ChatStateImage && !IsMuted)      //if chatStateImage isnt null and user isnt IsMuted
            {
                isSpeaking = value;             //We set the value to isSpeaking
                UpdateChatStateImage();        //And change the chat state image
            }
        }
    }

    #endregion


    #region Public Methods

    /// <summary>
    /// Using this method we will set the info about user to the UI user chat identifier
    /// </summary>
    /// <param name="participant"></param>
    public void SetupUserChatIdentifier(IParticipant participant)
    {
        _voiceManagerInstance = VivoxVoiceManager.Instance;     //we set the reference of VoiceManager

        Participant = participant;                              //Set the reference of our Participant

        PlayerNameText.text = Participant.Account.DisplayName;  //Set the name of user to the UI User chat identifier

        IsMuted = Participant.IsSelf ? _voiceManagerInstance.AudioInputDevices.Muted : Participant.LocalMute;   //Detect if user is muted or not

        isSpeaking = Participant.SpeechDetected;                                            //Detect if user is speaking or not
        GetComponentInChildren<Button>().onClick.AddListener(MuteButtonFunction);           //Add the Mute event to the mute button
        Participant.PropertyChanged += onPropertyChanged;                                   //Add the event to our current user used for speak detection for local and other users
    }

    #endregion

    #region Private Methods

    private void UpdateChatStateImage()
    {
        if (IsMuted)//If player is Muted
        {
            ChatStateImage.sprite = MutedImage;     //we set the chat state image to muted
        }
        else//is not muted
        {
            if (IsSpeaking)//and if speaking
            {
                ChatStateImage.sprite = SpeakImage;  //we set the chat state image to speak
            }
            else//if not speaking
            {
                ChatStateImage.sprite = StopSpeakImage; //we set the chat state image to non-speak.
            }
        }
    }

    private void MuteButtonFunction()       //for mute and unMute user
    {
        IsMuted = !IsMuted;
    }

    private void onPropertyChanged(object obj ,PropertyChangedEventArgs args)           //This event is triggered for our local user and all users in connected users
    {
        switch (args.PropertyName)
        {
            case "SpeechDetected":      //If our user is speaking the image will be changed in both our UI, and other users' UI
                IsSpeaking = Participant.SpeechDetected;
                break;
        }
    }
    #endregion
}
