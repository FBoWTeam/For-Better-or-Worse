using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[Serializable]
public class DialogControlClip : PlayableAsset, ITimelineClipAsset
{
	[SerializeField]
	private DialogControlBehaviour template = new DialogControlBehaviour();

	public ClipCaps clipCaps => ClipCaps.None;

	public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
	{
		return ScriptPlayable<DialogControlBehaviour>.Create(graph, template);
	}
}
