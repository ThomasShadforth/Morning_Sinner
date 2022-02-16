using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{

    public static DialogueManager instance;
    public GameObject testDialogueBox;

    private Queue<string> sentences;
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI sentenceText;
    [SerializeField] Animator animator;
    
    DialogueTrigger dialogueTrig;


    // Start is called before the first frame update
    void Start()
    {
        if(instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        sentences = new Queue<string>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void startDialogue(dialogue dialogue)
    {
        animator.SetBool("isOpen", true);

        nameText.text = dialogue.name;
        sentences.Clear(); //Clears the sentences queue for new dialogue

        foreach(string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if(sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        StartCoroutine(typeSentence(sentence));
    }

    public IEnumerator typeSentence(string sentence)
    {
        sentenceText.text = "";

        foreach(char letter in sentence.ToCharArray())
        {
            sentenceText.text += letter;
            yield return new WaitForSeconds(.05f);
        }
    }

    void EndDialogue()
    {
        animator.SetBool("isOpen", false);


    }
}
