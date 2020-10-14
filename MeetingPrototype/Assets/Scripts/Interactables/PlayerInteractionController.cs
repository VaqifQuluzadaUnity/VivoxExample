using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInteractionController : MonoBehaviour
{
    [SerializeField] Camera PlayerCamera;           //Bu camera vasitesile raycast yaradib obyektleri teyin edeceyik.
    [SerializeField] Text InteractableNameText;     //InteractableObjectin adini gostermek ucun istifade etdiyimiz text komponentinin reference-dir.

    private void Update()
    {
        DetectObjects();
        
    }

    void DetectObjects()
    {
        Ray newRay = PlayerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));       //Kameranin merkezinden ray yaradiriq.
        RaycastHit RayOut=new RaycastHit();
        if (Physics.Raycast(newRay,out RayOut,5,LayerMask.GetMask("Interactable")))//Eger interactable objecti gorurukse
        {
            InteractableNameText.text = RayOut.collider.gameObject.name;            //Adini ekranda yazdiririq.
            if (Input.GetMouseButtonDown(0))                                        //Eger sol duymeni bassaq
            {
                RayOut.collider.gameObject.GetComponent<IInteractable>().Interact(); //Hemin obyektle interact edirik.
            }
        }
        else
        {
            InteractableNameText.text = "";                                         //Eger interactable obyekt gormurukse bu zaman text-i bos edirik ki, ekranda yazi qalmasin.
        }
    }
}
