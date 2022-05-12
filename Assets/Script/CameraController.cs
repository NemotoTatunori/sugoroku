using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    Rigidbody m_rb = null;
    bool m_move = true;
    [SerializeField] float m_moveSpeed = 10f;
    [SerializeField] float m_turnSpeed = 10f;
    public bool Move
    {
        get => m_move;
        set
        {
            m_move = value;
        }
    }
    void Start()
    {
        m_rb = GetComponent<Rigidbody>();
    }
    void Update()
    {
        if (m_move)
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
        else
        {
            m_rb.velocity = Vector3.zero;
        }
    }

    public void PositionSet(Vector3 p)
    {
        float x = p.x;
        float z = p.z;
        transform.position = new Vector3(x + 20, 10, z);
        transform.rotation = Quaternion.Euler(30, -90, 0);
    }
}
