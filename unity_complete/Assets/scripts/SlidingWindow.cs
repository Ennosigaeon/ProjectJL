using UnityEngine;
using System.Collections;

public class SlidingWindow
{

    private readonly int SLIDING_WINDOW_SIZE;

    private Vector3[] sliding_window;
    private int pointer;

    public SlidingWindow(int sliding_window_size)
    {
        SLIDING_WINDOW_SIZE = sliding_window_size;
        sliding_window = new Vector3[SLIDING_WINDOW_SIZE];
        for (int i = 0; i < SLIDING_WINDOW_SIZE; i++)
        {
            sliding_window[i] = Vector3.zero;
        }
    }

    public void push(Vector3 value)
    {
        sliding_window[pointer] = value;
        pointer = (pointer + 1) % SLIDING_WINDOW_SIZE;
    }

    public Vector3 pop()
    {
        Vector3 tmp = Vector3.zero;
        for (int i = 0; i < SLIDING_WINDOW_SIZE; i++)
        {
            //tmp += sliding_window[(pointer + i) % SLIDING_WINDOW_SIZE] * Mathf.Pow(0.5f, SLIDING_WINDOW_SIZE - i );
            tmp += sliding_window[(pointer + i) % SLIDING_WINDOW_SIZE];
        }
        return tmp / SLIDING_WINDOW_SIZE;

    }






}
