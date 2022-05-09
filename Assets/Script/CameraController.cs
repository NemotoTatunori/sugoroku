using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    Rigidbody m_rb = null;
    [SerializeField] float m_moveSpeed = 10f;
    [SerializeField] float m_turnSpeed = 10f;
    void Start()
    {
        m_rb = GetComponent<Rigidbody>();
    }
    void Update()
    {
        float v = Input.GetAxisRaw("Vertical");
        float h = Input.GetAxisRaw("Horizontal");
        float ud = 0;
        float x = transform.rotation.eulerAngles.x;
        float y = transform.rotation.eulerAngles.y;
        float dt = Time.deltaTime;
        if (Input.GetKey(KeyCode.Q) && x < 40)
        {
            ud = dt * m_turnSpeed;
        }
        if (Input.GetKey(KeyCode.E) && x > 10)
        {
            ud = dt * -m_turnSpeed;
        }
        transform.rotation = Quaternion.Euler(x + ud, y + dt * h * m_turnSpeed, 0);
        Vector3 velo = transform.forward.normalized * v * m_moveSpeed;
        velo.y = m_rb.velocity.y;
        m_rb.velocity = velo;
    }

    public void Point()
    {

    }
}
