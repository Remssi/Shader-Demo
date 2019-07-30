# Shader-Demo
A few shader and graphic effects I created last summer, including force field, invisibility and camera distraction effect.

# To use the effects:

# Noise effect
Add the script NoiseImageEffect to a gameobject with a camera. Remember also to reference EffectMat material in NoiseImageEffect's public properties.

# Invisibility effect
Add InvisibilityMat material to a gameobject you want to be cloaked.

# Shield effect
Add the prefab under a gameobject you want to be shielded or create an empty gameobject and add ShieldData and ShieldController Components to it and place it under the gameobject you want to be shielded. The effect doesn't work if it doesn't have a parent with Mesh Renderer.

# I'm proud of the particle effect I created for the shield effect.
Unity's trail renderer always got affected by every parent gameobjects' movement, not only by the gameobject's movement in which the component was placed. For example, if a gun had a trail renderer and I want it to be affected by the guns movement and not the characters which holds it, it was not possible with the default trail renderer. Therefore I created my own, and it's trail is only affected by the gameobject which holds the component. I didn't find any reference to how to create such effect from the internet (I only found that other people also looked for this kind of effect) so I had to make it from scratch.

# How I did it
I created empty gameobject which holds the particles and is not any gameobject's children, and calculated the movement of the particles. Then I place the particle holder to follow the gameobject which I wanted to have particles flow around it. Now even though the object was moving fast forward, the lines of the particles got shaped only by the movement of the particles around the object.
