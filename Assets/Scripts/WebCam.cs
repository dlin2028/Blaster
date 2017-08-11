using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.VR.WSA.WebCam;

public class WebCam : MonoBehaviour
{
    [HideInInspector]
    public static Texture2D Texture;
    public float secondsInterval = 5;

    Resolution cameraResolution;
    PhotoCapture photoCaptureObject;


    void Start()
    {
        cameraResolution = PhotoCapture.SupportedResolutions.OrderByDescending((res) => res.width * res.height).First();



        StartCoroutine(updateCamera());
    }

    IEnumerator updateCamera()
    {
        while (true)
        {
            AnalyzeScene();
            yield return new WaitForSeconds(secondsInterval);
        }
    }

    void AnalyzeScene()
    {
        PhotoCapture.CreateAsync(false, OnPhotoCaputreCreated);
    }

    void OnPhotoCaputreCreated(PhotoCapture capuredObject)
    {
        photoCaptureObject = capuredObject;
        CameraParameters parameters = new CameraParameters();
        parameters.hologramOpacity = 0;
        parameters.cameraResolutionWidth = cameraResolution.width;
        parameters.cameraResolutionHeight = cameraResolution.height;
        parameters.pixelFormat = CapturePixelFormat.BGRA32;

        capuredObject.StartPhotoModeAsync(parameters, OnPhotoModeStarted);
    }

    void OnPhotoModeStarted(PhotoCapture.PhotoCaptureResult result)
    {
        if (result.success)
        {
            try
            {
                photoCaptureObject.TakePhotoAsync(OnCapturePhotoToMemory);
            }
            catch (System.ArgumentException e)
            {
                Debug.LogError(":c");
            }
        }
        else
        {
            //stuff
        }
    }

    private void OnCapturePhotoToMemory(PhotoCapture.PhotoCaptureResult result, PhotoCaptureFrame frame)
    {
        if (result.success)
        {
            var texture = new Texture2D(cameraResolution.width, cameraResolution.height, TextureFormat.RGB24, false);
            frame.UploadImageDataToTexture(texture);

            Texture = texture;
        }

        photoCaptureObject.StopPhotoModeAsync(OnStoppedPhotoMode);
    }

    void OnStoppedPhotoMode(PhotoCapture.PhotoCaptureResult result)
    {
        photoCaptureObject.Dispose();
        photoCaptureObject = null;
    }
}
