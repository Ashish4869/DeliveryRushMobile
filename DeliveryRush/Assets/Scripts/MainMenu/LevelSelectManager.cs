using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelSelectManager : MonoBehaviour
{
    /// <summary>
    /// Deals with showing the saved info on the levels
    /// </summary>

    [SerializeField]
    GameObject ButtonGroup;

    [SerializeField]
    Color yellow;

    [SerializeField]
    Color black;

    [SerializeField]
    Sprite star;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.K))
        {
            Scores();
        }

        if(Input.GetKeyDown(KeyCode.L))
        {
            DeleteScores();
        }
    }

    public void ConfigureLevels()
    {
        SetScores();
        ButtonActivity();

    }

    void ButtonActivity()
    {
        Button[] buttons = ButtonGroup.GetComponentsInChildren<Button>();
        buttons[0].interactable = true;

        int i = 2;

        while(i <= 4)
        { 
            if(PlayerPrefs.HasKey("Level" + (i - 1).ToString() + "Stars"))
            {
                if(PlayerPrefs.GetInt("Level" + (i - 1).ToString() + "Stars") > 1)
                {
                    buttons[i - 1].interactable = true;
                }
            }
            else
            {
                buttons[i - 1].interactable = false;
            }

            i++;
        }
    }

    void SetScores()
    {
        int i = 1;

        foreach (Transform item in ButtonGroup.transform)
        {
            TextMeshProUGUI[] texts = item.GetComponentsInChildren<TextMeshProUGUI>();

            if (!PlayerPrefs.HasKey("Level" + i + "BestTime"))
            {
                return;
            }
            else
            {
                foreach (var text in texts)
                {
                    if (text.text.StartsWith("Best Time :"))
                    {
                        int[] time = MinToHour(PlayerPrefs.GetInt("Level" + i + "BestTime" , 0));
                        text.text = "Best Time : " + time[0] + " hr " + time[1] + " min";
                    }

                    if (text.text.StartsWith("Max Tips :"))
                    {
                        text.text = "Max Tips : $" + PlayerPrefs.GetInt("Level" + i + "Tips",0);
                    }
                }
            }


            if (!PlayerPrefs.HasKey("Level" + i + "Stars"))
            {
                return;
            }
            else
            {
                int j = PlayerPrefs.GetInt("Level" + i + "Stars");

                if( j == 0 )
                {
                    return;
                }
                    Image[] images = item.GetComponentsInChildren<Image>();

                    foreach (var image in images)
                    {
                        if (image.sprite == star)
                        {
                            image.color = yellow;
                            j--;

                            if( j == 0 )
                            {
                                break;
                            }
                        }

                    }
            }
            i++;
        }
    }

   int[] MinToHour(int time)
   {
        int[] t1 = new int[2];

        t1[0] = time / 60;
        t1[1] = time % 60;

        return t1;
   }

    void Scores()
    {
        for (int i = 1; i <= 4; i++)
        {
            Debug.Log("Stars " + i + ": "+ PlayerPrefs.GetInt("Level" + i + "Stars"));
            Debug.Log("BestTime " + i + ": " + PlayerPrefs.GetInt("Level" + i + "BestTime"));
            Debug.Log("Tips " + i + ": " + PlayerPrefs.GetInt("Level" + i + "Tips"));
        }
    }

    void DeleteScores()
    {
        PlayerPrefs.DeleteAll();
    }
}
