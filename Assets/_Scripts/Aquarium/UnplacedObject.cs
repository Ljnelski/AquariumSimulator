using UnityEngine;

public class UnplacedObject : MonoBehaviour
{
    [SerializeField] private MeshRenderer _mesh;
    [SerializeField] private Material _unplacedMaterial;
    [SerializeField] private Material _placedMaterial;

    public void HighlightObject()
    {
        _mesh.material = _unplacedMaterial;
    }

    public void PlaceObject()
    {
        _mesh.material = _placedMaterial;
    }
}
