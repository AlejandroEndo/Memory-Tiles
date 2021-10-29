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
    [Header("Data")]
    public int number;
    public int c;
    public int r;

    [Header("Display")]
    private float rotationSpeed = 0.05f;
    public BlockState state;

    public BoxCollider tileCollider;
    public GameObject tmpObject;

    private TextMeshProUGUI tmp;
    private InGameUIController uiController;

    private MeshRenderer meshRenderer;
    [SerializeField]
    private Material idleMaterial;
    [SerializeField]
    private Material overMaterial;
    [SerializeField]
    private Material matchMaterial;

    #region init block
    void Start () {
        meshRenderer = GetComponent<MeshRenderer>();
        tileCollider = GetComponent<BoxCollider>();
        uiController = GameObject.FindGameObjectWithTag("UIController").GetComponent<InGameUIController>();

        tileCollider.enabled = true;
    }

    public void InitBlock (int _c, int _r, int value) {
        number = value;
        c = _c;
        r = _r;
        state = BlockState.HIDE;

        tmp = tmpObject.GetComponent<TextMeshProUGUI>();
        tmp.text = number.ToString();
    }
    #endregion

    #region mouse events
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
    #endregion

    #region Show/Hide/Match events
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
    #endregion

    #region Animation Coroutines
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
    #endregion
}