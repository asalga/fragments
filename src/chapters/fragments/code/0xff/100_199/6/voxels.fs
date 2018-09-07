// exactly this shader:
// https://www.shadertoy.com/view/4tsGD7
// [2TC 15] Minecraft. Created by Reinder Nijhoff 2015
// @reindernijhoff
// but expanded for learning purposes

const float MAX_STEPS = 100.;
const float GrassHeight = 0.8;

void mainImage(out vec4 fragColor, in vec2 fragCoord) {

  //
  vec3 rayOrigin = vec3(0., 3.5, iDate.w*5.);

  vec3 rayDir = vec3(fragCoord,1)/iResolution-.5;
  vec3 color;
  float depth;

  for(float i = .0; i < MAX_STEPS; i += .1 ) {

    vec3 co = rayOrigin + (rayDir * depth);

    vec3 frct = fract(co);
    vec3 p = floor(co)*0.45;

    if( cos(p.z) + sin(p.x) > p.y ) {

      if((frct.y - .04 * cos((co.x+co.z) * 30.) > GrassHeight)){
        vec3 grass = vec3(.2, 1., 0);
        color = grass/(i/10.);
      }
      else{
        vec3 dirt = vec3(.48,0.25,0);
        color = (frct.y*2. * dirt) / (i/10.);
      }
      break;
    }
    depth += i * .001;
  }

  color *= 2.;
  fragColor = vec4(color, 1);
}
