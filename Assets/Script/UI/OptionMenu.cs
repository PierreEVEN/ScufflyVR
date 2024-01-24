using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.Rendering;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class OptionMenu : MonoBehaviour
{
    public GameObject Frame;

    public Vector3 mainPosition;
    public Vector3 settingsPosition;
    public Vector3 inputsPosition;
    private Vector3 targetPosition;

    public Slider foliageSlider;
    public Slider landscapeSlider;
    public Slider graphicsSLider;
    public Slider dayTimeSlider;

    public GameObject enableTutorialButton;

    void Start()
    {
        GameObject controller = PlayerController.LocalPlayerController.LeftXRController;
        transform.parent = controller.transform;
        transform.SetLocalPositionAndRotation(new Vector3(0, 0.175f, 0.147f), Quaternion.Euler(25, 0, 0));

        targetPosition = mainPosition;
        Time.timeScale = 0;
        dayTimeSlider.value = GameObject.Find("Lighting").GetComponent<DayNightCycle>().getRotation() % 360;
        enableTutorialButton.GetComponent<Toggle>().onValueChanged.AddListener(tutorialChanged);
    }

    void tutorialChanged(bool enabled)
    {
        PlayerController.LocalPlayerController.GetComponentInChildren<TutorialManager>().enabled = enableTutorialButton.GetComponent<Toggle>().isOn;
    }

    void Update()
    {
        Vector3 worldTarget = Frame.transform.parent.TransformPoint(targetPosition);

        Frame.transform.position = Vector3.Lerp(Frame.transform.position, worldTarget, Time.unscaledDeltaTime * 10);
    }


    void OnDestroy()
    {
        Time.timeScale = 1;
    }

    public void Continue()
    {
        Destroy(gameObject);
    }
    public void GoToStart()
    {
        GameObject buyMenuPoint = GameObject.FindGameObjectWithTag("BuySpawnPoint");
        PlayerController.LocalPlayerController.MoveToLocation(buyMenuPoint.transform.position,
            Vector3.SignedAngle(buyMenuPoint.transform.forward,
                new Vector3(1, 0, 0), Vector3.up));
        Destroy(gameObject);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void resetCamera()
    {
        PlayerController.LocalPlayerController.OnCenterCamera(new InputValue());
    }

    public void goSettings()
    {
        targetPosition = settingsPosition;
    }
    public void goInputs()
    {
        targetPosition = inputsPosition;
        Debug.Log(targetPosition);
    }
    public void goMain()
    {
        targetPosition = mainPosition;
    }

    public void setLightingQuality()
    {
        int v = (int)graphicsSLider.value;
        switch (v)
        {
            case 0:
                AmbientOcclusion = false;
                Bloom = false;
                Fog = false;
                VolumetricClouds = false;
                GlobalShaderQuality = 0;
                Shadows = false;
                ContactShadows = false;
                break;
            case 1:
                AmbientOcclusion = false;
                Bloom = true;
                Fog = false;
                VolumetricClouds = false;
                GlobalShaderQuality = 0;
                Shadows = false;
                ContactShadows = false;
                break;
            case 2:
                AmbientOcclusion = true;
                Bloom = true;
                Fog = true;
                VolumetricClouds = true;
                GlobalShaderQuality = 1;
                Shadows = true;
                ContactShadows = true;
                break;
            case 3:
                AmbientOcclusion = true;
                Bloom = true;
                Fog = true;
                VolumetricClouds = true;
                GlobalShaderQuality = 2;
                Shadows = true;
                ContactShadows = true;
                break;
            case 4:
                AmbientOcclusion = true;
                Bloom = true;
                Fog = true;
                VolumetricClouds = true;
                GlobalShaderQuality = 2;
                Shadows = true;
                ContactShadows = true;
                break;
        }
    }
    public void setTerrainQuality()
    {
        int v = (int)landscapeSlider.value;
        switch (v)
        {
            case 0:
                LandscapeQuality = 10;
                LandscapeViewDistance = 2;
                break;
            case 1:
                LandscapeQuality = 25;
                LandscapeViewDistance = 4;
                break;
            case 2:
                LandscapeQuality = 50;
                LandscapeViewDistance = 2;
                break;
            case 3:
                LandscapeQuality = 80;
                LandscapeViewDistance = 3;
                break;
            case 4:
                LandscapeQuality = 100;
                LandscapeViewDistance = 4;
                break;
        }
    }
    public void setFoliageQuality()
    {
        int v = (int)foliageSlider.value;
        switch (v)
        {
            case 0:
                FolliageQuality = 0f;
                break;
            case 1:
                FolliageQuality = 0.025f;
                break;
            case 2:
                FolliageQuality = 0.1f;
                break;
            case 3:
                FolliageQuality = 0.2f;
                break;
            case 4:
                FolliageQuality = 0.5f;
                break;
        }
    }

    public void setDayTime()
    {
        GameObject.Find("Lighting").GetComponent<DayNightCycle>().SetRotation(dayTimeSlider.value);
    }

    /// <summary>
    /// Find the gameobject that contain the volume with all the HDRP settings
    /// </summary>
    public static Volume GlobalVolume
    {
        get
        {
            if (!_globalVolume)
            {
                GameObject lighting = GameObject.Find("Lighting");
                if (lighting)
                    _globalVolume = lighting.GetComponent<Volume>();
            }
            return _globalVolume;
        }
    }
    static Volume _globalVolume;

    public static int LandscapeViewDistance
    {
        get
        {
            GameObject landscape = GameObject.FindGameObjectWithTag("GPULandscape");
            if (landscape && landscape.GetComponent<GPULandscape>())
            {
                return landscape.GetComponent<GPULandscape>().sectionLoadDistance;
            }
            return 0;
        }
        set
        {
            GameObject landscape = GameObject.FindGameObjectWithTag("GPULandscape");
            if (landscape && landscape.GetComponent<GPULandscape>())
            {
                landscape.GetComponent<GPULandscape>().sectionLoadDistance = value;
                landscape.GetComponent<GPULandscape>().Reset = true;
            }
        }
    }


    public static int LandscapeQuality
    {
        get
        {
            GameObject landscape = GameObject.FindGameObjectWithTag("GPULandscape");
            if (landscape && landscape.GetComponent<GPULandscape>())
            {
                return landscape.GetComponent<GPULandscape>().meshDensity;
            }
            return 0;
        }
        set
        {
            GameObject landscape = GameObject.FindGameObjectWithTag("GPULandscape");
            if (landscape && landscape.GetComponent<GPULandscape>())
            {
                landscape.GetComponent<GPULandscape>().meshDensity = value;
                landscape.GetComponent<GPULandscape>().Reset = true;
            }
        }
    }


    public static float FolliageQuality
    {
        get
        {
            GameObject landscape = GameObject.FindGameObjectWithTag("GPULandscape");
            if (landscape && landscape.GetComponent<ProceduralFolliageSpawner>())
            {
                if (!landscape.activeSelf)
                    return 0;
                return landscape.GetComponent<ProceduralFolliageSpawner>().densityMultiplier;
            }
            return 0;
        }
        set
        {
            GameObject landscape = GameObject.FindGameObjectWithTag("GPULandscape");
            if (landscape && landscape.GetComponent<ProceduralFolliageSpawner>())
            {
                landscape.GetComponent<ProceduralFolliageSpawner>().densityMultiplier = value;
                landscape.GetComponent<ProceduralFolliageSpawner>().Reset = true;
            }
        }
    }

    public static int GlobalShaderQuality
    {
        get
        {
            return QualitySettings.GetQualityLevel();
        }
        set
        {
            QualitySettings.SetQualityLevel(Mathf.Clamp(value, 0, 2));
        }
    }


    public static bool Shadows
    {
        get
        {
            return GameObject.Find("Directional Light Sun").GetComponent<Light>().shadows != LightShadows.None;
        }
        set
        {
            GameObject.Find("Directional Light Sun").GetComponent<Light>().shadows = value ? LightShadows.Hard : LightShadows.None;
        }
    }

    public static bool VolumetricClouds
    {
        get { return GetVolumeComponent<VolumetricClouds>().active; }
        set
        {
            GetVolumeComponent<VolumetricClouds>().active = value;
        }
    }


    public static bool ContactShadows
    {
        get { return GetVolumeComponent<ContactShadows>().enable.value; }
        set
        {
            GetVolumeComponent<ContactShadows>().enable.value = value;
        }
    }

    public static bool Fog
    {
        get { return GetVolumeComponent<Fog>().enabled.value; }
        set
        {
            GetVolumeComponent<Fog>().enabled.value = value;
        }
    }

    public static bool Bloom
    {
        get { return GetVolumeComponent<Bloom>().active; }
        set
        {
            GetVolumeComponent<Fog>().active = value;
        }
    }

    public static bool AmbientOcclusion
    {
        get { return GetVolumeComponent<ScreenSpaceAmbientOcclusion>().active; }
        set
        {
            GetVolumeComponent<ScreenSpaceAmbientOcclusion>().active = value;
        }
    }

    public static T GetVolumeComponent<T>() where T : VolumeComponent
    {
        foreach (var component in GlobalVolume.profile.components)
        {
            if (component is T)
            {
                return component as T;
            }
        }
        return null;
    }

}