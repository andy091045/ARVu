using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RegisterScene : MonoBehaviour
{
    [SerializeField]
    FirebaseManager firebaseManager_;
    [SerializeField]
    InputField inputEmail_;
    [SerializeField]
    InputField inputPassword_;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Register()
    {
        firebaseManager_.Register(inputEmail_.text, inputPassword_.text);
    }
}
