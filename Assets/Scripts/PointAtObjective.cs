using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointAtObjective : MonoBehaviour
{
    private ObjectivePoint _pointAt;
    private Player _player;
    // Start is called before the first frame update
    void Start()
    {
        _pointAt = FindObjectOfType<ObjectivePoint>();
        _player = FindObjectOfType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 vectorToTarget = _pointAt.transform.position - _player.transform.position;
        float angle = (Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg) - 90;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * 1);
    }
}
