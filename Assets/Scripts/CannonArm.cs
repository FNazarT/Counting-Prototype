using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CannonArm : MonoBehaviour
{
    [SerializeField] private GameObject cannonBallPrefab;
    [SerializeField] private Slider slider;    
    [SerializeField] private Transform shootingPoint;
    [SerializeField] private float changeTime;
    private float rotation, rotationSpeed = -40f, verticalInput;
    private int changeRate = 1;

    void Start()
    {
        slider.minValue = 10f;
        slider.maxValue = 25f;
        StartCoroutine(nameof(SliderValueChange));
    }

    void Update()
    {
        //Rotate cannon based on user vertical input and clamp the angle of rotation between -90 and 0
        verticalInput = Input.GetAxis("Vertical");

        rotation += rotationSpeed * Time.deltaTime * verticalInput;
        rotation = Mathf.Clamp(rotation, -90f, 0f);

        transform.eulerAngles = new Vector3(rotation, transform.eulerAngles.y, transform.eulerAngles.z);

        //Instantiate a cannon ball when the user presses the space key and apply an instant force
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameObject cannonBall = Instantiate(cannonBallPrefab);
            cannonBall.transform.position = shootingPoint.position;

            Rigidbody cannonBallRb = cannonBall.GetComponent<Rigidbody>();
            cannonBallRb.AddRelativeForce(transform.forward * slider.value, ForceMode.Impulse);
        }
    }

    IEnumerator SliderValueChange()
    {
        while (true)
        {
            slider.value += changeRate;
            yield return new WaitForSeconds(changeTime);

            if (slider.value == slider.maxValue)
            {
                changeRate = -1;
            }
            else if (slider.value == slider.minValue)
            {
                changeRate = 1;
            }
        }
    }
}
