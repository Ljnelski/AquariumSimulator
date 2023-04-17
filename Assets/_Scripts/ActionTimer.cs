/*  Filename:           ActionTimer.cs
 *  Author:             Liam Nelski (301064116)
 *  Last Update:        November 21th, 2022
 *  Description:        Timer that has a Callback for a void 
 *  Revision History:   November 12th (Liam Nelski): Inital Script.
 *                      November 13th (Liam Nelski): Added Option Callback for onTick
 *                      November 21st (Liam Nelski): Added Support for Canceling, Pausing and Updating the Timer
 *                      April 16th (Liam Nelski): Removed 'Timer' from funtion name as it is redunent
 *                      April 17th (Liam Nelski): Added Set and Add functions to the Complete and TickCallbacks, removed assignment from start function
 *                      
 *                      
 * Version: 1.0.2
 *                      
 */

using System;

public class ActionTimer
{
    public bool Paused { get => _paused; set => _paused = value; }
    public bool Unpaused { get => !_paused; set => _paused = !value; }

    private Action _onCompleteCallback;
    private Action<ActionTimer> _onAfterCompleteCallback; // Passes itself to a action if something want to be done
    private Action<float> _onTickCallback;
    private float _timerTime;
    private bool _paused;

    // Should after complete be Called?
    private bool _callOnAfterComplete = false;


    public ActionTimer()
    {

    }

    public ActionTimer(bool callOnAfterComplete)
    {
        _callOnAfterComplete = callOnAfterComplete;
    }

    public ActionTimer(bool callOnAfterComplete, Action<ActionTimer> onAfterComplete)
    {
        _callOnAfterComplete = callOnAfterComplete;
        _onAfterCompleteCallback = onAfterComplete;
    }

    // sets the complete callback, overwriting all previous callbacks
    public void SetCompleteCallback(Action completeCallback)
    {
        _onCompleteCallback = completeCallback;
    }

    // adds to the existing complete callbacks
    public void AddCompleteCallback(Action completeCallback)
    {
        _onCompleteCallback += completeCallback;
    }

    // sets the tick callback, overwriting all previous callbacks 
    public void SetTickCallback(Action<float> tickCallback)
    {
        _onTickCallback = tickCallback;
    }

    // adds to the existing tick callbacks
    public void AddTickCallback(Action<float> tickCallback)
    {
        _onTickCallback += tickCallback;
    }

    // starts the timer
    public void Start(float timerLength)
    {
        _paused = false;
        SetTime(timerLength);
    }

    public void SetTime(float newTime)
    {
        _timerTime = newTime;
    }

    public void UpdateTime(float amount)
    {
        _timerTime += amount;
    }

    public void Tick(float deltaTime)
    {
        if (Unpaused)
        {
            if (_timerTime <= 0)
            {
                Complete();
            }

            _timerTime -= deltaTime;
            _onTickCallback?.Invoke(_timerTime);
        }
    }

    public void Complete()
    {
        _paused = true;
        _onCompleteCallback?.Invoke();

        if (_callOnAfterComplete)
        {
            _onAfterCompleteCallback?.Invoke(this);
        }
    }

    // > Clears call backs.
    public void ClearCallbacks()
    {
        _onCompleteCallback = null;
        _onTickCallback = null;
        _onAfterCompleteCallback = null;
    }

    public void Cancel()
    {
        _onCompleteCallback = null;
        _onTickCallback = null;
        _onAfterCompleteCallback?.Invoke(this);
    }
}

