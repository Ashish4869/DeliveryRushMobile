using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PostProcessing : MonoBehaviour
{
    /// <summary>
    /// Responsible for the thunder effect in the final level
    /// </summary>

    public PostProcessVolume volume;

    ColorGrading _colorGrading;

    float minTimeBetweenThunderStorms = 10;
    float maxTimeBetweenThunderStorms = 20;
    float timer = 5f;
    bool ThunderStruck;

    float MaxThunderLight = 10f;

    // Start is called before the first frame update
    void Start()
    {
        volume.profile.TryGetSettings(out _colorGrading);
        _colorGrading.postExposure.value = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        ScheduleThunderStorms();
    }

    void ScheduleThunderStorms()
    {
        if(timer > 0)
        {
            timer -= Time.deltaTime;
        }
        else
        {
            StrikeThunder();
        }
    }

    void StrikeThunder()
    {
        if (_colorGrading.postExposure.value > MaxThunderLight - 0.5)
        {
            //play thunder sound
            FindObjectOfType<AudioManager>().Play("THUNDER");
            ThunderStruck = true;
        }

        if (!ThunderStruck)
        {
            _colorGrading.postExposure.value = Mathf.Lerp(_colorGrading.postExposure.value, MaxThunderLight, 10 * Time.deltaTime);
        }
        else
        {
            _colorGrading.postExposure.value = Mathf.Lerp(_colorGrading.postExposure.value, 0, 7 * Time.deltaTime);
        }

        if (_colorGrading.postExposure.value < 0.1)
        {
            ThunderStruck = false;
            timer = Random.Range(minTimeBetweenThunderStorms, maxTimeBetweenThunderStorms);
        }
    }
}
