using System;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Events;
using VivoxUnity;

//[Serializable]
public class OnParticipantProperty : UnityEvent<IParticipant, PropertyChangedEventArgs> { }       //This event can be used for future purposes
public class PositionalVoice : MonoBehaviour
{

    #region Public Variables
    //public OnParticipantProperty _participantPropertyEvent = new OnParticipantProperty();

    public GameObject PositionalGameObject;         //This GameObject defines our player or object that we set speakers and listeners on.

    public bool isSpeaking { get; private set; }            //This variable is used for speech detection of player.For security purposes we declare it with private set.

    public Channel3DProperties ChannelProperties { get; private set; }//The general properties of positional channel. decleration purpose is same as isSpeaking.

    #endregion


    #region Private Variables
    private IChannelSession CurrentConnectedChannelSession;     //This is the channel session that we connected.

    //private string PositionalChannelName;           //That is test purpose channel name. We can then use it with different ways.

    #endregion

    #region Private Get-Set Variables
    private IParticipant _participant;                 //For controlling current users participant features such as speech detection and participant callbacks

    public IParticipant Participant
    {
        get
        {
            return _participant;
        }
        set
        {
            if (value != null)                      //When we set the value of participant we automatically set the participant for positional channel.
            {
                _participant = value;
                SetupParticipantForPositionalChannel();
            }
        }
    }
    #endregion

    #region Private Methods
    private void SetupParticipantForPositionalChannel()
    {
        PositionalGameObject = PositionalGameObject != null ? PositionalGameObject : gameObject;        //If we set PositionalGameObject externally when we set this variable it will be externally selected one.
        //  But for common we set PositionalVoice script to our player so we set our current player as our PositionalGameObject.
        CurrentConnectedChannelSession = Participant.ParentChannelSession;
        Participant.PropertyChanged -= participant_PropertyChanged;         //if we intend to change our positional gameObject and participant,for preventing security issues we remove the current PropertyChanged event from Participant.
        Participant.PropertyChanged+= participant_PropertyChanged;          //Then add again.
    }
    #endregion

    #region Vivox Callbacks
    private void participant_PropertyChanged(object sender, PropertyChangedEventArgs args)
    {
        IParticipant participant= (IParticipant)sender;    //sender is generally an object class so we need to convert it to IParticipant.

        switch (args.PropertyName)
        {
            case "SpeechDetected":
                isSpeaking = participant.SpeechDetected;
                break;
        }
    }

    #endregion

    #region Public Methods
    /// <summary>
    /// The working principle of positional channel is that: When we create a positional channel, it didnt move, only our players move and this channel calculate the distance and other properties
    /// to create real world voice communication among them. To let voice channel calculate the position of player we need to set a method that updates the position of player.
    /// </summary>
    /// <param name="listener"></param>
    /// <param name="speaker"></param>

    public void Update3DPosition(Transform listener, Transform speaker)     //Generally speaker and listener are set on the player.
    {
        if (listener == null || speaker == null)        //if any of them missing
        {
            VxClient.Instance.vivoxDebug.DebugMessage("Cannot set 3D position: Either speaker or listener is null", vx_log_level.log_error);
            return;
        }

        if(CurrentConnectedChannelSession!=null && CurrentConnectedChannelSession.AudioState == ConnectionState.Connected)          //If we connected to the channel
        {
            CurrentConnectedChannelSession.Set3DPosition(speaker.position, listener.position, listener.forward, listener.up);

        }
        else//if any errors occurs about channelSession
        {
            VxClient.Instance.vivoxDebug.DebugMessage("Cannot set 3D position: Either speaker or listener is null", vx_log_level.log_info);
        }
    }

    #endregion


    #region MonoBehaviour callbacks
    private void Update()
    {
        if (PositionalGameObject && PositionalGameObject.transform&&CurrentConnectedChannelSession!=null&&CurrentConnectedChannelSession.AudioState==ConnectionState.Connected)//If we have set the positional GameObject. Our ChannelSession isnt null(may be in the connecting state) and audioState is Connected
        {
            Update3DPosition(PositionalGameObject.transform, PositionalGameObject.transform);       //as the speaker and listener are both on our player.
        }
    }
    #endregion
}
