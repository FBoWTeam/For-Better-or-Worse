using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbController : MonoBehaviour
{
	public LineRenderer link;

	public float speed;

	bool ascending;
	int start, end;
	float progression;
	float step;
	
    void Start()
    {
		ascending = true;
		start = link.positionCount/2;
		end = link.positionCount/2 + 1;
		transform.position = link.GetPosition(start);
		progression = 0.0f;
    }
	
    void FixedUpdate()
    {
		if (progression < 1.0f)
		{
			step = (speed*Time.fixedDeltaTime) / (link.GetPosition(end) - link.GetPosition(start)).magnitude;
			progression += step;
			transform.position = Vector3.LerpUnclamped(link.GetPosition(start), link.GetPosition(end), progression);
		}
		else
		{
			progression = progression-1.0f;
			start = end;
			if (end == link.positionCount - 1 || end == 0)
				ascending = !ascending;
			end = ascending ? end + 1 : end - 1;
		}
    }
}
