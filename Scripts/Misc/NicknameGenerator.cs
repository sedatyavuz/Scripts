using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class NicknameGenerator : MonoBehaviour
{
    [SerializeField] private StringRuntimeSet nicknames;

    void Awake()
    {
        TextAsset asset = (TextAsset)Resources.Load("BotNicknames");
        nicknames.AddRange(new List<string>(asset.text.Split('\n')));
    }
}
