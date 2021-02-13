<!-- ABOUT THE PROJECT -->
## Mesh-Gizmos for Unity 
A collection of functions that imitate Unity's gizmo functionality using transforms and meshes.
![alt text](https://github.com/Genso-0/Mesh-Gizmos/blob/master/Assets/Mesh_Gizmos/Project%20Information/Gizmos.PNG)

### Built With
Unity 2020.1.4f1 
Universal Render Pipeline

### Language
C#

<!-- GETTING STARTED -->
## Getting Started
1) Add Mesh Gizmos Container prefab to your scene.
2) From your script, find MeshGizmos instance with MeshGizmos.Instance.  (eg. var _Gizmos = MeshGizmos.Instance;)
3) Use instance to call each gizmo as you would with regurlar gizmo calls. (eg. _Gizmos.DrawLine(Vector3.zero, Vector3.one, Color.yellow);) Does not need to be in OnDrawGizmos().
4) If you would like to use your own material then you can add it to the Material reference in Mesh Gizmos Container prefab.

For use with HDRP some extra steps are needed. 
1) Find gizmo material
![alt text](https://github.com/Genso-0/Mesh-Gizmos/blob/master/Assets/Mesh_Gizmos/Project%20Information/Working%20with%20HDRP/1FindGizmoMaterial.PNG)



2) Select shader tab ->HDRP -> Lit
![alt text](https://github.com/Genso-0/Mesh-Gizmos/blob/master/Assets/Mesh_Gizmos/Project%20Information/Working%20with%20HDRP/2MaterialWithError.PNG)



3)Material should now look like this
![alt text](https://github.com/Genso-0/Mesh-Gizmos/blob/master/Assets/Mesh_Gizmos/Project%20Information/Working%20with%20HDRP/3MaterialWithNoErrorReadyForHDRP.PNG)
<!-- USAGE EXAMPLES -->
## Usage
See [ExampleGizmoCalls.cs](https://github.com/Genso-0/Mesh-Gizmos/blob/master/Assets/Mesh_Gizmos/Scripts/ExampleGizmoCalls.cs) for usage examples.

<!-- LICENSE -->
## License

Distributed under the MIT License. See [LICENSE](https://github.com/Genso-0/Mesh-Gizmos/blob/master/LICENSE) for more information.

<!-- CONTACT -->
## Contact

[@genso_0](https://twitter.com/genso_0)

Project Link: [Mesh-Gizmos](https://github.com/Genso-0/Mesh-Gizmos)
