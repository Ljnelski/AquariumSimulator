using UnityEngine;

public class HighlightMesh : MonoBehaviour
{
    [SerializeField] private MeshRenderer _mesh;
    [SerializeField] private Material _positiveMaterial;
    [SerializeField] private Material _negativeHightlightMaterial;
    [SerializeField] private Material _placedMaterial;

    public void ApplyPositiveHighlight()
    {
        _mesh.material = _positiveMaterial;
    }

    public void ApplyNegativeHighlight()
    {
        _mesh.material = _negativeHightlightMaterial;
    }

    public void RemoveHighlight()
    {
        _mesh.material = _placedMaterial;
    }

}
