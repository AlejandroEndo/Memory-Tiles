using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public SaveFile sf;
    public Blocks dataBlocks;

    public float screenSize;
    public bool isPanoramic;

    public List<GameObject> blocks;
    public int[,] dataGrid;

    public int width = 1;
    public int height = 1;

    public GameObject tilePrefab;
    public BoxCollider clickDisabler;
    private Transform blocksHolder;

    private bool selectionEnabled = true;


    private void Start () {
        dataBlocks = DataManager.LoadGridData();

        blocksHolder = GameObject.FindGameObjectWithTag("BlockHolder").transform;


        clickDisabler.enabled = false;

        SetGridSize();
        SetGridValues();
        BuildGridTiles();
    }

    private void Update () {
        // Save Data
        if (Input.GetKeyDown(KeyCode.Space)) {
            DataManager.SaveResult(sf);
        }

        if (Input.GetMouseButtonDown(0) && selectionEnabled) {
            CheckSelectedTiles();
        }
    }

    private void SetGridSize () {
        for (int i = 0; i < dataBlocks.blocks.Length; i++) {
            Block b = dataBlocks.blocks[i];

            if (b.C > width) width = b.C;
            if (b.R > height) height = b.R;
        }
        dataGrid = new int[width, height];
    }

    private void SetGridValues () {
        for (int i = 0; i < dataBlocks.blocks.Length; i++) {
            Block b = dataBlocks.blocks[i];
            dataGrid[b.C - 1, b.R - 1] = b.number;
        }
    }

    private void BuildGridTiles () {
        isPanoramic = Mathf.Min(Screen.width, Screen.height) == Screen.height;

        float screenHeight = Camera.main.orthographicSize;
        float screenWidth = ((float) Screen.width / Screen.height) * screenHeight;

        float tileScale;

        if (isPanoramic)
            tileScale = (screenHeight / height) * 1.5f;
        else
            tileScale = (screenWidth / width) * 1.5f;


        for (int c = 0; c < width; c++) {
            for (int r = 0; r < height; r++) {
                Vector3 scale = new Vector3(tileScale, 0.2f, tileScale);

                float x = width % 2 == 0 ? (tileScale * c) - (tileScale / 2) * height : (tileScale * c) - tileScale;
                float y = height % 2 == 0 ? (tileScale * r) - (tileScale / 2) * width : (tileScale * r) - tileScale;

                Vector3 pos = new Vector3(x, 0f, y) * 1.1f;

                GameObject o = Instantiate(tilePrefab, pos, Quaternion.Euler(0, 0, 180), blocksHolder);
                o.transform.localScale = scale;

                o.GetComponent<BlockTile>().InitBlock(c, r, dataGrid[c, r]);

                blocks.Add(o);
            }
        }
    }

    private void CheckSelectedTiles () {
        List<int> selectedTiles = new List<int>();
        for (int i = 0; i < blocks.Count; i++) {
            BlockTile bt = blocks[i].GetComponent<BlockTile>();
            if (bt.state == BlockState.SHOW) selectedTiles.Add(i);
        }
        Debug.Log("Kill me");
        if (selectedTiles.Count == 2) {
            SwitchBlocksCollider(false);
            //clickDisabler.enabled = true;
            StartCoroutine(ValidateSelectedTiles(selectedTiles));
        }
    }

    private void HideTiles () {
        for (int i = 0; i < blocks.Count; i++) {
            BlockTile bt = blocks[i].GetComponent<BlockTile>();
            if (bt.state == BlockState.SHOW) {
                //bt.state = BlockState.HIDE;
                bt.StartHideAnimation();
            }
        }
    }

    private void SwitchBlocksCollider (bool value) {
        Debug.Log("Funcion!" + value);
        selectionEnabled = value;
        for (int i = 0; i < blocks.Count; i++) {
            BlockTile bt = blocks[i].GetComponent<BlockTile>();
            bt.tileCollider.enabled = value;
        }
    }

    IEnumerator ValidateSelectedTiles (List<int> tiles) {
        BlockTile bt1 = blocks[tiles[0]].GetComponent<BlockTile>();
        BlockTile bt2 = blocks[tiles[1]].GetComponent<BlockTile>();

        yield return new WaitForSeconds(1.5f);

        if (bt1.number == bt2.number) {
            bt2.MatchFinded();
            bt1.MatchFinded();
        } else {
            //HideTiles();
            bt1.StartHideAnimation();
            bt2.StartHideAnimation();
        }
        //yield return new WaitForSeconds(1.5f);
        SwitchBlocksCollider(true);
        //clickDisabler.enabled = false;
        yield return null;
    }
}