// 159 - Voxel Canyon
// original implementation from: https://www.shadertoy.com/view/4tsGD7
precision mediump float;

uniform vec2 u_res;
uniform float u_time;

const float STEP = 0.02;
const float MAX_DIST = 20.;
const float SCALE_STEP = 0.01;

float valueNoise(vec2 p){
  #define Y_SCALE 45343.
  #define X_SCALE 37738.
  float x = p.x * X_SCALE;
  float y = p.y * Y_SCALE;
  return fract( sin(x+y) * 23454.);
}

float smoothValueNoise(vec2 p){
  vec2 lv = fract(p);
  lv = smoothstep(0.,1.,lv);
  vec2 id = floor(p);
  float bl = valueNoise(vec2(id));
  float br = valueNoise(vec2(id)+vec2(1,0));
  float b = mix(bl,br,lv.x);

  float tl = valueNoise(vec2(id)+vec2(0,1));
  float tr = valueNoise(vec2(id)+vec2(1,1));
  float t = mix(tl,tr,lv.x);

  return mix(b,t,lv.y);
}

vec3 rayDirection(float fieldOfView, vec2 size, vec2 fragCoord) {
  vec2 xy = fragCoord - size / 2.0;
  float z = size.y / tan(radians(fieldOfView) / 2.0);
  return normalize(vec3(xy, z));
}

void main() {
  // point coord
  vec2 pc = gl_FragCoord.xy/u_res;
  float t = u_time;
  float bkColor = .8 - pc.y;
  vec3 color = vec3(bkColor);
  vec3 rayOrigin = vec3(0., 20., t*5.);

  //or use ray dir
  vec3 dir = vec3(pc-.5, .9);
  dir = normalize(dir);

  float density = .125;

  vec3 flr;
  vec3 frct;
  float depth;

  for(float i = 0.; i < MAX_DIST; i += STEP) {
    // we aren't rendering any voxels at the top of the screen, so
    // save some computation here.
    if(pc.y > 0.5){
      break;
    }

    vec3 p = rayOrigin + (depth * dir);

    flr = floor(p) * density;
    // flr.z *= abs(sin(t/10.));
    // flr.x *= abs(cos(t/20.));

    float n = smoothValueNoise(flr.zx);
    // float n2 = smoothValueNoise(flr.xz+ 100.)/1.;
    // float n = 0.;
    float n2 = 0.;
  // if(flr.x>0.)flr.x = 0.;

    flr.x = pow(flr.x, 2.);

    float cs = n;
    // * abs(sin(t/50.));
    float sn = n2;
    // = 0.;
    // = sin(flr.x);// * abs(sin(t/25. ));
    // sn = flr.x;

    float g = cs+sn;

    if(g > flr.y ) {  // hit
      frct = fract(p);

      float zLine = step(frct.z, 0.9);
      float xLine = step(frct.x, 0.9);

      color = vec3(1.) * zLine * xLine;

      // color white on top, black on bottom
      color = vec3(step(0.5, frct.y) * zLine * xLine);
      break;
    }

    depth += i * SCALE_STEP;
  }

  gl_FragColor = vec4(color, 1);
}