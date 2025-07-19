// File: Assets/Editor/CreatePlayerAnimator.cs
using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;

public class CreatePlayerAnimator
{
    [MenuItem("Tools/Create Player Animator Controller")]
    public static void CreateController()
    {
        string path = "Assets/PlayerAnimator.controller";

        var controller = AnimatorController.CreateAnimatorControllerAtPath(path);

        // Add Parameters
        controller.AddParameter("speed", AnimatorControllerParameterType.Float);
        controller.AddParameter("verticalVelocity", AnimatorControllerParameterType.Float);
        controller.AddParameter("isGrounded", AnimatorControllerParameterType.Bool);

        // Add states
        var root = controller.layers[0].stateMachine;

        var idle = root.AddState("Idle");
        var run = root.AddState("Run");
        var jump = root.AddState("Jump");
        var fall = root.AddState("Fall");

        // Set Idle as default
        root.defaultState = idle;

        // Transitions: Idle ↔ Run
        var toRun = idle.AddTransition(run);
        toRun.AddCondition(AnimatorConditionMode.Greater, 0.1f, "speed");
        toRun.hasExitTime = false;

        var toIdle = run.AddTransition(idle);
        toIdle.AddCondition(AnimatorConditionMode.Less, 0.1f, "speed");
        toIdle.hasExitTime = false;

        // Any → Jump
        var anyToJump = root.AddAnyStateTransition(jump);
        anyToJump.AddCondition(AnimatorConditionMode.Greater, 0.1f, "verticalVelocity");
        anyToJump.AddCondition(AnimatorConditionMode.If, 0, "isGrounded");
        anyToJump.hasExitTime = false;

        // Any → Fall
        var anyToFall = root.AddAnyStateTransition(fall);
        anyToFall.AddCondition(AnimatorConditionMode.Less, -0.1f, "verticalVelocity");
        anyToFall.hasExitTime = false;

        AssetDatabase.SaveAssets();
        Debug.Log("✅ Player Animator Controller created at: " + path);
    }
}
