using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

public class AnimationSystem : MonoBehaviour
{
    PlayableGraph playableGraph;
    readonly AnimationMixerPlayable mainMixer;
    readonly AnimationMixerPlayable locoMixer;

    AnimationClipPlayable clipPlayable;

    private Coroutine blendIn;
    private Coroutine blendOut;

    public AnimationSystem(Animator animator, AnimationClip idleClip, AnimationClip walkClip)
    {
        playableGraph = PlayableGraph.Create("AnimationSystem");
        AnimationPlayableOutput output = AnimationPlayableOutput.Create(playableGraph,"Animation",animator);

        mainMixer = AnimationMixerPlayable.Create(playableGraph, 2);
        output.SetSourcePlayable(mainMixer);

        locoMixer = AnimationMixerPlayable.Create(playableGraph, 2);
        mainMixer.ConnectInput(0, locoMixer, 0);

        playableGraph.GetRootPlayable(0).SetInputWeight(0, 1f);

        AnimationClipPlayable idlePlayable = AnimationClipPlayable.Create(playableGraph, idleClip);
        AnimationClipPlayable walkPlayable = AnimationClipPlayable.Create(playableGraph, walkClip);

        idlePlayable.GetAnimationClip().wrapMode = WrapMode.Loop;
        walkPlayable.GetAnimationClip().wrapMode= WrapMode.Loop;

        locoMixer.ConnectInput(0, idlePlayable, 0);
        locoMixer.ConnectInput(1, walkPlayable, 0);

        playableGraph.Play();
    }

    public void UpdateLocomotion(Vector3 velocity, float maxSpeed)
    {
        float weight = Mathf.InverseLerp(0f, maxSpeed, velocity.magnitude);
        locoMixer.SetInputWeight(0, 1f - weight);
        locoMixer.SetInputWeight(1, weight);
    }

    public void Destroy()
    {
        if(playableGraph.IsValid())
        {
            playableGraph.Destroy();
        }
    }

}
