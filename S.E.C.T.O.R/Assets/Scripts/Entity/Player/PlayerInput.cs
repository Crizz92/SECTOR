using UnityEngine;
using System;
using System.Collections;

[Serializable]
public class PlayerInput : MonoBehaviour {

    public enum PlayerIndex
    {
        P1,
        P2,
        PC,
    };

    public PlayerIndex _playerIndex = PlayerIndex.P1;
    private string _prefix = "";

    public InputContainer _PADInputContainer;
    public InputContainer _PCInputContainer;
    private InputContainer _currentInputUsed;
    private bool _anInputWasPressed = false;
    public bool AnInputWasPressed
    {
        get { return AnyInputPressed(); }
    }
    public bool ActionButtonWasPressed
    {
        get { return ActionButtonPressed(); }
    }

    #region GAMEPAD VAR


    #region RTLT Manager
    private bool _rtPressed = false;
    private bool _rtPressedOnce = false;
    private bool _ltPressed = false;
    private bool _ltPressedOnce = false;
    #endregion

    #region Horizontal Arrow Manager
    private bool _leftArrowPressed = false;
    private bool _leftArrowPressedOnce = false;
    private bool _rightArrowPressed = false;
    private bool _rightArrowPressedOnce = false;
    #endregion

    #region Vertical Arrow Manager
    private bool _topArrowPressed = false;
    private bool _topArrowPressedOnce = false;
    private bool _bottomArrowPressed = false;
    private bool _bottomArrowPressedOnce = false;
    #endregion
    #endregion
    #region PC VAR

    #endregion

    #region Global
    public bool _enable = true;
    #endregion  
    // Use this for initialization
    void Start () {
        SetupPrefix();
        SetupInput();
	}

	public void Initialize()
    {
        SetupPrefix();
        SetupInput();
    }
	// Update is called once per frame
	void LateUpdate () {
        _anInputWasPressed = false;
        if (_enable)
        {
            RTLTChecker();
            HorizontalArrowChecker();
            VerticalArrowChecker();
            if (_anInputWasPressed) Debug.Log("An input was pressed");
        }

	}
    private bool AnyInputPressed()
    {
        if (ActionButtonPressed()) return true;
        if (LTDown()) { Debug.Log("LT"); return true;                             }
        if (ButtonLB()) { Debug.Log("LB"); return true;                           }
        if (RTDown()) { Debug.Log("RT"); return true;                             }
        if (ButtonRB()) { Debug.Log("RB"); return true;                           }
        if (BottomArrowDown()) { Debug.Log("ArrowDown"); return true;             }
        if (RightArrowDown()) { Debug.Log("ArrowRight"); return true;             }
        if (LeftArrowDown()) { Debug.Log("ArrowLeft"); return true;               }
        if (TopArrowDown()) { Debug.Log("ArrowTop"); return true;                 }
        if (LeftJoystick() != Vector2.zero) { Debug.Log("LeftJoy"); return true;  }
        if (RightJoystick() != Vector2.zero) { Debug.Log("RightJoy"); return true;}
        if (ButtonBack()) { Debug.Log("Back"); return true; }
        return false;
    }

    private bool ActionButtonPressed()
    {
        if (ButtonA()) return true;
        if (ButtonB()) return true;
        if (ButtonY()) return true;
        if (ButtonX()) return true;
        return false;
    }
    private void SetupPrefix()
    {
        if(_playerIndex != PlayerIndex.PC)
        {
            _prefix = _playerIndex.ToString();
        }
        else
        {
            _prefix = "";
        }
    }
    private void SetupInput()
    {
        if(_playerIndex == PlayerIndex.PC)
        {
            _currentInputUsed = _PCInputContainer;
        }
        else
        {
            _currentInputUsed = _PADInputContainer;
        }
    }

    #region FUNCTIONS
    public bool ButtonA()
    {
        if (_enable)
        {
            bool pressed = Input.GetButtonDown(_prefix + _currentInputUsed._action1);
            return pressed;
        }
        else
            return false;
    }
    public bool ButtonB()
    {
        if (_enable)
        {
            bool pressed = Input.GetButtonDown(_prefix + _currentInputUsed._action2);
            return pressed;
        }
        else
            return false;
    }
    public bool ButtonX()
    {
        if (_enable)
        {
            bool pressed = Input.GetButtonDown(_prefix + _currentInputUsed._action3);
            return pressed;
        }
        else
            return false;
    }
    public bool ButtonY()
    {
        if (_enable)
        {
            bool pressed = Input.GetButtonDown(_prefix + _currentInputUsed._action4);
            return pressed;
        }
        else
            return false;
    }
    public bool ButtonStart()
    {
        if (_enable)
        {
            bool pressed = Input.GetButtonDown(_prefix + _currentInputUsed._pause);
            return pressed;
        }
        else
            return false;
    }
    public bool ButtonBack()
    {
        if (_enable)
        {
            bool pressed =  Input.GetButtonDown(_prefix + _currentInputUsed._esc);
            return pressed;
        }
        else
            return false;
    }

