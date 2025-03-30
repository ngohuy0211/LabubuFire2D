using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderRunner
{
    private readonly Slider _sliderUI;
    private readonly Text _textValue;
    //
    private float _durationPace = 0.1f;
    private bool _onRunning = false;
    private float _speed = 0f;
    private float _destValue = 0f;

    public SliderRunner(Slider slider, float durationPace = 0.1f)
    {
        _sliderUI = slider;
        _durationPace = durationPace;
    }

    public SliderRunner(Slider slider, Text textValue, float durationPace = 0.1f)
    {
        _sliderUI = slider;
        _durationPace = durationPace;
        _textValue = textValue;
    }

    public float GetCurrentValue()
    {
        return _sliderUI.value;
    }

    public void SetSliderValue(float value)
    {
        if (_sliderUI == null)
        {
            return;
        }

        if (value < 0)
            value = 0f;
        if (value > 1)
            value = 1f;
        //
        _sliderUI.value = value;
    }

    /// <summary>
    /// Value In Seconds
    /// </summary>
    public void SetDurationPace(float duration)
    {
        _durationPace = duration;
    }

    public void RunSliderValue(float value)
    {
        if (_sliderUI == null)
        {
            return;
        }

        if (value < 0)
            value = 0f;
        if (value > 1)
            value = 1f;
        //
        if (value != _sliderUI.value)
        {
            _destValue = value;
            _speed = (_destValue - _sliderUI.value) / (_durationPace);
            _onRunning = true;
        }
    }

    public void UpdateSliderRunner(float deltaTime)
    {
        if (_sliderUI == null)
            return;

        if (_onRunning && _speed != 0)
        {
            float offset = deltaTime * _speed;
            //
            float currentValue = _sliderUI.value;
            float nextValue = _sliderUI.value + offset;
            //
            if (nextValue > currentValue)
            {
                if (nextValue >= _destValue)
                {
                    nextValue = _destValue;
                    _onRunning = false;
                }
            }
            else
            {
                if (nextValue <= _destValue)
                {
                    nextValue = _destValue;
                    _onRunning = false;
                }
            }

            _sliderUI.value = nextValue;
            if (_textValue)
                _textValue.text = Mathf.Round(_sliderUI.value * 100) + "%";
        }
    }
}