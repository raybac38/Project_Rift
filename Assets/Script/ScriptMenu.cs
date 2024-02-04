using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System;
using TMPro;
using Unity.VisualScripting;

public class ScriptMenu : MonoBehaviour
{

    public Relay relay;
    private readonly float transitionDuration = 1.5f;
    public GameObject menuPrincipale;
    public GameObject menuConnexion;
    public GameObject menuParty;


    private CanvasGroup menuPrincipaleCG;
    private CanvasGroup menuConnexionCG;
    private CanvasGroup menuPartyCG;

    private Menu actualMenu = Menu.principale;

    private bool isTransition = false;


    public CanvasGroup warningCanvasGroup;

    public TextMeshProUGUI inputField;
    public TextMeshProUGUI joinCode;

    private void Awake()
    {
        menuPrincipaleCG = menuPrincipale.GetComponent<CanvasGroup>();
        menuConnexionCG = menuConnexion.GetComponent<CanvasGroup>();
        menuPartyCG = menuParty.GetComponent<CanvasGroup>();

        menuPrincipaleCG.alpha = 1;
        menuConnexionCG.alpha = 0;
        menuPartyCG.alpha = 0;
        warningCanvasGroup.alpha = 0;
    }


    public void Play_button()
    {
        MenuTransition(Menu.connexion);
    }
    public void Option_button()
    {

    }
    public void Quit_button()
    {
        Application.Quit();
    }

    public async void Host_button()
    {
        String code = await relay.StartHostWithRelay(4);
        joinCode.text = code;
        MenuTransition(Menu.party);

    }

    public async void Client_button()
    {
        String code = inputField.text;
        code = code.Substring(0, 6);
        bool isConnected = await relay.StartClientWithRelay(code);
        if(!isConnected)
        {
            ///Connextion echouer
            WarningTexteAnimation();
        }
        else
        {
            joinCode.text = code;
            MenuTransition(Menu.party);
        }
    }

    public void Return_button()
    {
        switch (actualMenu)
        {
            case Menu.party:
                MenuTransition(Menu.connexion);
                joinCode.text = "";
                break;
            case Menu.connexion:
                MenuTransition(Menu.principale);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(actualMenu), actualMenu, "Correspondance invalide");
        }
    }
    public void Kickoff_button()
    {
        Debug.Log("Lancement de la parti ! ");
    }
    public enum Menu
    {
        principale,
        connexion,
        party
    }

    /// <summary>
    /// Make a transition betwen two Menu, actual to newMenu
    /// </summary>
    /// <param name="newMenu">The replacement menu</param>
    private void MenuTransition(Menu newMenu)
    {
        if(newMenu == actualMenu || isTransition ) return;

        
        isTransition = true;


        CanvasGroup canvasGroup1 = GetCanvasGroupFromMenu(actualMenu);
        CanvasGroup canvasGroup2 = GetCanvasGroupFromMenu(newMenu);

        StartCoroutine(TransitionAnimation(canvasGroup1, canvasGroup2));

        actualMenu = newMenu;

    }

    /// <summary>
    /// Transition animation
    /// </summary>
    /// <param name="canvasGroup1">Depart</param>
    /// <param name="canvasGroup2">Arriver</param>
    /// <returns>Rien du tout</returns>
    IEnumerator TransitionAnimation(CanvasGroup canvasGroup1, CanvasGroup canvasGroup2)
    {
        float timer = 0f;

        while (timer < transitionDuration)
        {
            float t = timer / transitionDuration;

            canvasGroup1.alpha = Mathf.Lerp(1f, 0f, t);
            canvasGroup2.alpha = Mathf.Lerp(0f, 1f, t);

            timer += Time.deltaTime;

            yield return null; 
        }

        canvasGroup1.alpha = 0;
        canvasGroup2.alpha = 1;

        isTransition = false;

    }
    CanvasGroup GetCanvasGroupFromMenu(Menu menu)
    {
        switch (menu)
        {
            case Menu.principale:
                return menuPrincipaleCG;
            case Menu.connexion:
                return menuConnexionCG;
            case Menu.party:
                return menuPartyCG;
            default:
                throw new ArgumentOutOfRangeException(nameof(menu), menu, "Correspondance invalide");
        }
    }
    


    private bool isWarningShow = false;
    private void WarningTexteAnimation()
    {
        if(isWarningShow) return;
        isWarningShow = true;
        StartCoroutine(WarningAnimation());
    }

    IEnumerator WarningAnimation()
    {
        float timer = 0f;
        while(timer < transitionDuration)
        {
            warningCanvasGroup.alpha = Mathf.Lerp(0, 1, timer / transitionDuration);
            timer += Time.deltaTime;
            yield return null;
        }
        warningCanvasGroup.alpha = 1;
        yield return new WaitForSecondsRealtime(transitionDuration);
        
        timer = 0;
        while(timer < transitionDuration)
        {
            warningCanvasGroup.alpha = Mathf.Lerp(1, 0, timer / transitionDuration);
            timer += Time.deltaTime;
            yield return null;
        }
        warningCanvasGroup.alpha = 0;

        isWarningShow = false;
    }
    
}
