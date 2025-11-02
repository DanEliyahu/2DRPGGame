using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    [SerializeField] private ParallaxLayer[] backgroundLayers;

    private Camera mainCamera;
    private float lastCameraPositionX;
    private float cameraHalfWidth;

    private void Start()
    {
        mainCamera = Camera.main;
        if (mainCamera)
            cameraHalfWidth = mainCamera.orthographicSize * mainCamera.aspect;

        foreach (ParallaxLayer layer in backgroundLayers)
        {
            layer.Initialize();
        }
    }

    private void FixedUpdate()
    {
        float currentCameraPositionX = mainCamera.transform.position.x;
        float distanceToMove = currentCameraPositionX - lastCameraPositionX;
        lastCameraPositionX = currentCameraPositionX;

        float cameraLeftEdge = currentCameraPositionX - cameraHalfWidth;
        float cameraRightEdge = currentCameraPositionX + cameraHalfWidth;

        foreach (ParallaxLayer layer in backgroundLayers)
        {
            layer.Move(distanceToMove);
            layer.LoopBackground(cameraLeftEdge, cameraRightEdge);
        }
    }
}

[System.Serializable]
public class ParallaxLayer
{
    [SerializeField] private Transform parallaxLayer;
    [SerializeField] private float parallaxSpeedMultiplier;
    [SerializeField] private float imageWidthOffset = 10;

    private float imageFullWidth;
    private float imageHalfWidth;

    public void Initialize()
    {
        CalculateImageWidth();
    }

    private void CalculateImageWidth()
    {
        imageFullWidth = parallaxLayer.GetComponent<SpriteRenderer>().bounds.size.x;
        imageHalfWidth = imageFullWidth / 2;
    }

    public void Move(float distanceToMove)
    {
        parallaxLayer.position += Vector3.right * (distanceToMove * parallaxSpeedMultiplier);
    }

    public void LoopBackground(float cameraLeftEdge, float cameraRightEdge)
    {
        float imageRightEdge = (parallaxLayer.position.x + imageHalfWidth) - imageWidthOffset;
        float imageLeftEdge = (parallaxLayer.position.x - imageHalfWidth) + imageWidthOffset;

        if (imageRightEdge < cameraLeftEdge)
            parallaxLayer.position += Vector3.right * imageFullWidth;
        else if (imageLeftEdge > cameraRightEdge)
            parallaxLayer.position += Vector3.left * imageFullWidth;
    }
}