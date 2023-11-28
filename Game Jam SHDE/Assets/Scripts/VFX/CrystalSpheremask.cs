using UnityEngine;

public class CrystalSpheremask : MonoBehaviour
{
    //The material of the SphereMask you whant to set
    public Material IntersectionMaterial;
    void Update()
    {
        //Set position of the SphereMask in the shader
        IntersectionMaterial.SetVector("SpherePosition_", this.transform.position);
        //Set radius of the Spheremask in the shader
        IntersectionMaterial.SetVector("SphereRadius_", this.transform.localScale / 2);
    }
}
