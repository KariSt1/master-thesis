using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TwoWeightUICounter : MonoBehaviour
{

    // TextMeshPro - Text (UI)
    public TextMeshProUGUI counterText;

    // public Text counterText;

    int count = 1;

    // Start is called before the first frame update
    void Start()
    {
        counterText.text = count.ToString() + "/30";   
    }

    // Add one to the counter and update the text
    public void AddOne()
    {
        count++;
        counterText.text = count.ToString() + "/30";
    }
}
