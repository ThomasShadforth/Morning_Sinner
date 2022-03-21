using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using TMPro;

public class DialogueLoader : MonoBehaviour
{
    [SerializeField] private DialogueSO startingDialogue;
    [SerializeField] private TextMeshProUGUI textUI;
    [SerializeField] private TextMeshProUGUI nameUI;

    private DialogueSO currentDialogue;

    private Queue<string> sentences;
    string sentence;


    public Animator animator;
    public bool dialogueInProg;

    public static DialogueLoader instance;

    [SerializeField] AudioSource _as;
    [SerializeField] List<AudioClip> gruntClips;

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

        _as = GetComponent<AudioSource>();
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
        nameUI.text = currentDialogue.NameText;
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
            EndDialogue();
            return;
        }

        currentDialogue = nextDialogue;
        nameUI.text = currentDialogue.NameText;
        sentence = currentDialogue.DialogueText;

        ShowNextSentence();


    }

    IEnumerator TypeSentence()
    {
        textUI.text = "";

        foreach(char letter in sentence.ToCharArray())
        {
            textUI.text += letter;
            _as.Play();
            yield return new WaitForSeconds(.05f);
            _as.Stop();
        }
    }

    public void EndDialogue()
    {
        animator.SetBool("isOpen", false);
        dialogueInProg = false;
    }

    public void SetAudioSourceClip(string speakerName)
    {
        foreach(AudioClip clip in gruntClips)
        {
            if (clip.name.Contains(speakerName))
            {
                _as.clip = clip;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
