using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class O2Storage : MonoBehaviour
{
    public float amt;
    public float maxAmt;
    public MeshRenderer[] indicatorsT;
    public MeshRenderer[] indicatorsS;
    public MeshRenderer indicatorEmpty;
    public Material fullColor;
    public Material emptyColor;
    private int counter = 0;
    // Start is called before the first frame update
    void Awake()
    {
        if (amt >= maxAmt*(5/6f) ){
                for ( int i = 0; i < 6f; i++)
                {
                    indicatorsT[i].material = fullColor;
                }
                for ( int i = 0; i < 6f; i++)
                {
                    indicatorsS[i].material = fullColor;
                }
                indicatorEmpty.material = fullColor;

            }else if (amt < maxAmt*(5/6f) && amt >= maxAmt*(5/6f)){
                for ( int i = 0; i < 5; i++)
                {
                    indicatorsT[i].material = fullColor;
                }
                for ( int i = 0; i < 5; i++)
                {
                    indicatorsS[i].material = fullColor;
                }
                indicatorEmpty.material = fullColor;
                indicatorsT[5].material = emptyColor;
                indicatorsS[5].material = emptyColor;

            }else if (amt < maxAmt*(4/6f) && amt >= maxAmt*(3/6f)){
                for ( int i = 0; i < 4; i++)
                {
                    indicatorsT[i].material = fullColor;
                }
                for ( int i = 0; i < 4; i++)
                {
                    indicatorsS[i].material = fullColor;
                }
                indicatorEmpty.material = fullColor;
                for ( int i = 4; i < 6f; i++)
                {
                    indicatorsT[i].material = emptyColor;
                }
                for ( int i = 4; i < 6f; i++)
                {
                    indicatorsS[i].material = emptyColor;
                }

            }else if (amt < maxAmt*(3/6f) && amt >= maxAmt*(2/6f)){
                // Debug.Log("UNDER 3000");
                for ( int i = 0; i < 3; i++)
                {
                    indicatorsT[i].material = fullColor;
                }
                for ( int i = 0; i < 3; i++)
                {
                    indicatorsS[i].material = fullColor;
                }
                indicatorEmpty.material = fullColor;
                for ( int i = 3; i < 6f; i++)
                {
                    indicatorsT[i].material = emptyColor;
                }
                for ( int i = 3; i < 6f; i++)
                {
                    indicatorsS[i].material = emptyColor;
                }

            }else if (amt < maxAmt*(2/6f) && amt >= maxAmt*(1/6f)){
                for ( int i = 0; i < 2; i++)
                {
                    indicatorsT[i].material = fullColor;
                }
                for ( int i = 0; i < 2; i++)
                {
                    indicatorsS[i].material = fullColor;
                }
                indicatorEmpty.material = fullColor;
                for ( int i = 2; i < 6f; i++)
                {
                    indicatorsT[i].material = emptyColor;
                }
                for ( int i = 2; i < 6f; i++)
                {
                    indicatorsS[i].material = emptyColor;
                }

            }else if (amt < maxAmt*(1/6f) && amt > 0){
                indicatorsS[0].material = fullColor;
                indicatorsT[0].material = fullColor;
                indicatorEmpty.material = fullColor;
                for ( int i = 1; i < 6f; i++)
                {
                    indicatorsT[i].material = emptyColor;
                }
                for ( int i = 1; i < 6f; i++)
                {
                    indicatorsS[i].material = emptyColor;
                }
            }else if (amt <= 0){
                Debug.Log("YEET, EMPTY");
                for ( int i = 0; i < 6f; i++)
                {
                    indicatorsT[i].material = emptyColor;
                }
                for ( int i = 0; i < 6f; i++)
                {
                    indicatorsS[i].material = emptyColor;
                }
                indicatorEmpty.material = emptyColor;

            }
    }

    // Update is called once per frame
    void Update()
    {
        counter++;
        if (counter == 10){
            Debug.Log(amt);
            Debug.Log(maxAmt);
            if (amt >= maxAmt*(5/6f) ){
                for ( int i = 0; i < 6f; i++)
                {
                    indicatorsT[i].material = fullColor;
                }
                for ( int i = 0; i < 6f; i++)
                {
                    indicatorsS[i].material = fullColor;
                }
                indicatorEmpty.material = fullColor;

            }else if (amt < maxAmt*(5/6f) && amt > maxAmt*(5/6f)){
                for ( int i = 0; i < 5; i++)
                {
                    indicatorsT[i].material = fullColor;
                }
                for ( int i = 0; i < 5; i++)
                {
                    indicatorsS[i].material = fullColor;
                }
                indicatorEmpty.material = fullColor;
                indicatorsT[5].material = emptyColor;
                indicatorsS[5].material = emptyColor;

            }else if (amt < maxAmt*(4/6f) && amt > maxAmt*(3/6f)){
                for ( int i = 0; i < 4; i++)
                {
                    indicatorsT[i].material = fullColor;
                }
                for ( int i = 0; i < 4; i++)
                {
                    indicatorsS[i].material = fullColor;
                }
                indicatorEmpty.material = fullColor;
                for ( int i = 4; i < 6f; i++)
                {
                    indicatorsT[i].material = emptyColor;
                }
                for ( int i = 4; i < 6f; i++)
                {
                    indicatorsS[i].material = emptyColor;
                }

            }else if (amt < maxAmt*(3/6f) && amt > maxAmt*(2/6f)){
                for ( int i = 0; i < 3; i++)
                {
                    indicatorsT[i].material = fullColor;
                }
                for ( int i = 0; i < 3; i++)
                {
                    indicatorsS[i].material = fullColor;
                }
                indicatorEmpty.material = fullColor;
                for ( int i = 3; i < 6f; i++)
                {
                    indicatorsT[i].material = emptyColor;
                }
                for ( int i = 3; i < 6f; i++)
                {
                    indicatorsS[i].material = emptyColor;
                }

            }else if (amt < maxAmt*(2/6f) && amt > maxAmt*(1/6f)){
                for ( int i = 0; i < 2; i++)
                {
                    indicatorsT[i].material = fullColor;
                }
                for ( int i = 0; i < 2; i++)
                {
                    indicatorsS[i].material = fullColor;
                }
                indicatorEmpty.material = fullColor;
                for ( int i = 2; i < 6f; i++)
                {
                    indicatorsT[i].material = emptyColor;
                }
                for ( int i = 2; i < 6f; i++)
                {
                    indicatorsS[i].material = emptyColor;
                }

            }else if (amt < maxAmt*(1/6f) && amt > 0){
                indicatorsS[0].material = fullColor;
                indicatorsS[0].material = fullColor;
                indicatorEmpty.material = fullColor;
                for ( int i = 1; i < 6f; i++)
                {
                    indicatorsT[i].material = emptyColor;
                }
                for ( int i = 1; i < 6f; i++)
                {
                    indicatorsS[i].material = emptyColor;
                }
            }else if (amt <= 0){
                Debug.Log("YEET, EMPTY");
                for ( int i = 0; i < 6f; i++)
                {
                    indicatorsT[i].material = emptyColor;
                }
                for ( int i = 0; i < 6f; i++)
                {
                    indicatorsS[i].material = emptyColor;
                }
                indicatorEmpty.material = emptyColor;

            }
        }
    }
}
