using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.VR.WSA.WebCam;

public class Fireball : MonoBehaviour
{

    public GameObject Hole;


    public Vector3 StartingVector;
    public Vector3 StartingTorque;

    public Vector2 cropSize = new Vector2(100, 100);

    Material material;
    GameObject createdObject;



    private void launch()
    {
        Rigidbody body = GetComponent<Rigidbody>();
        body.AddRelativeForce(StartingVector);
        body.AddTorque(StartingTorque);
    }
    private void OnCollisionEnter(Collision collision)
    {
        createdObject = Instantiate(Hole);
        createdObject.transform.SetPositionAndRotation(transform.position, transform.rotation);
        createdObject.transform.position -= createdObject.transform.forward;

        RaycastHit hitInfo;
        Physics.Raycast(createdObject.transform.position, createdObject.transform.forward, out hitInfo);

        createdObject.transform.position = hitInfo.point;
        createdObject.transform.rotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);
        createdObject.transform.Rotate(-90, 0, 0);


        Texture2D texture = WebCam.Texture;
        Texture2D croppedTexture = new Texture2D((int)cropSize.x, (int)cropSize.y, TextureFormat.RGB24, false);
        

        Color[] pixels = texture.GetPixels((int)(texture.width / 2 - cropSize.x / 2),
                                           (int)(texture.height / 2 - cropSize.y / 2),
                                           (int)cropSize.x, (int)cropSize.y);
        croppedTexture.SetPixels(pixels);
        croppedTexture.Apply();

        createdObject.transform.GetChild(0).GetComponent<MeshRenderer>().material.mainTexture = croppedTexture;

        Destroy(gameObject);
    }
}
