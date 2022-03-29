using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using TMPro;

public class DialogueLoader : MonoBehaviour
{
    [Header("Starting Dialogue")]
    [SerializeField] private DialogueSO startingDialogue;

    [Header("Text Objects")]
    [SerializeField] private TextMeshProUGUI textUI;
    [SerializeField] private TextMeshProUGUI nameUI;

    [Header("UI Objects")]
    [SerializeField] GameObject SingleChoiceButtonGroup;
    [SerializeField] GameObject MultipleChoiceButtonGroup;
    [SerializeField] GameObject[] MultipleChoiceButtons;

    private DialogueSO currentDialogue;

    private Queue<string> sentences;
    string sentence;

    [Header("Dialogue Manager properties")]
    public Animator animator;
    public bool dialogueInProg;

    public static DialogueLoader instance;

    [Header("Audio")]
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
        SetAudioSourceClip(currentDialogue.NameText);
        ShowNextSentence();


    }

    public void ShowNextSentence()
    {
        StartCoroutine(TypeSentence());
    }

    public void SelectOption(int choiceIndex)
    {
        LoadUnloadChoiceButtons(false);
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
            PitchShiftDialogue();
            textUI.text += letter;
            _as.Play();
            yield return new WaitForSeconds(.1f);
            //_as.Stop();
        }

        LoadUnloadChoiceButtons(true);
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
            if (clip.name.ToString().Contains(speakerName))
            {
                _as.clip = clip;
            }
        }
    }


    void LoadUnloadChoiceButtons(bool isLoading)
    {
        if (isLoading)
        {
            if (currentDialogue.dialogueType == DialogueType.SingleChoice)
            {
                SingleChoiceButtonGroup.SetActive(true);
            }
            else
            {

                MultipleChoiceButtonGroup.SetActive(true);

                for (int i = 0; i < currentDialogue.Choices.Count; i++)
                {
                    MultipleChoiceButtons[i].SetActive(true);
                    MultipleChoiceButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = currentDialogue.Choices[i].Text;
                }
            }
        }
        else
        {
            if(currentDialogue.dialogueType == DialogueType.MultipleChoice)
            {
                for(int i = 0; i < currentDialogue.Choices.Count; i++)
                {
                    MultipleChoiceButtons[i].SetActive(false);
                }

                MultipleChoiceButtonGroup.SetActive(false);
            }
            else{
                SingleChoiceButtonGroup.SetActive(false);
            }
        }
    }

    void PitchShiftDialogue()
    {
        _as.pitch = Random.Range(.8f, 1.1f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}