using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlochSphere : MonoBehaviour{

    [SerializeField]
    private Material lineMaterial;
    private int numLines;

    [SerializeField]
    private List<Sphere> spheresToCreate;

    [System.Serializable]
    public struct Sphere{
        public float radius;
        public Vector3 position;
        public int numLongRings;
        public int numLatRings;
    }

    // Start is called before the first frame update
    void Start(){
        numLines = 1;
        for(int i = 0; i < spheresToCreate.Count; i ++){
            Sphere sphere = spheresToCreate[i];
            DrawSphere(sphere.position, sphere.radius, sphere.numLongRings, sphere.numLatRings);
        }
    }

    // Update is called once per frame
    void Update(){
        
    }

    void DrawSphere(Vector3 position, float radius, int numLongRings, int numLatRings){

        float latStep = (2 * radius) / (numLatRings + 1);
        float longStep = 180 / numLongRings;

        for(float height = -radius + latStep; height < radius; height += latStep){
            Vector3 centerPos = new Vector3(position.x, position.y + height, position.z);
            float theta = Mathf.Asin(Mathf.Abs(height)/radius);
            float minorRadius = radius * Mathf.Cos(theta);
            DrawPolygon(50, minorRadius, centerPos, 0.05f, 90, 0);
        }

        for(float angle = 0; angle < 360; angle += longStep){
            DrawPolygon(50, radius, position, 0.05f, 0, angle);
        }
    }

    void DrawPolygon(int vertexNumber, float radius, Vector3 centerPos, float width, float tilt, float spin){
        GameObject child = CreateLineObject();
        LineRenderer lineRenderer = child.AddComponent<LineRenderer>();
        lineRenderer.material = lineMaterial;
        lineRenderer.startWidth = width;
        lineRenderer.endWidth = width;
        lineRenderer.loop = true;
        float angle = 2 * Mathf.PI / vertexNumber;
        lineRenderer.positionCount = vertexNumber;

        float tiltRad = (Mathf.PI/180) * tilt;
        Matrix4x4 tiltMatrix = new Matrix4x4(new Vector4(1, 0, 0, 0),
                                             new Vector4(0, Mathf.Cos(tiltRad), -Mathf.Sin(tiltRad), 0),
                                             new Vector4(0, Mathf.Sin(tiltRad), Mathf.Cos(tiltRad), 0),
                                             new Vector4(0, 0, 0, 1));

        float spinRad = (Mathf.PI/180) * spin;
        Matrix4x4 spinMatrix = new Matrix4x4(new Vector4(Mathf.Cos(spinRad), 0, Mathf.Sin(spinRad), 0),
                                             new Vector4(0, 1, 0, 0),
                                             new Vector4(-Mathf.Sin(spinRad), 0, Mathf.Cos(spinRad), 0),
                                             new Vector4(0, 0, 0, 1));

        for (int i = 0; i < vertexNumber; i++){

            Matrix4x4 rotationMatrix = new Matrix4x4(new Vector4(Mathf.Cos(angle * i), Mathf.Sin(angle * i), 0, 0),
                                                     new Vector4(-Mathf.Sin(angle * i), Mathf.Cos(angle * i), 0, 0),
                                                     new Vector4(0, 0, 1, 0),
                                                     new Vector4(0, 0, 0, 1));
            Matrix4x4 finalRotation = tiltMatrix * spinMatrix * rotationMatrix;
            Vector3 initialRelativePosition = new Vector3(0, radius, 0);
            Vector3 nextPosition = finalRotation.MultiplyPoint(initialRelativePosition);
            lineRenderer.SetPosition(i, centerPos + nextPosition);
        }
    }

    GameObject CreateLineObject(){
        GameObject childObject = new GameObject("Line " + numLines);
        childObject.transform.SetParent(gameObject.transform);
        numLines++;
        return childObject;
    }
}
