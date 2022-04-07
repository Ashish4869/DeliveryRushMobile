using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Transition : MonoBehaviour
{
    /// <summary>
    /// causes a fade to black transition
    /// </summary>

    [SerializeField]
    Animator CrossFade;

    float LoadTime = 0.85f;

    public void LoadLevel(int BuildIndex)
    {
        StartCoroutine(LoadAnimaion(BuildIndex));
    }

    IEnumerator LoadAnimaion(int BuildIndex) //Animate from one scene to another
    {
        CrossFade.SetTrigger("Fade");

        yield return new WaitForSeconds(LoadTime);

        SceneManager.LoadScene(BuildIndex);
    }


}
