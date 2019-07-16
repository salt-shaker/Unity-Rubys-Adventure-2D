using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CloseTutorialDialog : MonoBehaviour
{
    public GameObject dialogBox;
    
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(CloseDialogBox);
    }

    void CloseDialogBox()
    {
        Destroy(dialogBox);
    }
}
