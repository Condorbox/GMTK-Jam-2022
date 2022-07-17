using UnityEngine;

public class TrackerPlayer : MonoBehaviour
{
    public new Renderer renderer;

    private MaterialPropertyBlock propBlock;
    [SerializeField]
    private CapsuleCollider playerCol;

    private void Awake()
    {
        propBlock = new MaterialPropertyBlock();
    }

    private void Update()
    {
        propBlock.SetVector("_SpherePos", playerCol.transform.position);
        propBlock.SetFloat("_SphereRadius", playerCol.radius);
        renderer.SetPropertyBlock(propBlock);
    }
}