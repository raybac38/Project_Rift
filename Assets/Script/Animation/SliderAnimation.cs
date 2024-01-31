using System.Collections;
using UnityEngine;

public class SliderAnimation : MonoBehaviour
{
    [SerializeField]
    private byte startingState = 0;
    [SerializeField]
    private bool startingWithAnimation = false;
    [SerializeField]
    private byte secondStartingState = 0;
    [SerializeField]
    private float animationDuration = 2f;
    public Vector3[] positions;
    private byte state;

    private void Start() {
        transform.localPosition = positions[startingState];
        state = startingState;

        if(startingWithAnimation)
        {
            SetState(secondStartingState);
        }
    }

    public byte GetState()
    {
        return state;
    }

    public void SetState(int intNewState)
    {
        byte newState = (byte)intNewState;
        StartCoroutine(SlideAnimation(state, newState, animationDuration));
        state = newState;
    }

    IEnumerator SlideAnimation(byte oldStat, byte newState, float duration)
    {
        float startingTime = Time.time;
        float endingTime = startingTime + duration;

        Vector3 oldPosition = positions[oldStat];
        Vector3 newPosition = positions[newState];

        float t;

        while(Time.time < endingTime)
        {
            t = (Time.time - startingTime) / duration;
            transform.localPosition = Vector3.Slerp(oldPosition, newPosition, t);
            yield return null;
        }
        transform.localPosition = newPosition;
    }
}
