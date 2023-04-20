using System.Collections;
using UnityEngine;
using Zenject;

public class Movement : MonoBehaviour
{
    public float Acceleration;
    public float CurrentGear { get { return _gear.Current; } }
    public bool MoveBackward { get { return _gear.Current < 0; } }

    [SerializeField] private Transform _connectPoint;
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private float _power;
    [SerializeField] private float _weight;

    private Gear _gear;
    private Rudder _rudder;
    private Rigidbody2D _rb;

    private MyButton _gasButton;
    private MyButton _breakButton;

    //Acceleration process variables
    private float _accelerationGap = 0.15f;
    private bool _gasPressed;
    private bool _breakPressed;

    [SerializeField] private Transform _leftWheel;
    [SerializeField] private Transform _rightWheel;

    [Inject]
    public void Initialize(Rudder rudder, Gear gear,[Inject(Id = "gas")]MyButton gasButton, [Inject(Id = "break")] MyButton breakButton)
    {
        _rudder = rudder;
        _gear = gear;
        _gasButton = gasButton;
        _breakButton = breakButton;
    }

    private void Awake()
    {     
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        StartCoroutine(AccelerationProcess());

        _gasButton.onDown.AddListener( () => _gasPressed = true);
        _gasButton.onUp.AddListener(() => _gasPressed = false);
        _breakButton.onDown.AddListener(() => _breakPressed = true);
        _breakButton.onUp.AddListener(() => _breakPressed = false);
    }

    private void FixedUpdate()
    {
        var gearRatio =  _gear.GetRatio();
        if (_rb.velocity.magnitude > 0.05f) 
            transform.RotateAround( _connectPoint.position,
                                    Vector3.forward,
                                    Mathf.Sign(gearRatio) * CalculateRotationAngle() * Time.fixedDeltaTime);
        
        _rb.velocity = Vector2.Lerp(_rb.velocity,
                                    CalculateVelocity() * gearRatio  * Time.fixedDeltaTime,
                                    0.10f);

        RotateWheels();
    }

    private float CalculateRotationAngle()
    {
        var sensetivity = PlayerPrefs.GetFloat("sensetivity");
        var velocityMagnitude = _rb.velocity.magnitude;
        var rotation = (_rotationSpeed + sensetivity) * _rudder.GetHorizontal;
        var rotationInfluenceCoefficient = Mathf.Min(velocityMagnitude * 3f, 1);
        return rotationInfluenceCoefficient * rotation;
    }

    private Vector2 CalculateVelocity()
    {
        var powerOfMovement = _power - _weight;
        return Acceleration * powerOfMovement * transform.up;
    }

    private void RotateWheels()
    {
        _leftWheel.localRotation = Quaternion.Euler(0f, 0f, _rudder.GetHorizontal * 45f);
        _rightWheel.localRotation = Quaternion.Euler(0f, 0f, _rudder.GetHorizontal * 45f);
    }


    IEnumerator AccelerationProcess()
    {
        var multiplicator = _power / 100 * 0.05f;
        for(; ; )
        {
            if (_gasPressed)
            {
                if(Acceleration < 1f)
                {
                    Acceleration += multiplicator;
                }
            }
            else
            {
                if(Acceleration > 0f)
                {
                    if (_breakPressed)
                    {
                        Acceleration = Mathf.Max(0f, Acceleration - 3 * multiplicator);
                    }
                    else
                        Acceleration = Mathf.Max(0f, Acceleration - multiplicator);
                }
            }
            yield return new WaitForSeconds(_accelerationGap);
        }
    }
}
