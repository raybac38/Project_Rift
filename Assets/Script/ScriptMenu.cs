using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System;

public class ScriptMenu : NetworkBehaviour
{

    private readonly float transitionDuration = 1.5f;
    public GameObject menuPrincipale;
    public GameObject menuConnexion;
    public GameObject menuParty;


    private CanvasGroup menuPrincipaleCG;
    private CanvasGroup menuConnexionCG;
    private CanvasGroup menuPartyCG;

    private Menu actualMenu = Menu.principale;

    private bool isTransition = false;
    private void Awake()
    {
        menuPrincipaleCG = menuPrincipale.GetComponent<CanvasGroup>();
        menuConnexionCG = menuConnexion.GetComponent<CanvasGroup>();
        menuPartyCG = menuParty.GetComponent<CanvasGroup>();

        menuPrincipaleCG.alpha = 1;
        menuConnexionCG.alpha = 0;
        menuPartyCG.alpha = 0;
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

    public void Host_button()
    {

    }

    public void Client_button()
    {

    }

    public void Return_button()
    {

    }
    public void Kickoff_button()
    {

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
    
    
}
