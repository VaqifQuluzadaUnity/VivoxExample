using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using VivoxUnity;

public class UserMenuController : MonoBehaviour
{
    VivoxVoiceManager VoiceMgrInstance;
    [SerializeField] Button LogOutButton;   //Bu button vasitesile chatdan logout ede bilerik.
    [SerializeField] TextMeshProUGUI UserNameText;  //User-in adinin daxil olacagi yeri bildirir.
    [SerializeField] GameObject ErrorMessageOrInfoPanel;          //Xeta bas verdikde ve ya hansisa problemle qarsilasdiqda melumat vermek ucun bu panelden istifade edeceyik.

    private void Start()
    {
        VoiceMgrInstance = VivoxVoiceManager.Instance;
        if (VoiceMgrInstance.LoginState != LoginState.LoggedIn)//Eger login edilmeyibse
        {
            ErrorMessageOrInfoPanel.SetActive(true);     //InfoPaneli aktivlesdirilsin.
            ErrorMessageOrInfoPanel.GetComponentInChildren<TextMeshProUGUI>().text = "Oops something went wrong :(";
            this.enabled = false;
            return;
        }
        ILoginSession currentLoginSession = VivoxVoiceManager.Instance.LoginSession;    //Hal hazirda oldugumuz login sessionu aliriq.Bu sessiondaki melumatlari istifade edeceyik.
        UserNameText.text = currentLoginSession.LoginSessionId.DisplayName; //Hazirki daxil oldugumuz adi username text-e elave edirik. Bu adi biz User Login Controllerde VivoxVoiceManager.Instance.Login(userName); ile teyin etmisik.
    }

    private void OnEnable()     //Eger login successful olubsa bu script disabled edilmir ve
    {
        LogOutButton.onClick.AddListener(LogOutButtonFunction);     //LogOut buttonunu logout-a gore ayarliyiriq.
    }

    private void OnDisable()                                        //Eger login successful olmayibsa bu zaman bu script disabled edilir ve 
    {
        LogOutButton.onClick.RemoveListener(LogOutButtonFunction);  //LogOut funksionalligi deyisdirilir.
        LogOutButton.onClick.AddListener(EmergencyLogOutButtonFunction);
    }


    private void LogOutButtonFunction()//Adi log out seraiti ucun yaradilmis event
    {
       VoiceMgrInstance.Logout();    //vivox managerden bizi logout edir
        SceneManager.LoadScene(0);              //ve esas login sehnesini yukleyir.
    }

    private void EmergencyLogOutButtonFunction()//Eger login olmazsa yalnizca chat menuya qayitmaq lazimdir buna gore de bu metodu teyin etdik ve OnDisable-da istifade etdik.
    {
        SceneManager.LoadScene(0);
    }
}
