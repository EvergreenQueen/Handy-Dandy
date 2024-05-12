//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.7.0
//     from Assets/Scripts/PlayerController/PlayerActionControls.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @PlayerActionControls: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerActionControls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerActionControls"",
    ""maps"": [
        {
            ""name"": ""Movement"",
            ""id"": ""123427b8-5761-4da3-a7b8-d7476b8c4fbf"",
            ""actions"": [
                {
                    ""name"": ""WASD"",
                    ""type"": ""Value"",
                    ""id"": ""1198d504-4e23-4d33-8187-48406c7970f9"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""LookAround"",
                    ""type"": ""PassThrough"",
                    ""id"": ""1918738b-c549-484c-8bf6-94ea1c3f32e2"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Click"",
                    ""type"": ""Button"",
                    ""id"": ""cdf95831-f0a9-409a-99e0-662de137eaf9"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Sprint"",
                    ""type"": ""Button"",
                    ""id"": ""c11ed11c-ec15-455a-8378-06484a6f35be"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Jump"",
                    ""type"": ""Button"",
                    ""id"": ""78006872-b4d1-42c2-8b69-9e9952dd1c7b"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""LClick"",
                    ""type"": ""Button"",
                    ""id"": ""3beebd90-10c9-42bb-b844-77fa0b9ec5da"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""RClick"",
                    ""type"": ""Button"",
                    ""id"": ""82a44133-3b87-4c43-bcd1-e6527194a947"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""2D Axis"",
                    ""id"": ""3ddbfe47-fa11-4366-af01-1b60c0a89716"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""WASD"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""ebb8da85-62b2-4efd-82bb-94169d3afc0b"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Movement"",
                    ""action"": ""WASD"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""4829a75a-60dd-4ffe-9cfb-eb8be58220cb"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Movement"",
                    ""action"": ""WASD"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""089776d8-cd88-4688-82b5-40bbaa7a532a"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Movement"",
                    ""action"": ""WASD"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""081d1599-f1a3-4e40-8239-a2762144e8de"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Movement"",
                    ""action"": ""WASD"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""2d922e9a-2b46-4ec8-9e8c-ea4750cec0d2"",
                    ""path"": ""<Mouse>/delta"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Movement"",
                    ""action"": ""LookAround"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e7e631d0-bfe6-4548-877c-e9c1bfa9528d"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Click"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0376aa46-8aa7-4e01-86c6-c00898a1c418"",
                    ""path"": ""<Keyboard>/leftShift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Sprint"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3e4982aa-a879-4bc1-b586-2ed1d0ca382c"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9a37e2a5-06f8-48bf-a167-aadbf145888d"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""LClick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""55fe809b-493d-4e1b-b41e-203eda9834b2"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""RClick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Movement"",
            ""bindingGroup"": ""Movement"",
            ""devices"": []
        }
    ]
}");
        // Movement
        m_Movement = asset.FindActionMap("Movement", throwIfNotFound: true);
        m_Movement_WASD = m_Movement.FindAction("WASD", throwIfNotFound: true);
        m_Movement_LookAround = m_Movement.FindAction("LookAround", throwIfNotFound: true);
        m_Movement_Click = m_Movement.FindAction("Click", throwIfNotFound: true);
        m_Movement_Sprint = m_Movement.FindAction("Sprint", throwIfNotFound: true);
        m_Movement_Jump = m_Movement.FindAction("Jump", throwIfNotFound: true);
        m_Movement_LClick = m_Movement.FindAction("LClick", throwIfNotFound: true);
        m_Movement_RClick = m_Movement.FindAction("RClick", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }

    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // Movement
    private readonly InputActionMap m_Movement;
    private List<IMovementActions> m_MovementActionsCallbackInterfaces = new List<IMovementActions>();
    private readonly InputAction m_Movement_WASD;
    private readonly InputAction m_Movement_LookAround;
    private readonly InputAction m_Movement_Click;
    private readonly InputAction m_Movement_Sprint;
    private readonly InputAction m_Movement_Jump;
    private readonly InputAction m_Movement_LClick;
    private readonly InputAction m_Movement_RClick;
    public struct MovementActions
    {
        private @PlayerActionControls m_Wrapper;
        public MovementActions(@PlayerActionControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @WASD => m_Wrapper.m_Movement_WASD;
        public InputAction @LookAround => m_Wrapper.m_Movement_LookAround;
        public InputAction @Click => m_Wrapper.m_Movement_Click;
        public InputAction @Sprint => m_Wrapper.m_Movement_Sprint;
        public InputAction @Jump => m_Wrapper.m_Movement_Jump;
        public InputAction @LClick => m_Wrapper.m_Movement_LClick;
        public InputAction @RClick => m_Wrapper.m_Movement_RClick;
        public InputActionMap Get() { return m_Wrapper.m_Movement; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(MovementActions set) { return set.Get(); }
        public void AddCallbacks(IMovementActions instance)
        {
            if (instance == null || m_Wrapper.m_MovementActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_MovementActionsCallbackInterfaces.Add(instance);
            @WASD.started += instance.OnWASD;
            @WASD.performed += instance.OnWASD;
            @WASD.canceled += instance.OnWASD;
            @LookAround.started += instance.OnLookAround;
            @LookAround.performed += instance.OnLookAround;
            @LookAround.canceled += instance.OnLookAround;
            @Click.started += instance.OnClick;
            @Click.performed += instance.OnClick;
            @Click.canceled += instance.OnClick;
            @Sprint.started += instance.OnSprint;
            @Sprint.performed += instance.OnSprint;
            @Sprint.canceled += instance.OnSprint;
            @Jump.started += instance.OnJump;
            @Jump.performed += instance.OnJump;
            @Jump.canceled += instance.OnJump;
            @LClick.started += instance.OnLClick;
            @LClick.performed += instance.OnLClick;
            @LClick.canceled += instance.OnLClick;
            @RClick.started += instance.OnRClick;
            @RClick.performed += instance.OnRClick;
            @RClick.canceled += instance.OnRClick;
        }

        private void UnregisterCallbacks(IMovementActions instance)
        {
            @WASD.started -= instance.OnWASD;
            @WASD.performed -= instance.OnWASD;
            @WASD.canceled -= instance.OnWASD;
            @LookAround.started -= instance.OnLookAround;
            @LookAround.performed -= instance.OnLookAround;
            @LookAround.canceled -= instance.OnLookAround;
            @Click.started -= instance.OnClick;
            @Click.performed -= instance.OnClick;
            @Click.canceled -= instance.OnClick;
            @Sprint.started -= instance.OnSprint;
            @Sprint.performed -= instance.OnSprint;
            @Sprint.canceled -= instance.OnSprint;
            @Jump.started -= instance.OnJump;
            @Jump.performed -= instance.OnJump;
            @Jump.canceled -= instance.OnJump;
            @LClick.started -= instance.OnLClick;
            @LClick.performed -= instance.OnLClick;
            @LClick.canceled -= instance.OnLClick;
            @RClick.started -= instance.OnRClick;
            @RClick.performed -= instance.OnRClick;
            @RClick.canceled -= instance.OnRClick;
        }

        public void RemoveCallbacks(IMovementActions instance)
        {
            if (m_Wrapper.m_MovementActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IMovementActions instance)
        {
            foreach (var item in m_Wrapper.m_MovementActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_MovementActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public MovementActions @Movement => new MovementActions(this);
    private int m_MovementSchemeIndex = -1;
    public InputControlScheme MovementScheme
    {
        get
        {
            if (m_MovementSchemeIndex == -1) m_MovementSchemeIndex = asset.FindControlSchemeIndex("Movement");
            return asset.controlSchemes[m_MovementSchemeIndex];
        }
    }
    public interface IMovementActions
    {
        void OnWASD(InputAction.CallbackContext context);
        void OnLookAround(InputAction.CallbackContext context);
        void OnClick(InputAction.CallbackContext context);
        void OnSprint(InputAction.CallbackContext context);
        void OnJump(InputAction.CallbackContext context);
        void OnLClick(InputAction.CallbackContext context);
        void OnRClick(InputAction.CallbackContext context);
    }
}
