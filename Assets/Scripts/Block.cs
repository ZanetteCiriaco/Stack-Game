using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public enum MoveAxis {
    x,
    z
}

public enum PlaceBlockReturn {
    PerfectStack, 
    PartialStack, 
    MissedStack 
}

public class Block : MonoBehaviour
{
     public float speed = 2f;
    [SerializeField] float sideLimit = 1.5f;
    private float direction = -1f;
    public float overlapTolerance = 0.2f;
    public MoveAxis moveAxis {get; set;}
    public GameObject perfectBlockEffectPrefab;
    public GameObject fallingBlockPrefab;
    public static Block currentBlock {get; private set; }
    public static Block previusBlock {get; private set; }

    private void OnEnable() 
    {
        if(previusBlock == null) previusBlock = GameObject.Find("StartBlock").GetComponent<Block>();
        currentBlock = this;  

        transform.localScale = previusBlock.transform.localScale;  
    }

    private void Update() 
    {
        Move();
    }

    private void Move() 
    {
        float position;

        if(moveAxis == MoveAxis.x) {
            transform.position += speed * Time.deltaTime * direction * transform.right;
            position = transform.position.x;
        } else {
            transform.position += speed * Time.deltaTime * direction * transform.forward;
            position = transform.position.z;
        }

        if(Math.Abs(position) >= sideLimit) {
            direction = -direction;

            if (moveAxis == MoveAxis.x) 
            {
                transform.position = new Vector3(
                    Mathf.Sign(transform.position.x) * sideLimit,
                    transform.position.y,
                    transform.position.z
                );
            } 
            else 
            {
                transform.position = new Vector3(
                    transform.position.x,
                    transform.position.y,
                    Mathf.Sign(transform.position.z) * sideLimit
                );
            }
        }
    }
    
    public PlaceBlockReturn PlaceBlock() {
        speed = 0;
        float maxSize = moveAxis == MoveAxis.x ? previusBlock.transform.localScale.x : previusBlock.transform.localScale.z;
        float overlapAbs = Mathf.Abs(GetOverlapValue());;

        if(overlapAbs >= maxSize) {
            this.AddComponent<Rigidbody>();
            previusBlock = null;
            currentBlock = null;
            return PlaceBlockReturn.MissedStack;
        }

        else if(overlapAbs <= Mathf.Abs(overlapTolerance)){
            transform.position = new Vector3(previusBlock.transform.position.x, transform.position.y, previusBlock.transform.position.z);
            previusBlock = this;
            GameObject effect = Instantiate(perfectBlockEffectPrefab);
            Vector3 blockScale = transform.localScale;
            effect.transform.position = new Vector3(transform.position.x, transform.position.y - 0.1f, transform.position.z);
            effect.transform.localScale = new Vector3(blockScale.x + 0.15f, blockScale.z + 0.15f, 1);
            Destroy(effect, 0.7f);
            return PlaceBlockReturn.PerfectStack;
        }

        else {
            if(moveAxis == MoveAxis.x) ResizeBlockX(GetOverlapValue());
            else ResizeBlockZ(GetOverlapValue());
        }

        previusBlock = this;
        return PlaceBlockReturn.PartialStack;
    }
    private float GetOverlapValue() 
    {
        return moveAxis == MoveAxis.x ?
            transform.position.x - previusBlock.transform.position.x :
            transform.position.z - previusBlock.transform.position.z;
    }
    private void ResizeBlockX(float suplusValue) {  
        float newSize = previusBlock.transform.localScale.x - Mathf.Abs(suplusValue);
        Vector3 scale = transform.localScale;
        float newPosition = previusBlock.transform.position.x + (suplusValue / 2); 

        transform.localScale = new Vector3(newSize, scale.y, scale.z);
        transform.position = new Vector3(newPosition, transform.position.y, transform.position.z);

        FallingOverlapBlock(suplusValue, true);  
    }
    private void ResizeBlockZ(float suplusValue) {
        float newSize = previusBlock.transform.localScale.z - Mathf.Abs(suplusValue);
        Vector3 scale = transform.localScale;
        float newPosition = previusBlock.transform.position.z + (suplusValue / 2);

        transform.localScale = new Vector3(scale.x, scale.y, newSize);
        transform.position = new Vector3(transform.position.x, transform.position.y, newPosition);

        FallingOverlapBlock(suplusValue, false);
    }
    private void FallingOverlapBlock(float suplusValue, bool isXAxis) {
        float over = Mathf.Abs(suplusValue);
        var fallingBlock = Instantiate(fallingBlockPrefab);
        fallingBlock.GetComponent<Renderer>().material.color = currentBlock.GetComponent<Renderer>().material.color;

        float newPosition = isXAxis 
            ? (Mathf.Abs(transform.localScale.x) / 2) + (over / 2) + Mathf.Abs(transform.position.x)
            : (Mathf.Abs(transform.localScale.z) / 2) + (over / 2) + Mathf.Abs(transform.position.z);

        if (suplusValue < 0) {
            newPosition = -newPosition;
        }

        fallingBlock.transform.localScale = isXAxis
            ? new Vector3(over, 0.2f, transform.localScale.z)
            : new Vector3(transform.localScale.x, 0.2f, over);

        fallingBlock.transform.position = isXAxis
            ? new Vector3(newPosition, transform.position.y, transform.position.z)
            : new Vector3(transform.position.x, transform.position.y, newPosition);
    }
}
