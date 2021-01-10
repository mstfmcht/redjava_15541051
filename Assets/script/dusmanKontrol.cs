using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class dusmanKontrol : MonoBehaviour
{
    public int resim;
    GameObject[] gidilecekNoktalar;
    bool aradakiNesafeyiBirKereAl = true;
    Vector3 aradakiMesafe;
    int aradakiMesafeSayaci = 0;
    bool ilerimiGerimi = true;
    GameObject karakter;
    RaycastHit2D ray;
    public LayerMask layermask;
    int hiz = 5;
    public Sprite onTaraf;
    public Sprite arkaTaraf;
    SpriteRenderer spriteRenderer;
    public GameObject kursun;
    float atesZamani = 0;

    void Start()
    {
        gidilecekNoktalar = new GameObject[transform.childCount];
        karakter = GameObject.FindGameObjectWithTag("Player");
        spriteRenderer = GetComponent<SpriteRenderer>();

        for (int i = 0; i < gidilecekNoktalar.Length; i++)
        {
            gidilecekNoktalar[i] = transform.GetChild(0).gameObject;
            gidilecekNoktalar[i].transform.SetParent(transform.parent);
        }
    }


    void FixedUpdate()
    {
        beniGordumu();
        if (ray.collider.tag=="Player")
        {
            hiz = 8;
            spriteRenderer.sprite = onTaraf;
            atesEt();
        }
        else
        {
            hiz = 3;
            spriteRenderer.sprite = arkaTaraf;
        }



        noktalaraGit();
    }
    void atesEt()
    {
        atesZamani += Time.deltaTime;
        if (atesZamani>Random.Range(0.2f,1))
        {
            Instantiate(kursun,transform.position,Quaternion.identity);
            atesZamani = 0;
        }
    }
    void beniGordumu()
    {
        Vector3 rayYonum = karakter.transform.position - transform.position;
        ray = Physics2D.Raycast(transform.position,rayYonum,1000,layermask);
        Debug.DrawLine(transform.position,ray.point,Color.magenta);
    }
    void noktalaraGit()
    {
        if (aradakiNesafeyiBirKereAl)
        {
            aradakiMesafe = (gidilecekNoktalar[aradakiMesafeSayaci].transform.position - transform.position).normalized;
            aradakiNesafeyiBirKereAl = false;

        }
        float mesafe = Vector3.Distance(transform.position, gidilecekNoktalar[aradakiMesafeSayaci].transform.position);
        transform.position += aradakiMesafe * Time.deltaTime * hiz;
        if (mesafe < 0.5f)
        {
            aradakiNesafeyiBirKereAl = true;
            if (aradakiMesafeSayaci == gidilecekNoktalar.Length - 1)
            {
                ilerimiGerimi = false;
            }
            else if (aradakiMesafeSayaci == 0)
            {
                ilerimiGerimi = true;

            }

            if (ilerimiGerimi)
            {
                aradakiMesafeSayaci++;
            }
            else
            {
                aradakiMesafeSayaci--;
            }
        }


    }
     public Vector2 getYon()
    {
        return (karakter.transform.position - transform.position).normalized;
    }






#if UNITY_EDITOR
    void OnDrawGizmos()
    {
    for(int i =0; i<transform.childCount;i++)
    {
    Gizmos.color = Color.red;
    Gizmos.DrawWireSphere(transform.GetChild(i).transform.position,1);
    }
    for(int i =0; i<transform.childCount-1;i++)
    {
    Gizmos.color=Color.blue;
    Gizmos.DrawLine(transform.GetChild(i).transform.position,transform.GetChild(i+1).transform.position);

    }

    }




#endif
}

#if UNITY_EDITOR
[CustomEditor(typeof(dusmanKontrol))]
[System.Serializable]
class dusmanKontrolEditor:Editor
{
      public override void OnInspectorGUI()
{
      dusmanKontrol script= (dusmanKontrol)target; 
      EditorGUILayout.Space();
      if(GUILayout.Button("ÜRET",GUILayout.MinWidth(100),GUILayout.Width(100)))
{
    GameObject yeniObjem= new GameObject();
    yeniObjem.transform.parent=script.transform;                 
    yeniObjem.transform.position=script.transform.position;   
    yeniObjem.name=script.transform.childCount.ToString(); //bu kısım testere objemizin altına yeni objeler eklememizi ve sırayla eklememizi sağlar//






}

EditorGUILayout.Space();
EditorGUILayout.PropertyField(serializedObject.FindProperty("layermask"));
EditorGUILayout.PropertyField(serializedObject.FindProperty("onTaraf"));
EditorGUILayout.PropertyField(serializedObject.FindProperty("arkaTaraf"));
EditorGUILayout.PropertyField(serializedObject.FindProperty("kursun"));
serializedObject.ApplyModifiedProperties();
serializedObject.Update();
}
}
#endif