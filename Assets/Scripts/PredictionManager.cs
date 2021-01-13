using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PredictionManager : MonoBehaviour
{
    public static PredictionManager Instance = null;

    public int maxIterations;

    Scene currentScene;
    Scene predictionScene;

    PhysicsScene2D currentPhysicsScene;
    PhysicsScene2D predictionPhysicsScene;

    public LineRenderer lineRenderer;
    GameObject dummy;
    Rigidbody2D dummyRBody;

    private void Awake()
    {
        /*
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else if (Instance != this)
        {
            Destroy(this.gameObject);
        }
        */
        Instance = this;
    }

    void Start()
    {
        Physics2D.simulationMode = SimulationMode2D.Script;

        currentScene = SceneManager.GetActiveScene();
        currentPhysicsScene = currentScene.GetPhysicsScene2D();

        CreateSceneParameters parameters = new CreateSceneParameters(LocalPhysicsMode.Physics2D);
        predictionScene = SceneManager.CreateScene("Prediction", parameters);
        predictionPhysicsScene = predictionScene.GetPhysicsScene2D();

        lineRenderer = GetComponent<LineRenderer>();
    }

    private void FixedUpdate()
    {
        //advance the normal Physics in standard time
        if (currentPhysicsScene.IsValid())
            currentPhysicsScene.Simulate(Time.fixedDeltaTime);
    }

    public void predict(GameObject subject, Vector2 currentPosition, Vector2 force)
    {
        if (currentPhysicsScene.IsValid() && predictionPhysicsScene.IsValid())
        {
            if (dummy == null)
            {
                dummy = Instantiate(subject);
                SceneManager.MoveGameObjectToScene(dummy, predictionScene);
                dummyRBody = dummy.GetComponent<Rigidbody2D>();
            }

            dummy.transform.position = currentPosition;
            dummyRBody.AddForce(force*1f);

            lineRenderer.positionCount = 0;
            lineRenderer.positionCount = maxIterations;

            for (int i=0; i<maxIterations; i++)
            {
                GravityManager.Instance.ApplyGravityTowardsAllPlanets(dummyRBody);
                //GravityManager.Instance.ApplyDrag(dummy.GetComponent<Rigidbody2D>());
                predictionPhysicsScene.Simulate(Time.fixedDeltaTime);
                lineRenderer.SetPosition(i, dummy.transform.position + new Vector3(0,0,-0.1f));
            }
            Destroy(dummy);
        }
    }
}
