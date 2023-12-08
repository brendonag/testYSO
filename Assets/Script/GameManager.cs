using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject m_globe;
    [SerializeField] private float m_rotationSpeed;
    [SerializeField] private Vector2 m_scaleMinMax;
    [SerializeField] private Scrollbar m_scrolebar;
    [SerializeField] private Image m_image;

    private Vector3 m_lastTouchPosition;
    private bool m_breakRotation;
    private float m_scale;
    private List<GameObject> m_listItem = new List<GameObject>();
    private GameObject m_ItemFind;
    private int m_indexItemFind;

    // Start is called before the first frame update
    void Awake()
    {
        for(int i = 0; i < m_globe.transform.childCount;i++)
        {
            if(m_globe.transform.GetChild(i).gameObject.tag != "Wall")
                m_listItem.Add(m_globe.transform.GetChild(i).gameObject);
        }
        m_indexItemFind = Random.Range(0, m_listItem.Count);
        m_ItemFind = Instantiate(m_listItem[m_indexItemFind], m_image.transform);
        m_ItemFind.transform.localScale = Vector3.one * 30;
        m_ItemFind.transform.rotation = Quaternion.Euler(-90,0,0);
        m_listItem[m_indexItemFind].tag = "Item"; 
    }

    private void Start()
    {
        

    }
    // Update is called once per frame
    void Update()
    {
        if(Input.touchCount > 0)
        {
            Touch l_touch = Input.GetTouch(0);
            Vector3 l_touchPosition = l_touch.position;


            if (l_touch.phase == TouchPhase.Began)
            {
                m_lastTouchPosition = l_touchPosition;

                RaycastHit l_hit;
                Ray l_ray = Camera.main.ScreenPointToRay(l_touch.position);

                if (Physics.Raycast(l_ray, out l_hit))
                {
                    if (l_hit.transform.tag == "Item")
                    {
                        //NewItemResearch();
                        StartCoroutine(NewItem());
                    }
                }
            }
            if(l_touch.phase == TouchPhase.Moved)
            {
                SetGlobeScale();

                if(!m_breakRotation)
                {
                    Vector3 l_dir = (m_lastTouchPosition - l_touchPosition).normalized* m_rotationSpeed;
                    l_dir = new Vector3(-l_dir.y,l_dir.x);
                    m_globe.transform.Rotate(l_dir,Space.World);
                    m_lastTouchPosition = l_touchPosition;
                }

            }

            if(l_touch.phase == TouchPhase.Ended)
            {
                m_breakRotation = false;
            }
        }
    }

    void SetGlobeScale()
    {
        if(m_scrolebar.value != m_scale)
        {
            m_breakRotation = true;
            m_scale = m_scrolebar.value;
            float l_newScale = (m_scaleMinMax.y - m_scaleMinMax.x);
            float l_tempo = m_scaleMinMax.x +m_scale * l_newScale;
            m_globe.transform.localScale = new Vector3(l_tempo, l_tempo, l_tempo) ;
        }
    }

    void NewItemResearch()
    {
        m_listItem[m_indexItemFind].tag = "Untagged";
        Destroy(m_ItemFind);
        m_indexItemFind = Random.Range(0, m_listItem.Count);
        m_ItemFind = Instantiate(m_listItem[m_indexItemFind], m_image.transform);
        m_ItemFind.transform.localScale = Vector3.one * 30;
        m_ItemFind.transform.rotation = Quaternion.Euler(-90, 0, 0);
        m_listItem[m_indexItemFind].tag = "Item";
    }

    IEnumerator NewItem()
    {
        m_image.GetComponent<Animator>().SetBool("PlayAnimation", true);       
        yield return new WaitForSeconds(0.75f);
        NewItemResearch();
        yield return new WaitForSeconds(0.25f);
        m_image.GetComponent<Animator>().SetBool("PlayAnimation", false);
    }
}
