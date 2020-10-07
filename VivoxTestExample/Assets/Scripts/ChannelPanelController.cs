using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using VivoxUnity;

public class ChannelPanelController : MonoBehaviour
{
    [SerializeField] GameObject ChannelButtonParent;        //Channel buttonlari ozunde dasiyan obyektin reference-dir.
    [SerializeField] GameObject ErrorMessageOrInfoPanel;                  //Melumat ve ya xetalar haqqinda info vermek ucun bu paneldeki text objectini istifade edeceyik.
    [SerializeField]List<Button> ChannelButtons = new List<Button>();
    VivoxVoiceManager VoiceMgrInstance;

    private void Awake()
    {
        VoiceMgrInstance = VivoxVoiceManager.Instance;
        ChannelButtons = ChannelButtonParent.GetComponentsInChildren<Button>().ToList();//Burda parent obyektdeki butun buttonlari aliriq ve liste ceviririk.
        if (ChannelButtons.Count == 0)//Eger hec bir channel yoxdusa bu zaman...
        {
            ErrorMessageOrInfoPanel.SetActive(true);
            ErrorMessageOrInfoPanel.GetComponentInChildren<TextMeshProUGUI>().text = "There is no Channel in Chat";//InfoPanelde bunun haqqinda melumat veririk.
        }
        else
        {
            JoinChannel(ChannelButtons[0].name);//Eger varsa ilkin olaraq default channele ve ya General channele qosuluruq.
        }
    }
    private void OnEnable()     //Bu gameObject enable edilen zaman
    {
        foreach(Button btn in ChannelButtons)//Butun channelbuttonlara onChannelButtonClicked eventini elave edirik ki, ustune basanda hemin channele daxil olsun.
        {
            btn.onClick.AddListener(onChannelButtonClicked);
        }
    }

    private void OnDisable()//Burda ise hemin eventlari legv edirik.
    {
        foreach(Button btn in ChannelButtons)
        {
            btn.onClick.RemoveListener(onChannelButtonClicked);
        }
    }
    void JoinChannel(string ChannelName)//Join eden zaman string olaraq channel adi alacagiq.
    {
        VoiceMgrInstance.DisconnectAllChannels();     //Butun diger channellerden cixiriq
        //ErrorMessageOrInfoPanel.SetActive(true);
        //ErrorMessageOrInfoPanel.GetComponentInChildren<TextMeshProUGUI>().text = "Joining to #" + ChannelName;//Info Panelde qosuldugumuz channel haqqinda melumat veririk.
        VoiceMgrInstance.JoinChannel(ChannelName, ChannelType.NonPositional, VivoxVoiceManager.ChatCapability.TextAndAudio);//bu adda channel yoxdusa verdiyimiz xususiyyetlere uygun channel yaradir.(1ci parametr channelin adi, 2ci channelin movable xususiyyeti, 3cu ise text ve audio ucun olmasini temin edir.)
        Debug.Log("Connected to " + ChannelName);
        //ErrorMessageOrInfoPanel.SetActive(false);                             //Info Paneli sondururuk.
    }
    
    private void onChannelButtonClicked()       //Bu ise channel buttona elave edeceyimiz eventdir.
    {
        string channelName = EventSystem.current.currentSelectedGameObject.name;    //Hazirki basilan buttonun adini aliriq.
        JoinChannel(channelName);                                                   //Ve ona uygun channel yaradiriq ve ya yaradilibsa qosuluruq.
    }   
}
