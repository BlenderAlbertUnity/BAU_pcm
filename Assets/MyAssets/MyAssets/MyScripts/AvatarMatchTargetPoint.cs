using UnityEngine;
using System.Collections;

public class AvatarMatchTargetPoint : StateMachineBehaviour {

	[SerializeField] AvatarTarget targetBodyPart = AvatarTarget.Root;
	[SerializeField, Range(0, 1)] float start = 0, end = 1;

	[HeaderAttribute("match target")]
	public Transform matchTransform;		// 指定パーツが到達して欲しい座標と到達して欲しい回転

	[HeaderAttribute("Weights")]
	public Vector3 positionWeight = Vector3.one;		// matchPositionに与えるウェイト。(1,1,1)で自由、(0,0,0)で移動できない。e.g. (0,0,1)で前後のみ
	public float rotationWeight = 0;			// 回転に与えるウェイト。

	private MatchTargetWeightMask weightMask;

	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) 
	{
		//matchTransform = animator.transform.GetComponent<TargetMatching>().RightHand;
		//matchTransform = animator.transform.GetComponent<SubActionMng>().target;
		weightMask = new MatchTargetWeightMask (positionWeight, rotationWeight);
	}

	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) 
	{
		if(!animator.IsInTransition(layerIndex))
		animator.MatchTarget (matchTransform.position, matchTransform.rotation, targetBodyPart, weightMask, start, end);
	}
	/*
	public class StateMachineExample : StateMachineBehaviour
{

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //新しいステートに移り変わった時に実行
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //ステートが次のステートに移り変わる直前に実行
    }

    public override void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
    {
        //スクリプトが貼り付けられたステートマシンに遷移してきた時に実行
    }

    public override void OnStateMachineExit(Animator animator, int stateMachinePathHash)
    {
        //スクリプトが貼り付けられたステートマシンから出て行く時に実行
    }

    public override void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //MonoBehaviour.OnAnimatorMoveの直後に実行される
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //最初と最後のフレームを除く、各フレーム単位で実行
    }

    public override void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //MonoBehaviour.OnAnimatorIKの直後に実行される
    }
}*/
}

