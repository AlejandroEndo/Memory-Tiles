using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum BlockState {
    HIDE,
    SHOW,
    MATCHED
}

public class BlockTile : MonoBehaviour {

    public int number;
    public int c;
    public int r;
    public GameObject tmpObject;

    private float rotationSpeed = 0.025f;

    public BlockState state;

    private TextMeshProUGUI tmp;

    [SerializeField]
    private Material idleMaterial;
    [SerializeField]
    private Material overMaterial;
    [SerializeField]
    private Material matchMaterial;

    private MeshRenderer meshRenderer;
    public BoxCollider tileCollider;
    private InGameUIController uiController;

    void Start () {
        meshRenderer = GetComponent<MeshRenderer>();
        tileCollider = GetComponent<BoxCollider>();
        uiController = GameObject.FindGameObjectWithTag("UIController").GetComponent<InGameUIController>();
    }

    public void InitBlock (int _c, int _r, int value) {
        number = value;
        c = _c;
        r = _r;
        state = BlockState.HIDE;

        tmp = tmpObject.GetComponent<TextMeshProUGUI>();

        tmp.text = number.ToString();
    }

    private void OnMouseEnter () {
        if (state == BlockState.HIDE)
            meshRenderer.material = overMaterial;
    }

    private void OnMouseExit () {
        if (state != BlockState.MATCHED)
            meshRenderer.material = idleMaterial;
    }

    private void OnMouseDown () {
        if (state == BlockState.HIDE) {
            uiController.OnClickUpdated();
            StartShowAnimation();
            GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().CheckSelectedTiles();
        }
    }

    public void MatchFinded () {
        state = BlockState.MATCHED;
        tileCollider.enabled = false;
        meshRenderer.material = matchMaterial;
        transform.rotation = Quaternion.identity;
    }

    public void StartShowAnimation () {
        tileCollider.enabled = false;
        state = BlockState.SHOW;
        transform.rotation = Quaternion.Euler(0, 0, 180);
        StartCoroutine("ShowValue");
    }

    public void StartHideAnimation () {
        tileCollider.enabled = false;
        state = BlockState.HIDE;
        transform.rotation = Quaternion.identity;
        StartCoroutine("HideValue");
    }

    IEnumerator ShowValue () {
        while (transform.localEulerAngles.z > 0) {
            float dif = transform.localEulerAngles.z;
            Quaternion newRotation;

            if (dif < 5f) {
                newRotation = Quaternion.identity;
                transform.rotation = newRotation;
                tileCollider.enabled = true;
            } else {
                dif *= rotationSpeed;
                newRotation = Quaternion.Euler(0, 0, transform.localEulerAngles.z - dif);
            transform.rotation = newRotation;
            }
            yield return null;
        }
    }


    IEnumerator HideValue () {
        while (transform.eulerAngles.z < 180) {
            float dif = 180 - transform.eulerAngles.z;
            Quaternion newRotation;

            if (dif < 5f) {
                newRotation = Quaternion.Euler(0, 0, 180);
                transform.rotation = newRotation;
                tileCollider.enabled = true;
            } else {
                dif *= rotationSpeed;
                newRotation = Quaternion.Euler(0, 0, transform.localEulerAngles.z + dif);
            transform.rotation = newRotation;
            }

            yield return null;
        }
    }

}