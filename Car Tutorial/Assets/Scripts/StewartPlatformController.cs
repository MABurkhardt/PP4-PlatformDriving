using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;

public class StewartPlatformController : MonoBehaviour
{
    public static StewartPlatformController instance;

    public enum PlatformModes
    {
        Mode_8Bit,
        Mode_Float
    }
    public PlatformModes mode = PlatformModes.Mode_Float;

    private SerialPort serialPort;
    public string comPort;
    public int baudRate;
    bool initialized = false;

    public byte[] byteValues;
    public float[] floatValues;

    private string startFrame = "!";
    private string endFrame = "#";

    private float nextSendTimestamp = 0;
    private float nextSendDelay = .02f;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        if (!initialized)
        {
            Init(comPort, baudRate);
        }
    }

    public void Init(string _com, int _baud)
    {
        if (initialized)
        {
            Debug.LogWarning(typeof(StewartPlatformController).ToString() + ": is already initialized");
            return;
        }

        initialized = true;

        comPort = _com;
        baudRate = _baud;
        byteValues = new byte[] { 128, 128, 128, 128, 128, 128 };
        floatValues = new float[] { 0, 0, 0, 0, 0, 0 };

        if (serialPort == null)
        {
            serialPort = new SerialPort(@"\\.\" + comPort);
            serialPort.BaudRate = baudRate;
            serialPort.Parity = Parity.None;
            serialPort.DataBits = 8;
        }

        try
        {
            serialPort.Open();
            Debug.Log("Initialize Serial Port: " + comPort);
        }
        catch (System.IO.IOException ex)
        {
            Debug.LogError("Error opening " + comPort + "\n" + ex.Message);
        }

        //if (sliderControls != null)
        //{
        //    sliderControls.SetSlideParameters(this);
        //}

        HomePlatform();
    }

    private void Update()
    {
        if (Time.time > nextSendTimestamp)
        {
            nextSendTimestamp = Time.time + nextSendDelay;
            //if (sliderControls != null)
            //{
            //    SendSerialFromSliders();
            //}
            //else
            //{
                SendSerial();
            //}
        }
    }

    public float MapRange(float val, float min, float max, float newMin, float newMax)
    {
        return Mathf.Clamp(((val - min) / (max - min) * (newMax - newMin) + newMin), newMin, newMax);
    }

    public void SendSerial()
    {
        if (serialPort != null && serialPort.IsOpen)
        {
            //8 bit mode
            if (mode == PlatformModes.Mode_8Bit)
            {
                serialPort.Write(startFrame);
                serialPort.Write(byteValues, 0, byteValues.Length);
                serialPort.Write(endFrame);
            }
            //float mode
            else if (mode == PlatformModes.Mode_Float)
            {
                serialPort.Write(startFrame);
                for (int i = 0; i < floatValues.Length; i++)
                {
                    byte[] myBytes = System.BitConverter.GetBytes(floatValues[i]);
                    serialPort.Write(myBytes, 0, myBytes.Length);
                }
                serialPort.Write(endFrame);
            }
        }
    }

    //public void SendSerialFromSliders()
    //{
    //    for (int i = 0; i < SliderControls.instance.sliders.Length; i++)
    //    {
    //        if (mode == PlatformModes.Mode_8Bit) byteValues[i] = (byte)SliderControls.instance.sliders[i].value;
    //        else if (mode == PlatformModes.Mode_Float) floatValues[i] = SliderControls.instance.sliders[i].value;
    //    }
    //    SendSerial();
    //}

    public void HomePlatform()
    {
        if (serialPort != null && serialPort.IsOpen)
        {
            //8 bit mode
            if (mode == PlatformModes.Mode_8Bit)
            {
                for (int i = 0; i < byteValues.Length; i++)
                {
                    byteValues[i] = 128;
                }
            }
            //float mode
            else if (mode == PlatformModes.Mode_Float)
            {
                for (int i = 0; i < floatValues.Length; i++)
                {
                    floatValues[i] = 0;
                }
            }

            SendSerial();
        }
    }

    private void OnApplicationQuit()
    {
        if (serialPort != null && serialPort.IsOpen)
        {
            //if (sliderControls != null)
            //{
            //    sliderControls.ResetSliders();
            //}
            HomePlatform();
            serialPort.Close();
        }
    }

    private static StewartPlatformController _singleton;
    public static StewartPlatformController singleton
    {
        get
        {
            if (_singleton == null)
            {
                GameObject go = new GameObject("PlatformController");
                DontDestroyOnLoad(go);
                _singleton = go.AddComponent<StewartPlatformController>();
            }

            return _singleton;
        }
    }
}