using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PartsManager : MonoBehaviour
{
    public enum SpotIndex { HEAD, ARMLEFT, ARMRIGHT, LEGLEFT, LEGRIGHT };

    public Transform[] spots;

    private List<Interactable> itemDetectedList = new List<Interactable>();
    private Interactable m_currentInteractable;
    public Transform m_AttachQuestion; 
    
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

    }

    public void DisplayBind()
    {
        if (m_currentInteractable == null)
        {
            DisplayUI(false);
            DisplayAttach(false);
            return;
        }


        Transform nearestSpot = GetNearestSpot(m_currentInteractable);

        //RaycastHit2D hit = Physics2D.Raycast(nearestSpot.transform.position, m_currentInteractable.transform.position, 50f);

        DrawLine(nearestSpot.transform.position, m_currentInteractable.transform.position, Color.cyan);
        DisplayAttach(true);
        DisplayUI(true);


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
            SpriteRenderer rend = spot.GetComponent<SpriteRenderer>();
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

        GameObject.Destroy(m_line, duration);
    }

    public Transform GetNearestSpot(Interactable item)
    {
        Transform nearestSpot = null;
        float minDistance = 10.0f;

        foreach (var spot in spots)
        {
            float distance = Vector2.Distance(item.transform.position, spot.transform.position);
            if (distance < minDistance)
            {
                nearestSpot = spot;
                minDistance = distance;
            }
        }

        return nearestSpot;
    }

    public void Attach()
    {

    }

    public Interactable Detect()
    {
        float minDistance = 10.0f;
        Interactable nearestInteractable = null;

        foreach (var item in itemDetectedList)
        {
            float distance = Vector2.Distance(item.transform.position, transform.position);
            if (distance < minDistance)
            {
                nearestInteractable = item;
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
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        var item = collision.GetComponent<Interactable>();

        if (item != null && itemDetectedList.Contains(item))
        {
            itemDetectedList.Remove(item);
        }
    }


}
