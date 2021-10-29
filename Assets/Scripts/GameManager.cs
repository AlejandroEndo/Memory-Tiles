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
    private Transform blocksHolder;

    public int matches = 0;

    private bool gameOver = false;

    private InGameUIController uIController;

    private void Start () {
        dataBlocks = DataManager.LoadGridData();

        blocksHolder = GameObject.FindGameObjectWithTag("BlockHolder").transform;
        uIController = GameObject.FindGameObjectWithTag("UIController").GetComponent<InGameUIController>();

        SetGridSize();
        SetGridValues();
        BuildGridTiles();
    }

    private void Update () {
        if (gameOver) return;

        if(matches >= dataBlocks.blocks.Length / 2) {
            SwitchBlocksCollider(false);
            gameOver = true;
            uIController.GameOver();
            Debug.Log("GAME OVER!!");
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

    public void CheckSelectedTiles () {
        List<int> selectedTiles = new List<int>();
        for (int i = 0; i < blocks.Count; i++) {
            BlockTile bt = blocks[i].GetComponent<BlockTile>();
            if (bt.state == BlockState.SHOW) selectedTiles.Add(i);
        }
        if (selectedTiles.Count == 2) {
            StartCoroutine(ValidateSelectedTiles(selectedTiles));
        }
    }

    private void HideTiles () {
        for (int i = 0; i < blocks.Count; i++) {
            BlockTile bt = blocks[i].GetComponent<BlockTile>();
            if (bt.state == BlockState.SHOW) {
                bt.StartHideAnimation();
            }
        }
    }

    private void SwitchBlocksCollider (bool value) {
        for (int i = 0; i < blocks.Count; i++) {
            BlockTile bt = blocks[i].GetComponent<BlockTile>();
            bt.tileCollider.enabled = value;
        }
    }

    IEnumerator ValidateSelectedTiles (List<int> tiles) {
        BlockTile bt1 = blocks[tiles[0]].GetComponent<BlockTile>();
        BlockTile bt2 = blocks[tiles[1]].GetComponent<BlockTile>();

        SwitchBlocksCollider(false);
        yield return new WaitForSeconds(2f);

        if (bt1.number == bt2.number) {
            bt2.MatchFinded();
            bt1.MatchFinded();
            matches++;
        } else {
            HideTiles();
        yield return new WaitForSeconds(1.5f);
        }
        SwitchBlocksCollider(true);
        yield return null;
    }
}