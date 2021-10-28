using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BlockTile : MonoBehaviour {

    public int number;
    public int c;
    public int r;
    public GameObject tmpObject;

    private float rotationSpeed = 0.025f;

    public int state;

    private TextMeshProUGUI tmp;

    public bool onAnimation = false;

    [SerializeField]
    private Material idleMaterial;
    [SerializeField]
    private Material overMaterial;
    [SerializeField]
    private Material matchMaterial;

    private MeshRenderer meshRenderer;
    private BoxCollider tileCollider;

    void Start () {
        meshRenderer = GetComponent<MeshRenderer>();
        tileCollider = GetComponent<BoxCollider>();
    }

    void Update () {

    }

    public void InitBlock (int _c, int _r, int value) {
        number = value;
        c = _c;
        r = _r;
        state = 0; // 0-> Hidden, 1-> Selected, 2-> Match finded

        tmp = tmpObject.GetComponent<TextMeshProUGUI>();

        tmp.text = number.ToString();
    }

    private void OnMouseEnter () {
        if (state != 2)
            meshRenderer.material = overMaterial;
    }

    private void OnMouseExit () {
        if (state != 2)
            meshRenderer.material = idleMaterial;
    }

    private void OnMouseDown () {
        if (state == 0)
            StartCoroutine("ShowValue");
    }

    public void MatchFinded () {
        state = 2;
        tileCollider.enabled = false;
        meshRenderer.material = matchMaterial;
    }

    IEnumerator ShowValue () {
        tileCollider.enabled = false;
        state = 1;
        onAnimation = true;
        while (transform.localEulerAngles.z > 0) {
            float dif = transform.localEulerAngles.z;
            Quaternion newRotation;

            if (dif < 5f) {
                tileCollider.enabled = true;
                newRotation = Quaternion.identity;
                onAnimation = false;
            } else {
                dif *= rotationSpeed;
                newRotation = Quaternion.Euler(0, 0, transform.localEulerAngles.z - dif);
            }
            transform.rotation = newRotation;
            yield return null;
        }
    }


    IEnumerator HideValue () {
        tileCollider.enabled = false;
        state = 0;
        onAnimation = true;
        while (transform.eulerAngles.z < 180) {
            float dif = 180 - transform.eulerAngles.z;
            Quaternion newRotation;

            if (dif < 5f) {
                tileCollider.enabled = true;
                newRotation = Quaternion.Euler(0, 0, 180);
                onAnimation = false;
            } else {
                dif *= rotationSpeed;
                newRotation = Quaternion.Euler(0, 0, transform.localEulerAngles.z + dif);
            }

            transform.rotation = newRotation;
            yield return null;
        }
    }

}