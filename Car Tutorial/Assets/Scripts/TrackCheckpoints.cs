using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TrackCheckpoints : MonoBehaviour
{
    public event EventHandler OnPlayerCorrectCheckpoint;
    public event EventHandler OnPlayerWrongCheckpoint;

    private List<CheckpointSingle> checkpointSingleList;

    private int nextCheckpointSingleIndex;
    private int currentCheckpointSingleIndex;

    private int lapCount;
    private int currentLap;
    [SerializeField] private int maxLaps;

    private bool totalTimerRunning = false;
    private bool lap1TimerRunning = false;
    private bool lap2TimerRunning = false;
    private bool lap3TimerRunning = false;

    private static int lap1MinuteCount = 0;
    private static int lap1SecondCount = 0;
    private static float lap1MilliCount = 0;
    //private static string lap1MilliDisplay;
    [SerializeField] private TextMeshProUGUI lap1MinuteBox;
    [SerializeField] private TextMeshProUGUI lap1SecondBox;
    [SerializeField] private TextMeshProUGUI lap1MilliBox;

    private static int lap2MinuteCount = 0;
    private static int lap2SecondCount = 0;
    private static float lap2MilliCount = 0;
    //private static string lap2MilliDisplay;
    [SerializeField] private TextMeshProUGUI lap2MinuteBox;
    [SerializeField] private TextMeshProUGUI lap2SecondBox;
    [SerializeField] private TextMeshProUGUI lap2MilliBox;

    private static int lap3MinuteCount = 0;
    private static int lap3SecondCount = 0;
    private static float lap3MilliCount = 0;
    //private static string lap3MilliDisplay;
    [SerializeField] private TextMeshProUGUI lap3MinuteBox;
    [SerializeField] private TextMeshProUGUI lap3SecondBox;
    [SerializeField] private TextMeshProUGUI lap3MilliBox;

    private static int totalMinuteCount = 0;
    private static int totalSecondCount = 0;
    private static float totalMilliCount = 0;
    //private static string totalMilliDisplay;
    [SerializeField] private TextMeshProUGUI totalMinuteBox;
    [SerializeField] private TextMeshProUGUI totalSecondBox;
    [SerializeField] private TextMeshProUGUI totalMilliBox;

    private void Awake()
    {
        Transform checkpointsTransform = transform.Find("FunctionalCheckpoints");

        checkpointSingleList = new List<CheckpointSingle>();

        foreach (Transform checkpointSingleTransform in checkpointsTransform)
        {
            CheckpointSingle checkpointSingle = checkpointSingleTransform.GetComponent<CheckpointSingle>();

            checkpointSingle.SetTrackCheckpoints(this);

            checkpointSingleList.Add(checkpointSingle);
        }

        nextCheckpointSingleIndex = 0;
        currentCheckpointSingleIndex = -1;
        lapCount = 0;
        currentLap = 0;
    }

    private void Update()
    {
        if (totalTimerRunning)
        {
            totalMilliCount += Time.deltaTime * 10;
            totalMilliBox.text = "" + totalMilliCount.ToString("F0");

            if (totalMilliCount >= 10)
            {
                totalMilliCount = 0;
                totalSecondCount++;
            }

            if (totalSecondCount <= 9)
            {
                totalSecondBox.text = "0" + totalSecondCount + ".";
            }
            else
            {
                totalSecondBox.text = "" + totalSecondCount + ".";
            }
            if (totalSecondCount >= 60)
            {
                totalSecondCount = 0;
                totalMinuteCount++;
            }

            if (totalMinuteCount <= 9)
            {
                totalMinuteBox.text = "0" + totalMinuteCount + ":";
            }
            else
            {
                totalMinuteBox.text = "" + totalMinuteCount + ":";
            }
        }

        if (lap1TimerRunning && currentLap == 0)
        {
            lap1MilliCount += Time.deltaTime * 10;
            lap1MilliBox.text = "" + lap1MilliCount.ToString("F0");

            if (lap1MilliCount >= 10)
            {
                lap1MilliCount = 0;
                lap1SecondCount++;
            }

            if (lap1SecondCount <= 9)
            {
                lap1SecondBox.text = "0" + lap1SecondCount + ".";
            }
            else
            {
                lap1SecondBox.text = "" + lap1SecondCount + ".";
            }
            if (lap1SecondCount >= 60)
            {
                lap1SecondCount = 0;
                lap1MinuteCount++;
            }

            if (lap1MinuteCount <= 9)
            {
                lap1MinuteBox.text = "0" + lap1MinuteCount + ":";
            }
            else
            {
                lap1MinuteBox.text = "" + lap1MinuteCount + ":";
            }
        }

        if (lap2TimerRunning && currentLap == 1)
        {
            lap2MilliCount += Time.deltaTime * 10;
            lap2MilliBox.text = "" + lap2MilliCount.ToString("F0");

            if (lap2MilliCount >= 10)
            {
                lap2MilliCount = 0;
                lap2SecondCount++;
            }

            if (lap2SecondCount <= 9)
            {
                lap2SecondBox.text = "0" + lap2SecondCount + ".";
            }
            else
            {
                lap2SecondBox.text = "" + lap2SecondCount + ".";
            }
            if (lap2SecondCount >= 60)
            {
                lap2SecondCount = 0;
                lap2MinuteCount++;
            }

            if (lap2MinuteCount <= 9)
            {
                lap2MinuteBox.text = "0" + lap2MinuteCount + ":";
            }
            else
            {
                lap2MinuteBox.text = "" + lap2MinuteCount + ":";
            }
        }

        if (lap3TimerRunning && currentLap == 2)
        {
            lap3MilliCount += Time.deltaTime * 10;
            lap3MilliBox.text = "" + lap3MilliCount.ToString("F0");

            if (lap3MilliCount >= 10)
            {
                lap3MilliCount = 0;
                lap3SecondCount++;
            }

            if (lap3SecondCount <= 9)
            {
                lap3SecondBox.text = "0" + lap3SecondCount + ".";
            }
            else
            {
                lap3SecondBox.text = "" + lap3SecondCount + ".";
            }
            if (lap3SecondCount >= 60)
            {
                lap3SecondCount = 0;
                lap3MinuteCount++;
            }

            if (lap3MinuteCount <= 9)
            {
                lap3MinuteBox.text = "0" + lap3MinuteCount + ":";
            }
            else
            {
                lap3MinuteBox.text = "" + lap3MinuteCount + ":";
            }
        }
    }

    public void PlayerThroughCheckpoint(CheckpointSingle checkpointSingle)
    {
        if (checkpointSingleList.IndexOf(checkpointSingle) == nextCheckpointSingleIndex)
        {
            CheckpointSingle correctCheckpointSingle = checkpointSingleList[nextCheckpointSingleIndex];
            correctCheckpointSingle.Hide();

            nextCheckpointSingleIndex++;
            if (nextCheckpointSingleIndex == 23)
            {
                nextCheckpointSingleIndex = 0;
            }
            currentCheckpointSingleIndex++;
            if (currentCheckpointSingleIndex == 23)
            {
                currentCheckpointSingleIndex = 0;
                lapCount++;
                currentLap++;
                //Debug.Log("Laps " + lapCount);
                if (lapCount == maxLaps)
                {
                    totalTimerRunning = false;
                    lap1TimerRunning = false;
                    lap2TimerRunning = false;
                    lap3TimerRunning = false;
                }
            }

            OnPlayerCorrectCheckpoint?.Invoke(this, EventArgs.Empty);
            Debug.Log("Next checkpoint " + nextCheckpointSingleIndex);
            Debug.Log("Previous checkpoint " + currentCheckpointSingleIndex);

            if (currentCheckpointSingleIndex == 0 && currentLap == 0)
            {
                totalTimerRunning = true;
                lap1TimerRunning = true;
            }

            if (currentCheckpointSingleIndex == 0 && currentLap == 1)
            {
                lap1TimerRunning = false;
                lap2TimerRunning = true;
                lap3TimerRunning = false;
            }
            if (currentCheckpointSingleIndex == 0 && currentLap == 2)
            {
                lap1TimerRunning = false;
                lap2TimerRunning = false;
                lap3TimerRunning = true;
            }
        }
        else
        {
            OnPlayerWrongCheckpoint?.Invoke(this, EventArgs.Empty);

            CheckpointSingle correctCheckpointSingle = checkpointSingleList[nextCheckpointSingleIndex];
            correctCheckpointSingle.Show();
        }
    }
}