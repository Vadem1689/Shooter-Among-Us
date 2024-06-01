using UnityEngine;

public class DeathLine : MonoBehaviour
{

    [SerializeField] private int lengthOfLineRenderer = 20;
   
    [SerializeField] private float initialRadius = 35f, currentRadius, shrinkTime = 120, damageAmount = 10f, damagePeriod = 0.5f, widthMultiplier = 5f;

    [SerializeField] private LineRenderer lineRenderer;

    private float timeFromLatestDamage = 0;
    
    public float GetDamageRadius()
    {
        return currentRadius-widthMultiplier/2;
    }

    public float GetDamageAmount()
    {
        return damageAmount;
    }

    void Start()
    {
        currentRadius = initialRadius;
        lineRenderer.positionCount = lengthOfLineRenderer;
        lineRenderer.loop = true;
        lineRenderer.widthMultiplier = widthMultiplier;
    }

    void Update()
    {
        currentRadius = currentRadius - initialRadius / shrinkTime * Time.deltaTime;
        var points = new Vector3[lengthOfLineRenderer];
        for (int i = 0; i < lengthOfLineRenderer; i++)
        {
            points[i] = new Vector3(-Mathf.Cos(2*Mathf.PI*i/lengthOfLineRenderer)*currentRadius, Mathf.Sin(2*Mathf.PI*i/lengthOfLineRenderer)*currentRadius, 0.0f);
        }
        lineRenderer.SetPositions(points);

        timeFromLatestDamage += Time.deltaTime;
        
        if (timeFromLatestDamage >= damagePeriod)
        {
            GameController.Instance.CheckDeathLine();
            Debug.Log("DeathLineCheck");
            timeFromLatestDamage = 0;
        }
    }
}
