using UnityEngine;
using System.Collections;
using System;

public class CubeMyoController : MyoPoseBaseController
{
    protected override void onDoubleTap()
    {
        Debug.LogError("OnDoubleTap");
    }

    protected override void onFingerSpread()
    {
        Debug.LogError("FingerSpread");
    }

    protected override void onFist()
    {
        Debug.LogError("Fist");
    }

    protected override void onRest()
    {
        Debug.LogError("Rest");
    }

    protected override void onUnknown()
    {
        Debug.LogError("Unknown");
    }

    protected override void onWaveIn()
    {
        Debug.LogError("WaveIn");
    }

    protected override void onWaveOut()
    {
        Debug.LogError("WaveOut");
    }

    protected override void reset()
    {
        Debug.LogError("Reset");
    }
}
