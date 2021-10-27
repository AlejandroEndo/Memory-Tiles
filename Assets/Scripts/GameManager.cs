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

    private void Start () {
        dataBlocks = DataManager.LoadGridData();
        blocksHolder = GameObject.FindGameObjectWithTag("BlockHolder").transform;

        SetGridSize();
        SetGridValues();
        BuildGridTiles();
    }

    private void Update () {
        // Save Data
        if (Input.GetKeyDown(KeyCode.Space)) {
            DataManager.SaveResult(sf);
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
        screenSize = Camera.main.orthographicSize;
        isPanoramic = Mathf.Min(Screen.width, Screen.height) == Screen.height;

        float tileScale = isPanoramic ? screenSize / width : screenSize / height;
        tileScale -= 0.05f;

        for (int c = 0; c < width; c++) {
            for (int r = 0; r < height; r++) {
                Vector3 scale = new Vector3(tileScale, 0.2f, tileScale);

                float x = width % 2 == 0 ? (tileScale * c) - (tileScale / 2) * 3 : (tileScale * c) - tileScale;
                float y = height % 2 == 0 ? (tileScale * r) - (tileScale / 2) * 3 : (tileScale * r) - tileScale;

                Vector3 pos = new Vector3(x, 0f, y) * tileScale;

                GameObject o = Instantiate(tilePrefab, pos, Quaternion.identity, blocksHolder);
                o.transform.localScale = scale;
                blocks.Add(o);
            }
        }
    }
}
