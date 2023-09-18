using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GameContainer : Architecture<GameContainer>
{
    protected override void Init()
    {
        Register(new DataManager());        
    }
}
