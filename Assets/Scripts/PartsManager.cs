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
    public Transform m_DropQuestion;
    public bool m_Rpressed = false;
    public int m_selectedIndex = 0;
    public SpotIndex m_SelectedSpot;
    public float m_offset;
    Coroutine m_AttachCo;
    public bool m_Attaching = false;
    public float m_AttachDelay = 2.0f;
    public float m_AttachForceUp = 200.0f;
    public Vector2 m_currentItemPosRef;
    public float m_MovementSmoothing = 0.5f;
    public float m_maxVelocityAttach = 1000f;
    public bool m_DropDisplayed = false;
    public int m_DropIndex = 0;

    public Sprite m_normalSprite;
    public Sprite m_selectedSprite;
    public Color m_selectedColor;

    private HingeJoint2D[] m_joints = new HingeJoint2D[10];
    public bool m_ObjectEnters = false;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        m_ObjectEnters = false;
        Interactable newNearestInteractable = null;
        newNearestInteractable = Detect();

        if (m_currentInteractable != newNearestInteractable)
        {
            m_currentInteractable = newNearestInteractable;
        }

        //if (m_DropDisplayed && m_ObjectEnters)
        //{
        //    m_DropDisplayed = false;
        //    m_ObjectEnters = false;
        //}

        if (m_currentInteractable != null)
        {
            m_DropDisplayed = false;
            DisplayUIDrop(false);
            UnselectSprite(m_DropIndex);


        }

        if (!m_DropDisplayed)
            DisplayBind();
        m_SelectedSpot = (SpotIndex)m_selectedIndex;

        if (m_DropDisplayed)
            DropSelect();

        if (Input.GetKeyDown(KeyCode.X))
        {
            if (m_Attaching || CheckSpotsEmpty())
            {

                    m_DropDisplayed = false;
                    DisplayUIDrop(false);
                    DisplayAllAttach(false);
                
            }
        else
            {
                if (m_currentInteractable == null)
                {
                    m_DropDisplayed = true;
                    Dropintent();
                }

            }
        }

    }

    public bool CheckSpotsFull()
    {
        bool full = true;
        foreach (var spot in spots)
        {
            if (!spot.isTaken)
                full = false;
        }
        return full;
    }

    public bool CheckSpotsEmpty()
    {
        bool empty = true;
        foreach (var spot in spots)
        {
            if (spot.isTaken)
                empty = false;
        }
        return empty;
    }

    public void DisplayBind()
    {
        if (m_currentInteractable == null)
        {
            DisplayUIAttach(false);
            DisplayAllAttach(false);
            return;
        }

        if (CheckSpotsFull())
            return;

        if (Input.GetKeyDown(KeyCode.R))
        {
            m_Rpressed = true;
            m_selectedIndex = (m_selectedIndex + 1) % spots.Length;

            while (spots[m_selectedIndex].isTaken)
            {
                m_selectedIndex = (m_selectedIndex + 1) % spots.Length;
            }
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

        DisplayFreeAttach(true);
        DisplayUIAttach(true);

        if (Input.GetKeyDown(KeyCode.E) && !spots[m_selectedIndex].isTaken)
        {
            Interactable lockedInteractable = m_currentInteractable;

            m_AttachCo = StartCoroutine(Attach(lockedInteractable, m_selectedIndex));
        }

    }

    public void DisplayUIAttach(bool state)
    {
        m_AttachQuestion.gameObject.SetActive(state);
        m_AttachQuestion.transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    public void DisplayUIDrop(bool state)
    {
        m_DropQuestion.gameObject.SetActive(state);
        m_DropQuestion.transform.rotation = Quaternion.Euler(0, 0, 0);
    }


    void DisplayFreeAttach(bool state)
    {
        foreach (var spot in spots)
        {
            SpriteRenderer rend = spot.tr.GetComponent<SpriteRenderer>();
            if (rend != null)
            {
                rend.enabled = state;

                if (spot.isTaken)
                    rend.enabled = false;
            }
        }
    }


    void DisplayAllAttach(bool state)
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



    void DisplayTakenAttach()
    {
        foreach (var spot in spots)
        {
            SpriteRenderer rend = spot.tr.GetComponent<SpriteRenderer>();
            if (rend != null)
            {
                rend.enabled = false;

                if (spot.isTaken)
                    rend.enabled = true;
            }
        }
    }

    void SelectSprite(int spotIndex)
    {
        spots[spotIndex].tr.GetComponent<SpriteRenderer>().sprite = m_selectedSprite;
    }

    void SelectItem(int spotIndex)
    {
        if (spots[spotIndex].isTaken)
            spots[spotIndex].tr.GetComponentInChildren<Interactable>().GetComponent<SpriteRenderer>().color = m_selectedColor;
    }

    void UnselectSprite(int spotIndex)
    {
        spots[spotIndex].tr.GetComponent<SpriteRenderer>().sprite = m_normalSprite;
    }

    public void Dropintent()
    {
        DisplayUIDrop(true);
        DisplayAllAttach(true);
        SelectSprite(m_DropIndex);
        //SelectItem(m_DropIndex);
    }

    public void DropSelect()
    {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            DisplayUIDrop(false);
            DisplayAllAttach(false);
            m_DropDisplayed = false;
            return;
        }

        if (!m_DropDisplayed)
            return;

        DisplayUIDrop(true);
        DisplayAllAttach(true);
        SelectSprite(m_DropIndex);


        if (Input.GetKeyDown(KeyCode.R))
        {
            UnselectSprite(m_DropIndex);
            m_DropIndex = (m_DropIndex + 1) % spots.Length;
            SelectSprite(m_DropIndex);
            //SelectItem(m_DropIndex);
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            Drop(m_DropIndex);
        }
    }

    public void Drop(int dropIndex)
    {
        if (!spots[dropIndex].isTaken)
            return;

        m_joints[dropIndex].enabled = false;
        m_joints[dropIndex].breakForce = 0;
        spots[dropIndex].isTaken = false;
        //Destroy(m_joints[dropIndex]);

        //m_joints[dropIndex].breakTorque = 0;
        spots[dropIndex].tr.GetComponentInChildren<Interactable>().transform.parent = null;
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
            if (!spots[i].isTaken)
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
        }

        //nearestSpot = spots[(int)m_selectedIndex].transform;

        return nearestSpot;
    }

    public IEnumerator Attach(Interactable item, int lockedIndex)
    {
        if (!m_Attaching)
        {
            Vector3 oldPos = transform.position;
            Vector3 newPos = oldPos + new Vector3(0, 1.3f, 0);
            if (spots[lockedIndex].tr.transform.position.y < transform.position.y)
            {
                transform.position = newPos;
                transform.GetComponent<Rigidbody2D>().gravityScale = 0;
            }

            m_Attaching = true;

            yield return new WaitForSeconds(m_AttachDelay);

            m_Rpressed = false;

            Rigidbody2D itemRB = item.GetComponent<Rigidbody2D>();

            if (itemRB != null)
            {
                HingeJoint2D hingejoint = gameObject.AddComponent<HingeJoint2D>();
                hingejoint.breakForce = 1000;
                hingejoint.breakTorque = 1000;
                hingejoint.connectedBody = itemRB;
                m_joints[lockedIndex] = hingejoint;

                Vector3 vectorBind = spots[lockedIndex].tr.transform.position - item.transform.position;
                Vector3 vectorAttach = item.attach.position - item.transform.position;
                Vector3 vectorInner = spots[lockedIndex].tr.transform.position - GameObject.FindGameObjectWithTag("Player").transform.position;
                float attachDiff = Vector2.Distance(item.attach.position, item.transform.position);
                float angle = Vector2.SignedAngle(vectorBind, vectorAttach);


                itemRB.constraints = RigidbodyConstraints2D.FreezeRotation;

                item.transform.position = spots[lockedIndex].tr.transform.position;// - vectorBind.normalized * (attachDiff + m_offset);

                float innerAngle = Vector2.SignedAngle(vectorAttach, vectorInner);
                item.transform.Rotate(0, 0, 180 + innerAngle);


                //item.transform.position = Vector2.SmoothDamp(item.transform.position, spots[m_selectedIndex].tr.transform.position, ref m_currentItemPosRef, m_MovementSmoothing);

                item.transform.position += vectorInner.normalized * attachDiff;
                item.transform.parent = spots[lockedIndex].tr.transform;
                //item.transform.rotation = Quaternion.Euler(new Vector3(0,0, innerAngle));

                //GameObject.FindGameObjectWithTag("Player").transform.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
                spots[lockedIndex].isTaken = true;
            }

            transform.GetComponent<Rigidbody2D>().gravityScale = 1;

            m_Attaching = false;

        }

        yield return null;
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
            Debug.Log("ENTER");
            m_ObjectEnters = true;

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
