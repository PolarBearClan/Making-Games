using Unity.Collections;
using UnityEngine;

public class SoundEmitterBranch : MonoBehaviour
{
    //Print names of GameObject inside the sphere
    public void BatchOverlapSphere()
    {
        var commands = new NativeArray<OverlapSphereCommand>(1, Allocator.TempJob);
        var results = new NativeArray<ColliderHit>(3, Allocator.TempJob);

        commands[0] = new OverlapSphereCommand(transform.position, 10f, QueryParameters.Default);

        OverlapSphereCommand.ScheduleBatch(commands, results, 1, 3).Complete();

        foreach (var hit in results)
            Debug.Log(hit.collider.name);

        commands.Dispose();
        results.Dispose();
    }
}