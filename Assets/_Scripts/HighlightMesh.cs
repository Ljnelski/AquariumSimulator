using UnityEngine;

public class HighlightMesh : MonoBehaviour
{
    [SerializeField] private MeshRenderer[] _meshs;
    [SerializeField] private Material _positiveMaterial;
    [SerializeField] private Material _negativeHightlightMaterial;
    [SerializeField] private Material[] _placedMaterials;

    public void ApplyPositiveHighlight()
    {
        for (int i = 0; i < _meshs.Length; i++)
        {
            _meshs[i].material = _positiveMaterial;
        }
    }

    public void ApplyNegativeHighlight()
    {
        for (int i = 0; i < _meshs.Length; i++)
        {
            _meshs[i].material = _negativeHightlightMaterial;
        }
    }

    public void RemoveHighlight()
    {
        for (int i = 0; i < _meshs.Length; i++)
        {
            _meshs[i].material = _placedMaterials[i];
        }
    }

    private void OnValidate()
    {
        if(_meshs.Length != _placedMaterials.Length)
        {
            Debug.LogError("HighlightMesh ERRROR: The number of meshs and materials for placement do not match");
        }
    }

}
