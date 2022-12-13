using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARTrackedImageManager))]
public class ObjectPlacement : MonoBehaviour
{
    private ARTrackedImageManager aRTrackedImageManager;
    private readonly Dictionary<string,GameObject> InstantiatedArPrefab = new Dictionary<string, GameObject>();
    public GameObject[] ARPrefabs;

    private void Awake()
    {
        aRTrackedImageManager = GetComponent<ARTrackedImageManager>();
        
    }

    private void OnEnable()
    {
        aRTrackedImageManager.trackedImagesChanged += OnTrackedImageChanged;
    }
    private void OnDisable()
    {
        aRTrackedImageManager.trackedImagesChanged -= OnTrackedImageChanged;
    }

    private void OnTrackedImageChanged(ARTrackedImagesChangedEventArgs Args)
    {
        
        foreach (var image in Args.added)
        {
            var ImgName = image.referenceImage.name;


            foreach (var currentPrefab in ARPrefabs)
            {
                if (string.Compare(currentPrefab.name, ImgName,System.StringComparison.OrdinalIgnoreCase) == 0 && !InstantiatedArPrefab.ContainsKey(ImgName))
                {
                    var CreatePrefab = Instantiate(currentPrefab,image.transform);
                    InstantiatedArPrefab[ImgName] = CreatePrefab;
                }
            }
        }
        //image  changed
        foreach(var image in Args.updated)
        {
            InstantiatedArPrefab[image.referenceImage.name].SetActive(image.trackingState == TrackingState.Tracking);
        }
        //image reemoved from scene
        foreach (var image in Args.removed)
        {
            Destroy(InstantiatedArPrefab[image.referenceImage.name]);
            InstantiatedArPrefab.Remove(image.referenceImage.name);
        }
    }
    
}
