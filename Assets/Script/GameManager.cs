using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject m_Globe;
    private Vector3 m_lastTouchPosition;
    private Quaternion m_saveRotation;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.touchCount > 0)
        {
            Touch l_touch = Input.GetTouch(0);
            Vector3 l_touchPosition = l_touch.position;
            
            if(l_touch.phase == TouchPhase.Began)
            {
                m_lastTouchPosition = l_touchPosition;

            }

            if(l_touch.phase == TouchPhase.Moved)
            {
                Vector3 l_dir = (m_lastTouchPosition - l_touchPosition ).normalized;
                l_dir = new Vector3(l_dir.y,l_dir.x);
                m_Globe.transform.Rotate(l_dir,Space.Self);
                m_lastTouchPosition = l_touchPosition;
                //m_Globe.transform.Rotate(new Vector3(l_dir.y, l_dir.x, 0));//+= new Vector3(l_dir.y, -l_dir.x, 0);
            }

            if(l_touch.phase == TouchPhase.Ended)
            {

            }
        }
    }
}
