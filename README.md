# JobVSNormalMeshWithComputeShaders
Testing the mesh generation &amp; its deformation performances using Unity's Job and normal C# systems along with compute shaders(compute &amp; graphic buffers[WIP])

**NOTE** Required Unity version is 2020.1 or later. 
Unity 2020.1 adds MeshData APIs for C# Jobs/Burst compatible way of reading & writing Mesh data; see https://docs.google.com/document/d/1QC7NV7JQcvibeelORJvsaTReTyszllOlxdfEsaVL2oA/edit.

When on Unity 2021.2 or later version, the examples also show how to use GPU Compute Shaders to access and modify Mesh vertex buffers.

![NormalMeshScene](https://user-images.githubusercontent.com/30407235/159897872-e3f57c94-df4c-45f0-94ed-cdd638b39073.PNG)


## Normal Mesh Scene
A simple example where **QUAD** mesh has been generated through C# code and then used compute shader(compute buffer & graphic buffer[WIP]) to manipulate/deform it to produce a ripple wave effect.

***Assets\BOG\JobVSNormal\Normal*** is the sample scene. The sample implements a similar computation using a compute shader(compute buffer & grpahic buffer[WIP]) to modify the Mesh vertex buffer, for comparison.

Frame times on 100 resolution quad mesh, on 2021 Windows 10 laptop (Core i5, GTX 1660 Ti, DX 11); note that these are full frame times including rendering:

* Normal C# + compute shader(Compute Buffer): 8ms ~ 11ms
* Normal C# + compute shader(Graphic Buffer[WIP]): 7ms ~ 10ms (Requires Unity 2021.2 or later version)

***When you increase resolution or generate some complex mesh, then you will see big differences in performance***


## Job Mesh Scene
A simple example where **QUAD** mesh has been generated through Unity's Job system and then used compute shader(compute buffer & graphic buffer[WIP]) to manipulate/deform it to produce a ripple wave effect.

***Assets\BOG\JobVSNormal\Job*** is the sample scene.

Frame times on 100 resolution quad mesh, on 2021 Windows 10 laptop (Core i5, GTX 1660 Ti, DX 11); note that these are full frame times including rendering:

* Jobs + compute shader(Compute Buffer): 7ms ~ 9ms
* Jobs + compute shader(Graphic Buffer[WIP]): 6ms ~ 9ms (Requires Unity 2021.2 or later version)

***When you increase resolution or generate some complex mesh, then you will see big differences in performance***

## REFERENCES:
> * https://catlikecoding.com/unity/tutorials/procedural-meshes/square-grid/
> * https://github.com/Unity-Technologies/MeshApiExamples
