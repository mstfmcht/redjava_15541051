using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class karakterKontrol : MonoBehaviour
{

    public Sprite[] beklemeAnim;
    public Sprite[] ziplamaAnim;
    public Sprite[] yurumeAnim;
    public Text canText;
    public Text AltinText;
    public Image SiyahArkaPlan;
   
    int can = 100;
    
    SpriteRenderer spriteRenderer;


    int beklemeAnimSayac = 0;
    int yurumeAnimSayac = 0;
    int altinSayaci = 0;
 

    Rigidbody2D fizik;
    Vector3 vec;
    Vector3 kameraSonPos;
    Vector3 kameraIlkPos;

    float horizontal = 0;
    float beklemeAnimZaman = 0;
    float yurumeAnimZaman = 0;
    float siyahArkaPlanSayaci;
   
    bool birKereZipla = true;

    float anaMenuyeDonZaman=0;

    GameObject kamera;
    void Start()
    {
        SiyahArkaPlan.gameObject.SetActive(false);
        Time.timeScale = 1;
        spriteRenderer = GetComponent<SpriteRenderer>();
        fizik = GetComponent<Rigidbody2D>();
        kamera = GameObject.FindGameObjectWithTag("MainCamera");
        if (SceneManager.GetActiveScene().buildIndex>PlayerPrefs.GetInt("kacincilevel"))
        {
            PlayerPrefs.SetInt("kacincilevel", SceneManager.GetActiveScene().buildIndex);
        }
        
        kameraIlkPos = kamera.transform.position - transform.position;
        canText.text = "CAN   " + can;
        AltinText.text = "Altın 20 -" + altinSayaci.ToString();


    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (birKereZipla)
            {
                fizik.AddForce(new Vector2(0, 500));
                birKereZipla = false;
            }

        }
    }


    void FixedUpdate()
    {
        karakterHareket();
        Animasyon();
        if (can <=0)//Öldüğünde
        {
            Time.timeScale = 0.3f;
            canText.enabled = false;
            siyahArkaPlanSayaci += 0.05f;
            SiyahArkaPlan.gameObject.SetActive(true);
            SiyahArkaPlan.color = new Color(0,0,0,siyahArkaPlanSayaci);
            anaMenuyeDonZaman += Time.deltaTime;
            if (anaMenuyeDonZaman>1)
            {
                SceneManager.LoadScene("anamenu");
            }
        }
    }
    void LateUpdate()
    {
        kameraKontrol();
    }
    void karakterHareket()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vec = new Vector3(horizontal * 10, fizik.velocity.y, 0);
        fizik.velocity = vec;

    
    }
    void OnCollisionEnter2D(Collision2D col)
    {
        birKereZipla = true;
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag=="kursun")
        {
            can -= 2;
            canText.text = "CAN   " + can.ToString();
        }
        else if (col.gameObject.tag == "dusman")
        {
            can -= 15;
            canText.text = "CAN   " + can.ToString();
        }
        else if (col.gameObject.tag == "testere")
        {
            can -= 5;
            canText.text = "CAN   " + can.ToString();
        }
        else if (col.gameObject.tag == "levelbitsin")
        {

            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
        }
        else if (col.gameObject.tag == "canver")
        {
            can +=5;
            canText.text = "CAN   " + can.ToString();
            col.GetComponent<BoxCollider2D>().enabled = false;
            col.GetComponent<canver>().enabled = true;
            Destroy(col.gameObject,2);

        }
        else if (col.gameObject.tag == "canver")
        {
            can += 5;
            canText.text = "CAN   " + can.ToString();
            col.GetComponent<BoxCollider2D>().enabled = false;
            col.GetComponent<canver>().enabled = true;
            Destroy(col.gameObject, 2);

        }
        else if (col.gameObject.tag == "altin")
        {
            altinSayaci++;
            AltinText.text = "Altin 20 -" + altinSayaci.ToString();
            Debug.Log(altinSayaci++);
            Destroy(col.gameObject);

        }
        else if (col.gameObject.tag == "su")
        {
            can = 0;

        }
    }
    void kameraKontrol()
    {
        kameraSonPos = kameraIlkPos + transform.position;
        kamera.transform.position = Vector3.Lerp(kamera.transform.position,kameraSonPos,0.05f);

    }
    
    void Animasyon()
    {
        if (birKereZipla)
        {
            if (horizontal == 0)
            {

                beklemeAnimZaman += Time.deltaTime;
                if (beklemeAnimZaman > 0.1f)
                {
                    spriteRenderer.sprite = beklemeAnim[beklemeAnimSayac++];
                    if (beklemeAnimSayac == beklemeAnim.Length)
                    {
                        beklemeAnimSayac = 0;
                    }
                    beklemeAnimZaman = 0;
                }

            }

            else if (horizontal > 0)
            {
                yurumeAnimZaman += Time.deltaTime;
                if (yurumeAnimZaman > 0.01f)
                {
                    spriteRenderer.sprite = yurumeAnim[yurumeAnimSayac++];
                    if (yurumeAnimSayac == yurumeAnim.Length)
                    {
                        yurumeAnimSayac = 0;
                    }
                    yurumeAnimZaman = 0;
                }
                transform.localScale = new Vector3(1, 1, 1);
            }
            else if (horizontal < 0)
            {
                yurumeAnimZaman += Time.deltaTime;
                if (yurumeAnimZaman > 0.01f)
                {
                    spriteRenderer.sprite = yurumeAnim[yurumeAnimSayac++];
                    if (yurumeAnimSayac == yurumeAnim.Length)
                    {
                        yurumeAnimSayac = 0;
                    }
                    yurumeAnimZaman = 0;
                }
                transform.localScale = new Vector3(-1, 1, 1);
            }

        }
        else
        {
            if (fizik.velocity.y>0)
            {
                spriteRenderer.sprite = ziplamaAnim[0];
            }
            else
            {
                spriteRenderer.sprite = ziplamaAnim[1];
            }
            if (horizontal>0)
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
            else if (horizontal < 0)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
            
        }
    }
}
    

