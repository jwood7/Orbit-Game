using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ObjectiveScreen : MonoBehaviour
{
    // public List<string> texts;
    // public int index;
    public TextMeshProUGUI objectiveDisplay;
    public TextMeshProUGUI distanceDisplay;
    public GameObject distanceDisplayObject;
    private GameObject[] objectiveObjects;
    public GameObject objectiveObject;
    public GameObject mothership;
    public bool haveObjective;
    public string[] textQueue1;
    public string[] textQueue2;
    public int preObjectiveStage;
    public int postObjectiveStage;
    public GameObject tabToContinueObject;
    public TextMeshProUGUI tabToContinue;
    bool finishedPrinting;
    public string[] bbQueue;
    public int bbStage;
    public bool readingSecrets;
    public bool readSecrets;
    private Coroutine currentCoroutine;

    // Start is called before the first frame update
    void Awake()
    {
        preObjectiveStage = 0;
        postObjectiveStage = 0;
        currentCoroutine = StartCoroutine(printString(textQueue1[preObjectiveStage].Replace("\\n", "\r\n"), preObjectiveStage+1 < textQueue1.Length));
        haveObjective = false;
        finishedPrinting = false;
        readingSecrets = false;
        readSecrets = false;
    }

    void OnTriggerEnter(Collider col){

        //Issue: No solution for conditional text (i.e. after objective is delivered)
        
        
        //Check for specific tag or component
        if (col.gameObject.GetComponent<TextTrigger>() != null){
            objectiveDisplay.text = col.gameObject.GetComponent<TextTrigger>().text.Replace("\\n", "\r\n");
            Destroy(col.gameObject);
        }
    }


    void OnCollisionEnter(Collision col){
        
        
        //Check for specific tag or component
        if (col.gameObject.GetComponent<TextTrigger>() != null){
            // objectiveDisplay.text = col.gameObject.GetComponent<TextTrigger>().text.Replace("\\n", "\r\n");
        }
        if (col.collider.tag == "Objective Item"){ 
            //Will hit an issue if hit objective while holding something else; worry about that later
            if (!haveObjective){
                if (postObjectiveStage == 0){
                    finishedPrinting = true;
                    StopCoroutine(currentCoroutine);
                    currentCoroutine = StartCoroutine(printString(textQueue2[0].Replace("\\n", "\r\n"), false));
                    //consider resetting bbStage;
                }else{
                    finishedPrinting = true;
                    StopCoroutine(currentCoroutine);
                    currentCoroutine = StartCoroutine(printString(textQueue2[postObjectiveStage].Replace("\\n", "\r\n"), postObjectiveStage+1 < textQueue2.Length));
                }
            }
            haveObjective = true;
            
        }
        
    }
    void OnRelease(){
        if (haveObjective){
            haveObjective = false;
            readingSecrets = false;
            finishedPrinting = true;
            StopCoroutine(currentCoroutine);
            objectiveDisplay.text = textQueue1[preObjectiveStage].Replace("\\n", "\r\n");
            currentCoroutine = StartCoroutine(printString(textQueue1[preObjectiveStage].Replace("\\n", "\r\n"), preObjectiveStage+1 < textQueue1.Length));

        }
    }

    void OnTab(){
        //Increase stage
        if (!finishedPrinting){
            finishedPrinting = true;
        }else{
            if (haveObjective && !readingSecrets && postObjectiveStage+1 < textQueue2.Length){
                postObjectiveStage = postObjectiveStage+1;
                currentCoroutine = StartCoroutine(printString(textQueue2[postObjectiveStage].Replace("\\n", "\r\n"), postObjectiveStage+1 < textQueue2.Length));

            }else if (!haveObjective && preObjectiveStage+1 < textQueue1.Length){
                preObjectiveStage = preObjectiveStage+1;
                currentCoroutine = StartCoroutine(printString(textQueue1[preObjectiveStage].Replace("\\n", "\r\n"), preObjectiveStage+1 < textQueue1.Length));
            }else if (haveObjective && readingSecrets && bbStage+1 < bbQueue.Length){
                bbStage = bbStage+1;
                currentCoroutine = StartCoroutine(printString(bbQueue[bbStage].Replace("\\n", "\r\n"), true));
                if (bbStage+1 >= bbQueue.Length){
                    readingSecrets = false;
                    readSecrets = true;
                }
            }
        }
        
    }

    void OnTilde(){
        //Increase stage\
        if (haveObjective && !readSecrets && !readingSecrets && postObjectiveStage == 0){
            finishedPrinting = true;
            StopCoroutine(currentCoroutine);
            readingSecrets = true;
            currentCoroutine = StartCoroutine(printString(bbQueue[bbStage].Replace("\\n", "\r\n"), true));
        }
        
    }

    void OnSkipBriefing(){
        if (!haveObjective && preObjectiveStage+1 < textQueue1.Length){
            finishedPrinting = true;
            StopCoroutine(currentCoroutine);
            preObjectiveStage = textQueue1.Length-1;
            currentCoroutine = StartCoroutine(printString(textQueue1[textQueue1.Length-1].Replace("\\n", "\r\n"), false));
        }
    }



    // Update is called once per frame
    void Update()
    {
        if (objectiveObject != null){
            distanceDisplayObject.SetActive(true);
            distanceDisplay.text = Vector3.Distance(transform.position, objectiveObject.transform.position).ToString("F1") + "m";
        }else{
            // distanceDisplayObject.SetActive(false);
            distanceDisplay.text = Vector3.Distance(transform.position, mothership.transform.position).ToString("F1") + "m";
            objectiveObjects = GameObject.FindGameObjectsWithTag("Objective Item");
            if (objectiveObjects.Length > 0){
                objectiveObject = objectiveObjects[0];
            }
        }
        
        
    }

    IEnumerator TabFlicker()
    {
        while(tabToContinueObject.activeSelf){
            tabToContinue.text = "Hit [Tab] to Continue";
            yield return new WaitForSeconds(0.55f);
            tabToContinue.text = "";
            yield return new WaitForSeconds(0.35f);
        }
    }

    IEnumerator printString(string s, bool cont){
        finishedPrinting = false;  
        tabToContinueObject.SetActive(false);
        objectiveDisplay.text = "";
        foreach(char c in s){
        // for (int i = 0; i< s.Length; i++){
            if (finishedPrinting == true){
                objectiveDisplay.text = s;
                break;
            }
            objectiveDisplay.text = objectiveDisplay.text + c;
            // objectiveDisplay.text = s.Substring(0, i);
            yield return new WaitForSeconds(0.01f);
        }
        finishedPrinting = true;
        Debug.Log(cont);
        if(cont){
            tabToContinueObject.SetActive(true);
            currentCoroutine = StartCoroutine(TabFlicker());
        }

    }
}
