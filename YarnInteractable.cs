using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;
using UnityEngine.SceneManagement;

public class YarnInteractable : MonoBehaviour {
    //Yarn variables
    [SerializeField] private string conversationStartNode;

    private DialogueRunner dialogueRunner;
    private bool interactable = true;
    private bool isCurrentConversation = false;
    private int gamePhase = 0;   
    public bool present = false;

    //My variables
    public GameObject theLetterE;
    public GameObject bullies;
    public GameObject juniper;
    public DecisionSO SecondLoop;
    public DecisionSO ThirdLoop;
    public DecisionSO BullyQ1;
    public DecisionSO FriendQ1;
    public DecisionSO BullyQ2;
    public DecisionSO FriendQ2;
    public DecisionSO party;
    public DecisionSO goodEnd;
    public DecisionSO badEnd;
    public GameObject completedLoop;
    public GameObject goodEnding;
    public GameObject badEnding;
    public GameObject nextDay;

    public void Start() {
        //Default Yarn stuff
        dialogueRunner = FindObjectOfType<Yarn.Unity.DialogueRunner>();
        dialogueRunner.onDialogueComplete.AddListener(EndConversation);
        //FindObjectOfType<SceneNext>();
        FindObjectsOfType<SceneNext>();
    }

    public void Update()
    {
        //Allows players to interact with NPCs
        if (Input.GetKeyDown("e") && present == true)
        {
            Debug.Log("ur mom");
            if (interactable && !dialogueRunner.IsDialogueRunning)
            {
                StartConversation();
            }
        }
    }

    //Default Yarn stuff
    private void StartConversation() {
        Debug.Log($"Started conversation with {name}.");
        isCurrentConversation = true;
        dialogueRunner.StartDialogue(conversationStartNode);
    }

    //Default Yarn stuff
    private void EndConversation() {
        if (isCurrentConversation) {
            isCurrentConversation = false;
            Debug.Log($"Started conversation with {name}.");
        }
    }

    //[YarnCommand]s are commands that I can trigger from within the Yarn Dialogue script.
    //Upon being triggered, the funciton below it will activate.
    //This function disables the player's ability to start a converstation with an NPC after 
    //having already completed said conversation
    [YarnCommand("disable")]
    public void DisableConversation() {
        interactable = false;
    }
    
    //Triggers the fade to black that happens upon starting a new day
    [YarnCommand("nextday")]
    public void NextDay()
    {
        nextDay.SetActive(true);
    }

    //Spawns in bullies during final quest converstation
    [YarnCommand("SpawnIn")]
    public void FadeInBullies()
    {
        bullies.SetActive(true);
    }

    //Despawns juniper during final quest converstation
    [YarnCommand("Despawn")]
    public void DespawnJuniper()
    {
        juniper.GetComponent<SpriteRenderer>().enabled = false;
        theLetterE.SetActive(false);
    }

    //Rest of below handle the decision booleans used for scene transitions
    //Indicates that the player is currently on day 2
    [YarnCommand("SecondL")]
    public void Second()
    {
        SecondLoop.decision = true;
        completedLoop.GetComponent<SceneNext>().LoadScene();
    }

    //Indicates that the player is currently on day 3
    [YarnCommand("ThirdL")]
    public void Third()
    {
        ThirdLoop.decision = true;
        SecondLoop.decision = false;
        party.decision = false;
        completedLoop.GetComponent<SceneNext>().LoadScene();
    }

    //Player has accepted friend quest 1
    [YarnCommand("Friend1")]
    public void Friend1()
    {
        Debug.Log("urmom");
        FriendQ1.decision = true;
    }
    
    //Player has accepted bully quest 1
    [YarnCommand("Bully1")]
    public void Bully1()
    {
        BullyQ1.decision = true;
    }

    //Player has accepted bully quest 2
    [YarnCommand("Bully2")]
    public void Bully1Complete()
    {
        BullyQ2.decision = true;
    }

    //Player has accepted friend quest 1
    [YarnCommand("Friend2")]
    public void Friend1Complete()
    {
        FriendQ2.decision = true;
    }

    //A part of bully quest 2. Lets scene transtion system know to load in the alternate
    //downstairs scene after leaving bedroom. Also tells player to spawn into bedroom after lying
    //to Peepaw for bullies
    [YarnCommand("party")]
    public void Party()
    {
        party.decision = true;
        completedLoop.GetComponent<SceneNext>().LoadScene();
    }

    //Loads the scene that contains the good end card
    [YarnCommand("good")]
    public void GoodEnd()
    {
        goodEnding.GetComponent<SceneNext>().LoadScene();
    }

    //Loads the scene that contains the bad end card
    [YarnCommand("bad")]
    public void BadEnd()
    {
        badEnding.GetComponent<SceneNext>().LoadScene();
    }

    //These OnTriggers control when the player is able to start a converstion
    //with a NPC
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            present = true;
            theLetterE.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            present = false;
            theLetterE.SetActive(false);
        }
    }
}