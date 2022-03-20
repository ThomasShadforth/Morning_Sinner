using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using TMPro;

public class DialogueLoader : MonoBehaviour
{
    [SerializeField] private DialogueSO startingDialogue;
    [SerializeField] private TextMeshProUGUI textUI;

    private DialogueSO currentDialogue;

    private Queue<string> sentences;
    string sentence;


    public Animator animator;
    public bool dialogueInProg;

    public static DialogueLoader instance;

    private void Awake()
    {
        //currentDialogue = startingDialogue;
        
    }

    // Start is called before the first frame update
    void Start()
    {
        if(instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        sentences = new Queue<string>();
    }

    public void SetStartingDialogue(DialogueSO dialogueObject)
    {
        currentDialogue = dialogueObject;
    }

    public void StartBranchingDialogue()
    {
        animator.SetBool("isOpen", true);

        dialogueInProg = true;

        sentence = currentDialogue.DialogueText;

        ShowNextSentence();


    }

    public void ShowNextSentence()
    {
        StartCoroutine(TypeSentence());
    }

    public void SelectOption(int choiceIndex)
    {
        DialogueSO nextDialogue = currentDialogue.Choices[choiceIndex].NextDialogue;
        if(nextDialogue == null)
        {
            //Close the dialogue, then return
        }

        currentDialogue = nextDialogue;
        sentence = currentDialogue.DialogueText;
        ShowNextSentence();


    }

    IEnumerator TypeSentence()
    {
        textUI.text = "";

        foreach(char letter in sentence.ToCharArray())
        {
            textUI.text += letter;
            yield return new WaitForSeconds(.05f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
