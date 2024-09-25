using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class Spawner : MonoBehaviour
{
    [SerializeField] 
    private Block blockPrefab;
    public MoveAxis moveAxis;
    public ObjectPool BlockPool;
    private int returnBlockIndex = 0;

    public void SpawnBlock(int stackCount, int colorLevel) {
        RemoveBlockFromEnd(stackCount);

        var poolGameObject = BlockPool.GetGameObjectFromPool();
        Block block = poolGameObject.GetComponent<Block>();
        block.speed = blockPrefab.speed;

        if (Block.previusBlock != null && Block.previusBlock.gameObject != GameObject.Find("StartBlock")) {
            float x = moveAxis == MoveAxis.x ? transform.position.x : Block.previusBlock.transform.position.x;
            float z = moveAxis == MoveAxis.z ? transform.position.z : Block.previusBlock.transform.position.z;
            block.transform.position = new Vector3(x, Block.previusBlock.transform.position.y + blockPrefab.transform.localScale.y, z);
        }

        else {
            block.transform.position = transform.position;
        }

        poolGameObject.SetActive(true);

        block.moveAxis = moveAxis;
        block.GetComponent<Renderer>().material.color = Color.HSVToRGB(((colorLevel + (stackCount * 3)) / 100f) % 1f, 1f, 1f);
    }


    private void RemoveBlockFromEnd(int stackCount) {
        if(stackCount >= (BlockPool.MaximumOfObjects * 2) && returnBlockIndex <= BlockPool.MaximumOfObjects) {
            
            BlockPool.ReturnObjectToPool(returnBlockIndex);
            returnBlockIndex++;

            returnBlockIndex = returnBlockIndex >= BlockPool.MaximumOfObjects ? 0 : returnBlockIndex;
        }
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, blockPrefab.transform.localScale);
    }
}
