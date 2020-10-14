using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    [SerializeField] bool reverse = false;      //Qapi sagdan sola ve soldan saga acilirsa bu parametr vasitesile arasinda kecid edeceyik
    
    bool interacting = false;//Eger oyuncu qapini acirsa acilmasini gozdesin eger bagliyirsa baglanmasini gozlesin deye bu deyiseni teyin etdik.
                             //Belelikle animasiya kecidleri arasinda atlama olmaz.
    bool isOpen = false;
    public void Interact()
    {
        if (!interacting)
        {
            Debug.Log(transform.localRotation.x);
            Debug.Log(transform.rotation.x);
            StartCoroutine(OpenCloseMechanism());
        }
    }




    IEnumerator OpenCloseMechanism()
    {
        interacting = true;
        if (!reverse)   //Ve eks terefli deyilse(metbex skafinin qapisi meselen)
        {
            if (!isOpen)
            {
                
                for (int i = 0; i < 90; i++)
                {
                    transform.localRotation = Quaternion.Euler(transform.rotation.x, i, transform.rotation.y);
                    yield return new WaitForSeconds(0.01f);
                }
                isOpen = true;
                Debug.Log("Door is opened");

            }
            else
            {
                isOpen = false;
                for (int i = 90; i > 0; i--)
                {
                    transform.localRotation = Quaternion.Euler(transform.rotation.x, i, transform.rotation.y);
                    yield return new WaitForSeconds(0.01f);
                }
            }
        }

        else//Eger qapi ters terefe acilirsa
        {
            if (!isOpen)
            {
                
                for (int i = 0; i > -90; i--)
                {
                    transform.localRotation = Quaternion.Euler(transform.localRotation.x, i, transform.localRotation.y);
                    yield return new WaitForSeconds(0.01f);
                }
                isOpen = true;
                Debug.Log("Door is opened");

            }
            else
            { 
                for (int i = -90; i < 0; i++)
                {
                    transform.localRotation = Quaternion.Euler(transform.localRotation.x, i, transform.localRotation.y);
                    yield return new WaitForSeconds(0.01f);
                }
                isOpen = false;

            }
        }
        interacting = false;
    }
}
