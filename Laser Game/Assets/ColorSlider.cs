using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorSlider : MonoBehaviour
{
    public Slider slider;
    public Image knob;

    public static Color WavToCol(float wav, float alpha)
    {
        //Approximates RGB
        //Based on code by Dan Bruton
        //http://www.physics.sfasu.edu/astro/color/spectra.html
        
        float r;
        float g;
        float b;
        float reductionFactor;
        float gamma = 0.8f;
        if (wav >= 380 && wav <= 440)
        {
            reductionFactor = 0.3f + 0.7f * (wav - 380f) / (440f - 380f);
            r = Mathf.Pow((-(wav - 440f) / (440f - 380f)) * reductionFactor, gamma);
            g = 0.0f;
            b = Mathf.Pow((1.0f * reductionFactor), gamma);
        }
        else if (wav >= 440 && wav <= 490)
        {
            r = 0.0f;
            g = Mathf.Pow((wav - 440f) / (490f - 440f), gamma);
            b = 1.0f;
        }
        else if (wav >= 490 && wav <= 510)
        {
            r = 0.0f;
            g = 1.0f;
            b = Mathf.Pow(-(wav - 510f) / (510f - 490f), gamma);
        }
        else if (wav >= 510 && wav <= 580)
        {
            r = Mathf.Pow((wav - 510f) / (580f - 510f), gamma);
            g = 1.0f;
            b = 0.0f;
        }
        else if (wav >= 580f && wav <= 645f)
        {
            r = 1.0f;
            g = Mathf.Pow(-(wav - 645f) / (645f - 580f), gamma);
            b = 0.0f;
        }
        else if (wav >= 645f && wav <= 750f)
        {
            reductionFactor = 0.3f + 0.7f * (750f - wav) / (750f - 645f);
            r = Mathf.Pow((1.0f * reductionFactor), gamma);
            g = 0.0f;
            b = 0.0f;
        }
        else
        {
            r = 0.0f;
            g = 0.0f;
            b = 0.0f;
        }
        return new Color(r, g, b, alpha);
    }

    public static float WavToRef(float wav) {
         return 2.1f - (wav-380)/400;
    }

    public Color SliderColor() {
        return WavToCol(slider.value, 1.0f);
    }

    public float SliderRef() {
        return WavToRef(slider.value);
    }

    // Update is called once per frame
    void Update()
    {
        knob.color = SliderColor();
    }
}
