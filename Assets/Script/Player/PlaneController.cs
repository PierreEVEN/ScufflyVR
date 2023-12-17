
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;


/// <summary>
/// Interface between the player keyboard and the current controlled plane
/// </summary>
[RequireComponent(typeof(PlayerController))]
public class PlaneController : MonoBehaviour
{
    /// <summary>
    /// The pitch input from keyboard act like a trim
    /// </summary>
    float pitchInput = 0;
    float pitchTrim = 0;

    /// <summary>
    /// Current keyboard input state
    /// </summary>
    float currentKeyboardThrottle = 0;
    float trimIncreaseInput = 0;
    float currentKeyboardYaw = 0;
    float currentKeyboardRoll = 0;

    bool enableInputs = false;

    private PlayerController playerController;

    void Start()
    {
        playerController = GetComponent<PlayerController>();
    }

    /// <summary>
    /// Are input enabled
    /// </summary>
    public bool EnableInputs
    {
        set
        {
            enableInputs = value;
        }
        get
        {
            return enableInputs && playerController.ControlledPlane;
        }
    }

    void Update()
    {
        if (!EnableInputs)
            return;

        // Update the pitch of the plane
        pitchTrim += trimIncreaseInput * Time.deltaTime * 1.5f;
        pitchTrim = Mathf.Clamp(pitchTrim, -1, 1);
        playerController.ControlledPlane.SetPitchInput(Mathf.Clamp(pitchInput + pitchTrim, -1, 1));
    }

    /**
     * Direct axis : joystick / analogic controls
     */

    public void OnAxisThrottle(InputValue input)
    {
        if (!EnableInputs)
            return;

        playerController.ControlledPlane.SetThrustInput(input.Get<float>() * 0.5f + 0.5f);
    }

    public void OnAxisYaw(InputValue input)
    {
        if (!EnableInputs)
            return;

        playerController.ControlledPlane.SetYawInput(input.Get<float>());
    }

    public void OnAxisRoll(InputValue input)
    {
        if (!EnableInputs)
            return;

        playerController.ControlledPlane.SetRollInput(input.Get<float>());
    }
    public void OnAxisPitch(InputValue input)
    {
        if (!EnableInputs)
            return;

        pitchInput = input.Get<float>();
    }

    /**
     * Button and keyboard axis
     */

    public void OnIncreaseThrottle(InputValue input)
    {
        currentKeyboardThrottle = Mathf.Clamp(currentKeyboardThrottle + input.Get<float>(), 0, 1);
        if (!EnableInputs)
            return;
        playerController.ControlledPlane.SetThrustInput(currentKeyboardThrottle);
    }

    public void OnSetPitch(InputValue input)
    {
        trimIncreaseInput = Mathf.Clamp(input.Get<float>(), -1, 1);
    }
    public void OnSetYaw(InputValue input)
    {
        currentKeyboardYaw = Mathf.Clamp(input.Get<float>(), -1, 1);
        if (!EnableInputs)
            return;
        playerController.ControlledPlane.SetYawInput(currentKeyboardYaw);
    }
    public void OnSetRoll(InputValue input)
    {
        currentKeyboardRoll = Mathf.Clamp(input.Get<float>(), -1, 1);
        if (!EnableInputs)
            return;
        playerController.ControlledPlane.SetRollInput(currentKeyboardRoll);
    }

    public void OnSwitchAPU()
    {
        if (!EnableInputs)
            return;
        playerController.ControlledPlane.EnableAPU = !playerController.ControlledPlane.EnableAPU;
    }

    public void OnSwitchPower()
    {
        if (!EnableInputs)
            return;
        playerController.ControlledPlane.MainPower = !playerController.ControlledPlane.MainPower;
    }

    public void OnSwitchThrottleNotch()
    {
        if (!EnableInputs)
            return;
        playerController.ControlledPlane.ThrottleNotch = !playerController.ControlledPlane.ThrottleNotch;
        playerController.ControlledPlane.SetThrustInput(currentKeyboardThrottle);
    }

    public void OnSwitchGear()
    {
        if (!EnableInputs)
            return;
        playerController.ControlledPlane.RetractGear = !playerController.ControlledPlane.RetractGear;
    }

    public void OnSwitchBrakes()
    {
        if (!EnableInputs)
            return;
        playerController.ControlledPlane.ParkingBrakes = !playerController.ControlledPlane.ParkingBrakes;
    }

    public void OnSetBrake(InputValue input)
    {
        if (!EnableInputs)
            return;
        playerController.ControlledPlane.ParkingBrakes = input.isPressed;
    }

    public void OnShoot()
    {
        if (!EnableInputs)
            return;

        WeaponManager weaponManager = playerController.ControlledPlane.GetComponent<WeaponManager>();
        if (!weaponManager)
            return;

        weaponManager.BeginShoot();
    }
    public void OnEndShoot()
    {
        WeaponManager weaponManager = playerController.ControlledPlane.GetComponent<WeaponManager>();
        if (!weaponManager)
            return;

        weaponManager.EndShoot();
    }

    public void OnSwitchCanon()
    {
        if (!EnableInputs)
            return;

        WeaponManager weaponManager = playerController.ControlledPlane.GetComponent<WeaponManager>();
        if (!weaponManager)
            return;

        weaponManager.SwitchToCanon();
    }
    public void OnSwitchAirGround()
    {
        if (!EnableInputs)
            return;

        WeaponManager weaponManager = playerController.ControlledPlane.GetComponent<WeaponManager>();
        if (!weaponManager)
            return;

        weaponManager.AirGroundMode();
    }

    public void OnSwitchAirAir()
    {
        if (!EnableInputs)
            return;

        WeaponManager weaponManager = playerController.ControlledPlane.GetComponent<WeaponManager>();
        if (!weaponManager)
            return;

        weaponManager.AirAirMode();
    }

    public void OnEnableWeapons()
    {
        if (!EnableInputs)
            return;

        WeaponManager weaponManager = playerController.ControlledPlane.GetComponent<WeaponManager>();
        if (!weaponManager)
            return;

        weaponManager.IsToggledOn = !weaponManager.IsToggledOn;
    }

    public void OnSwitchCanopy()
    {
        if (!EnableInputs)
            return;
        playerController.ControlledPlane.OpenCanopy = !playerController.ControlledPlane.OpenCanopy;
    }

    public void OnBrakes(InputValue input)
    {
        if (!EnableInputs)
            return;
        playerController.ControlledPlane.Brakes = input.Get<float>() > 0.5f;
    }
}