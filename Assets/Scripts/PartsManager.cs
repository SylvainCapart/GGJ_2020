using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class PartsManager : MonoBehaviour
{
    public enum SpotIndex { HEAD, ARMLEFT, ARMRIGHT, LEGLEFT, LEGRIGHT };

    [System.Serializable]
    public struct Spot
    {
        public Transform tr;
        public bool isTaken;
    }

    public Spot[] spots;
    public bool[] spotsTaken;

    private List<Interactable> itemDetectedList = new List<Interactable>();
    public Interactable m_currentInteractable;
    public Transform m_AttachQuestion;
    public bool m_Rpressed = false;
    public int m_selectedIndex = 0;
    public SpotIndex m_SelectedSpot;
    public float m_offset;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Interactable newNearestInteractable = null;
        newNearestInteractable = Detect();

        if (m_currentInteractable != newNearestInteractable)
        {
            m_currentInteractable = newNearestInteractable;
        }

        DisplayBind();
        m_SelectedSpot = (SpotIndex)m_selectedIndex;

    }

    public void DisplayBind()
    {
        if (m_currentInteractable == null)
        {
            DisplayUI(false);
            DisplayAttach(false);
            return;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            m_Rpressed = true;
            m_selectedIndex = (m_selectedIndex + 1) % spots.Length ;
        }

        Transform nearestSpot = GetNearestSpot(m_currentInteractable);
        Vector3 spotPos = nearestSpot.transform.position;
        if (!m_Rpressed)
        {
            //
        }
        else
        {
            spotPos = spots[m_selectedIndex].tr.transform.position;
        }

        DrawLine(spotPos, m_currentInteractable.transform.position, Color.cyan);

        DisplayAttach(true);
        DisplayUI(true);

        if (Input.GetKeyDown(KeyCode.E))
        {
            Attach(m_currentInteractable);
        }

    }

    public void DisplayUI(bool state)
    {
        m_AttachQuestion.gameObject.SetActive(state);
        m_AttachQuestion.transform.rotation = Quaternion.Euler(0, 0, 0);
    }


    void DisplayAttach(bool state)
    {
        foreach (var spot in spots)
        {
            SpriteRenderer rend = spot.tr.GetComponent<SpriteRenderer>();
            if (rend != null)
            {
                rend.enabled = state;
            }
        }
    }

    void DrawLine(Vector3 start, Vector3 end, Color color, float duration = 0.1f)
    {
        GameObject m_line = new GameObject();
        m_line.transform.position = start;
        m_line.AddComponent<LineRenderer>();
 
        LineRenderer lr = m_line.GetComponent<LineRenderer>();
        lr.material = new Material(Shader.Find("Unlit/NewUnlitShader"));
        lr.startColor = color;
        lr.endColor = color;
        lr.startWidth = 0.2f;
        lr.endWidth = 0.2f;
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
        lr.sortingLayerName = "Default";
        lr.sortingOrder = 5;

        GameObject.Destroy(m_line, duration);
    }

    public Transform GetNearestSpot(Interactable item)
    {
        Transform nearestSpot = null;
        float minDistance = 10.0f;

        for (int i = 0; i < spots.Length; i++)
        {
            float distance = Vector2.Distance(item.transform.position, spots[i].tr.transform.position);
            if (distance < minDistance)
            {
                nearestSpot = spots[i].tr;
                minDistance = distance;
                if (!m_Rpressed)
                m_selectedIndex = i;
            }
        }

        //nearestSpot = spots[(int)m_selectedIndex].transform;

        return nearestSpot;
    }

    public void Attach(Interactable item)
    {
        m_Rpressed = false;

        Rigidbody2D itemRB = item.GetComponent<Rigidbody2D>();

        if (itemRB != null)
        {
            HingeJoint2D hingejoint = gameObject.AddComponent<HingeJoint2D>();
            hingejoint.breakForce = 1000;
            hingejoint.breakTorque = 1000;
            hingejoint.connectedBody = itemRB;

            Vector3 vectorBind = spots[m_selectedIndex].tr.transform.position - item.transform.position;
            Vector3 vectorAttach = item.attach.position - item.transform.position;
            float attachDiff = Vector2.Distance(item.attach.position, item.transform.position);
            float angle = Vector2.SignedAngle(vectorBind, vectorAttach);


            itemRB.constraints = RigidbodyConstraints2D.FreezeRotation;
            item.transform.Rotate(0, 0, -angle);
            item.transform.position = spots[m_selectedIndex].tr.transform.position - vectorBind.normalized * (attachDiff + m_offset);


        }
    }

    public Interactable Detect()
    {
        float minDistance = 10.0f;
        Interactable nearestInteractable = null;

        for (int i = 0; i < itemDetectedList.Count; i++)
        {
            float distance = Vector2.Distance(itemDetectedList[i].transform.position, transform.position);
            if (distance < minDistance)
            {
                nearestInteractable = itemDetectedList[i];
                minDistance = distance;

            }
        }

        return nearestInteractable;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var item = collision.GetComponent<Interactable>();

        if (item != null && !itemDetectedList.Contains(item))
        {
            itemDetectedList.Add(item);
            m_Rpressed = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        var item = collision.GetComponent<Interactable>();

        if (item != null && itemDetectedList.Contains(item))
        {
            itemDetectedList.Remove(item);
            m_Rpressed = false;
        }
    }


}
