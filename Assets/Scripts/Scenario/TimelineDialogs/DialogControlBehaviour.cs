using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

[Serializable]
public class DialogControlBehaviour : PlayableBehaviour
{
	bool launched = false;

	public override void ProcessFrame(Playable playable, FrameData info, object playerData)
	{
		ScenarioHandler handler = playerData as ScenarioHandler;

		if (handler == null)
			return;

		if (!launched)
		{
			handler.LaunchNextDialog();
			launched = true;
		}
	}
}
