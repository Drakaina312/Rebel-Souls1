using System.Collections.Generic;
using System.Security;
using UnityEngine;
using UnityEngine.UI;

public class UIParticalSys : MaskableGraphic 
{
    [SerializeField] private ParticleSystemRenderer _particleSystemRenderer;
    [SerializeField] private Camera _bakeCamera;
    [SerializeField] private Texture _texture;

    public override Texture mainTexture => _texture ?? base.mainTexture;
    private void Update()
    {
        SetVerticesDirty();
    }
    protected override void OnPopulateMesh(Mesh mesh)
    {
        mesh.Clear();  
        if(_particleSystemRenderer != null && _bakeCamera != null)
           _particleSystemRenderer.BakeMesh(mesh, _bakeCamera);
    }


}