    #region RBLB Functions
    public bool ButtonLB()
    {
        if (_enable)
        {
            bool pressed = Input.GetButtonDown(_prefix + _currentInputUsed._action7);
            return pressed;
        }
        else
            return false;
    }
    public bool ButtonRB()
    {
        if (_enable)
        {
            bool pressed = Input.GetButtonDown(_prefix + _currentInputUsed._action8);
            return pressed;
        }
        else
            return false;
    }
    #endregion
    #region RTLT Functions

    void RTLTChecker()
    {
		float lt = Input.GetAxis(_prefix + _currentInputUsed._action5);
		float rt = Input.GetAxis (_prefix + _currentInputUsed._action6);
        if (lt > 0.1f)
        {
            if (_ltPressed)
            {
                _ltPressedOnce = false;
            }
            else
            {
                _ltPressed = true;
                _ltPressedOnce = true;
            }
        }
        else if (rt > 0.1f)
        {
            if (_rtPressed)
            {
                _rtPressedOnce = false;
            }
            else
            {
                _rtPressed = true;
                _rtPressedOnce = true;
            }
        }
        else
        {
            _rtPressed = false;
            _rtPressedOnce = false;
            _ltPressed = false;
            _ltPressedOnce = false;
        }
    }

    public bool RTDown()
    {
        return _rtPressedOnce;
    }
    public bool RTStay()
    {
        return _rtPressed;
    }
    public bool LTDown()
    {
        return _ltPressedOnce;
    }
    public bool LTStay()
    {
        return _ltPressed;
    }
    #endregion

    #region Arrow Functions
    void HorizontalArrowChecker()
    {
        
        float horizontalArrow = Input.GetAxis(_prefix + _currentInputUsed._horizontalArrow);
        if (horizontalArrow < -0.5f)
        {
            if (_leftArrowPressed)
            {
                _leftArrowPressedOnce = false;
            }
            else
            {
                _leftArrowPressed = true;
                _leftArrowPressedOnce = true;
            }
        }
        else if (horizontalArrow > 0.5f)
        {
            if (_rightArrowPressed)
            {
                _rightArrowPressedOnce = false;
            }
            else
            {
                _rightArrowPressed = true;
                _rightArrowPressedOnce = true;
            }
        }
        else
        {
            _leftArrowPressed = false;
            _leftArrowPressedOnce = false;
            _rightArrowPressed = false;
            _rightArrowPressedOnce = false;
        }
    }
    void VerticalArrowChecker()
    {
        float verticalArrow = Input.GetAxis(_prefix + _currentInputUsed._verticalArrow);

        if (verticalArrow < -0.5f)
        {
            if (_topArrowPressed)
            {
                _topArrowPressedOnce = false;
            }
            else
            {
                _topArrowPressed = true;
                _topArrowPressedOnce = true;
            }
        }
        else if (verticalArrow > 0.5f)
        {
            if (_bottomArrowPressed)
            {
                _bottomArrowPressedOnce = false;
            }
            else
            {
                _bottomArrowPressed = true;
                _bottomArrowPressedOnce = true;
            }
        }
        else
        {
            _topArrowPressed = false;
            _topArrowPressedOnce = false;
            _bottomArrowPressed = false;
            _bottomArrowPressedOnce = false;
        }
    }
    public bool LeftArrowDown()
    {
        return _leftArrowPressedOnce;
    }
    public bool LeftArrowStay()
    {
        return _leftArrowPressed;
    }
    public bool RightArrowDown()
    {
        return _rightArrowPressedOnce;
    }
    public bool RightArrowStay()
    {
        return _rightArrowPressed;
    }
    public bool TopArrowDown()
    {
        return _topArrowPressedOnce;
    }
    public bool TopArrowStay()
    {
        return _topArrowPressed;
    }
    public bool BottomArrowDown()
    {
        return _bottomArrowPressedOnce;
    }
    public bool BottomArrowStay()
    {
        return _bottomArrowPressed;
    }
    public Vector2 LeftJoystick()
    {
        if (_enable)
        {
            float lhJoystick = Mathf.Clamp(Input.GetAxis(_prefix + _currentInputUsed._horizontal), -0.9f, 0.9f);
            float lvJoystick = Mathf.Clamp(Input.GetAxis(_prefix + _currentInputUsed._vertical), -0.9f, 0.9f);
            return new Vector3(lhJoystick, lvJoystick);
        }
        else
            return Vector3.zero;
    }
    public Vector2 RightJoystick()
    {
        if (_enable)
        {
            float rhJoystick = Mathf.Clamp(Input.GetAxis(_prefix + _currentInputUsed._horizontalSec), -0.9f, 0.9f);
            float rvJoystick = Mathf.Clamp(Input.GetAxis(_prefix + _currentInputUsed._verticalSec), -0.9f, 0.9f);
            return new Vector3(rhJoystick, rvJoystick);
        }
        else
            return Vector3.zero;
    }
    public bool LeftJoystickDown()
    {
        if (_enable)
        {
            bool pressed = Input.GetButtonDown(_prefix + _currentInputUsed._leftJoystickClick);
            return pressed;
        }
        else
            return false;
    }
    public bool RightJoystickDown()
    {
        if (_enable)
        {
            bool pressed = Input.GetButtonDown(_prefix + _currentInputUsed._rightJoystickClick);
            return pressed;
        }
        else
            return false;
    }
    #endregion
    #endregion
}
