using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SubActionMng : MonoBehaviour {

	public enum ActionIndex{
		None, Climb, Sit_Down, Crouch
	}

	ActionIndex actionIndex;

	Animator animator;

	BasicMatchTarget bmTarget;

	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator>();
		bmTarget = GetComponent<BasicMatchTarget>();
	}

	//外部からの呼び出しで各値の設定
	public void SetSubActionValues(ActionIndex action, Transform[] matchTargets, BasicMatchTarget.BodyParts targetPart) {
		actionIndex = action;
		List<Transform> newList = new List<Transform>();
		newList.AddRange(matchTargets);
		bmTarget.targetList = newList;
		bmTarget.targetPart = targetPart;
		animator.SetBool("SubAction", (actionIndex != ActionIndex.None));
		animator.SetInteger("ActionIndex", (int)actionIndex);
	}
}
