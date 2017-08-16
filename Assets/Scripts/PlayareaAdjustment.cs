using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayareaAdjustment : MonoBehaviour
{
    public MapHolder mapHolder;
    private int[,] map;

	void Start ()
	{
        //ToDo: Start Coroutine and wait for Map to be ready
	    map = mapHolder.getMap();

        // Set child plane size equal to map length * obstacle size
	    AdjustPlayAreaDimensions();
	}

    private void AdjustPlayAreaDimensions()
    {
        Debug.Log(map);
        Transform playAreaPlane = transform.GetChild(0);
        Debug.Log(playAreaPlane.GetComponent<MeshRenderer>());
        Vector3 playAreaDimensions = playAreaPlane.GetComponent<MeshRenderer>().bounds.size;

        playAreaPlane.localScale = new Vector3(map.GetLength(0) * 2/playAreaDimensions.x, 0, map.GetLength(1) * 2/ playAreaDimensions.z);
        /* Not needed anymore. Floor will always be squared for now.
        playAreaPlane.position = playAreaPlane.position 
            + new Vector3(playAreaPlane.GetComponent<Renderer>().bounds.size.x/2,
                0,
                playAreaPlane.GetComponent<Renderer>().bounds.size.z / 2);
                */
    }
}
