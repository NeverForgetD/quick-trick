using Febucci.UI;
using UnityEngine;

public class TestScript1 : MonoBehaviour
{

    string testString = "This is a Test a <wave>very nice</wave> Test!";
    public TypewriterByCharacter tw;

    private void Start()
    {
        tw.ShowText(testString);
    }
}
