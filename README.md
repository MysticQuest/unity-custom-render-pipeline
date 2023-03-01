# Custom Render Pipeline

### Unity version: 2019.2.14f1

## Summary:
The purpose of this (WIP) project is to explore rendering through some of [these tutorials](https://catlikecoding.com/unity/tutorials/custom-srp/).

### /Custom RP/Runtime:
A custom render pipeline asset is created and set in Project Settings -> Graphics -> Scriptable Render Pipeline Settings. 
The custom render pipeline class calls a render function for each camera in the scene, passing its constructor parameters along.
Each instance of the camera renderer class is responsible for issuing an order of rendering commands, such as setting up lighting or drawing the geometry in the viewing frustum. This is accomplished through command buffers, a low-level API provided by Unity, for sending grouped rendering commands to the GPU, or setting global shader variables (e.g. in Lighting.cs or Shadows.cs, which use their own command buffers).

### /Custom RP/Shaders:
The project contains an Unlit and a Lit Shader. The Lit Shader includes two passes, one that handles the rendering of materials affected by light, and that handles shadows. Each pass typically includes a vertex and a fragment function. The former is responsible for translating geometry to world space so that it can be rendered from a camera's point of view, and the latter for applying the material properties to this geometry. Calculating these properties is done in separate HLSL files located in /Custom RP/ShaderLibrary, and included in the pass in a specific order.

### /Custom RP/ShaderLibrary:
HLSL scripts responsible for calculating material properties. For instance, Shadows.hlsl gets a shadow atlas texture with depth information from the lights' point of view, provided by Shadows.cs, and samples the UV coodinates of each pixel in the texture to determine the shadow factor of the pixel being rendered by the camera in world space.

Shadow atlas texture for three directional lights with different directions, each using four cascades (sections for rendering shadows in a specific resolution), in a scene with simple geometry:

![image](https://user-images.githubusercontent.com/9077026/222180920-29d72e30-20ca-4b9d-a69b-ad9e4ecfc5cb.png)


### /Scripts:
Mostly fluff for testing - includes a mesh generator that draws a plane mesh defined on a grid of vertices and a fractal tree generator. 
