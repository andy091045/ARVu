using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirebaseManager : MonoBehaviour
{
    public Firebase.Auth.FirebaseAuth auth;
    public Firebase.Auth.FirebaseUser user;

    // Start is called before the first frame update
    void Start()
    {
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        auth.StateChanged += AuthStateChanged;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Register(string email, string password)
    {
        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith(task => {
            if (task.IsCanceled)
            {
                return;
            }
            if(task.IsFaulted)
            {
                print(task.Exception.InnerException.Message);
                return;
            }
            if(task.IsCompletedSuccessfully)
            {
                print("Registered!");
            }
        });
    }

    void AuthStateChanged(object sender, System.EventArgs eventArgs)
    {
        if(auth.CurrentUser != user)
        {
            user = auth.CurrentUser;
            if(user != null)
            {
                print($"Logib - {user.Email}");
            }
        }
    }

    private void OnDestroy()
    {
        auth.StateChanged -= AuthStateChanged;
    }
}
