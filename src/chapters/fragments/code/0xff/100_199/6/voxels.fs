// exactly this shader:
// https://www.shadertoy.com/view/4tsGD7
// [2TC 15] Minecraft. Created by Reinder Nijhoff 2015
// @reindernijhoff
// but expanded for learning purposes

// For the most part we're just doing raymarching with some
// extra logic to render the voxels.

// Define some constants.
const float MAX_STEPS = 1000.;
const float GrassHeight = 0.8;
const vec3 Grass = vec3(.2, 1, 0);
const vec3 Dirt = vec3(.5,.25, 0);

// just a main(), no magic yet.
void mainImage(out vec4 fragColor, in vec2 fragCoord) {

  // Okay, we're raymarching right? That means, we'll need to
  // define the ray start position.
  // We can set x to whatever. It doesn't really matter.
  // The y component needs to be a large enough value so we can see the terrain.
  // Z is set to simulation time so that the flyover seems animated.
  vec3 rayOrigin = vec3(0, 5, 0); //iDate.w*5.

  // normalized screenspace coords
  vec2 pnt = fragCoord.xy/iResolution.xy*2.-1.;

  // The ray direction. Unlike the ray origin, the ray direction
  // varies slightly for each fragment--spreading out from -1 to 1.
  vec3 rayDir = vec3(pnt, 1.);

  // The final color we'll assign to this particular fragment.
  vec3 color;

  // The purpose of depth is to scale the ray direction vector.
  float depth = 0.;

  // The raymarching loop.
  for(float i = .0; i < MAX_STEPS; ++i) {

    // Since the top part of the screen space is empty,
    // it makes sense to add a check early on in the
    // loop to just exit if we know we aren't going to hit anything
    // Of course this might need to be adjusted if the rayOrigin
    // changes.
    if(pnt.y > 0.){
      break;
    }

    // Pretty standard raymarching stuff. We start with the origin
    // and scale the rayDirection a bit longer every iteration.
    // The first iteration depth is zero, so we'd just be using the rayOrigin.
    vec3 p = rayOrigin + (rayDir * depth);

    // Okay, now we finally come to something that's necessarily
    // the 'voxel' part of the rendering. By flooring p
    // we are 'snapping' the calculated raymarched point to the
    // nearest 3D box.
    vec3 flr = floor(p) * 1.;

    // Looking at the terrain, there is a wave-like pattern
    // that occurs along the zx plane.


    // The ray hit something!
    if( cos(flr.z) + sin(flr.x) > flr.y ) {

      // Here's w
      vec3 frct = fract(p);
      if(frct.y > GrassHeight){
        color = Grass;
      }
      else{
        color = Dirt;
      }

      // If we hit a voxel the color has been set, so we
      // can safely break out of the loop.
      break;
    }

    // depth is used to scale the ray direction, so we
    // increment it a tiny bit every iteration.
    // Since i ranges from 0 to 1000 and since we are scaling
    // i here, the ray direction will scale from .0 to .1
    depth += i * (.1/MAX_STEPS);
  }

  // We need to add some lighting, even if really fake to
  // give the illusion of distance. It also helps to hide
  // some of the aliasing that happens at the very far points
  // in the terrain.

  // If we exited the loop early on, depth will be a relativley
  // small value which means the fragments will end up being bright.

  // The farther it took to hit something, the darker that fragment will be.
  // Play around with this value.
  color *= 1./(depth/5.);
  fragColor = vec4(color, 1);
}
