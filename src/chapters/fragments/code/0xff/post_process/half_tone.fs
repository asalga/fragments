// http://webstaff.itn.liu.se/~stegu/OpenGLinsights/shadertutorial.html

#ifdef GL_OES_standard_derivatives
#extension GL_OES_standard_derivatives : enable
#endif

precision highp float;

uniform sampler2D u_t0;
uniform vec2 u_res;
uniform float u_time;
const float PI = 3.141592658;
float fq = 100.;

float uScale = 11.;
float uYrot = 11.;

float aastep(float threshold, float v){
  float afwidth = fq;
  return smoothstep(threshold-afwidth, threshold+ afwidth, v);
}


mat2 r2d(float a){
  return mat2(cos(a),sin(a),-cos(a),sin(a));
}

void main(){
  vec2 p = vec2(gl_FragCoord.xy/u_res);// 0..1
  p *= r2d(PI/4.);

  float i;

  // float rad = .5;

  vec2 nearest = fract(fq*p)*2.-1.;
  float dist = length(nearest);

  // repeat uv coords
  vec2 texUV = fract(vec2(gl_FragCoord.xy/u_res));

  float texcol = texture2D(u_t0, texUV).r; // Unrotated coords
  float rad = sqrt(1. - texcol); // Use green channel

  // i = mix(0., 1., step(rad,dist));
  i = step(rad, dist);

  // vec2 grid = fract(fq*p);

  gl_FragColor = vec4(vec3(i),1);
}




// precision mediump float;

// uniform sampler2D u_t0;
// uniform vec2 u_res;
// uniform float u_time;

// float sdCircle(vec2 p, float r){
//   return length(p)-r;
// }

// void main() {
//   vec2 p = gl_FragCoord.xy / u_res;

//   p.y = 1.0 - p.y;

//   // divide up into sections

//   float cnt = 100.;
//   vec2 cell = floor(p*cnt)/cnt;

//   float intensity = (texture2D(u_t0, cell) ).x;
//   float sz = (1./cnt)/2.;
//   vec2 c = vec2(1./cnt);
//   vec2 rp = mod(p, c)-c*0.5;

//   float test = intensity*sz;

//   float i = step(sdCircle(rp, test ),0.);
//   gl_FragColor = vec4(vec3(i),1);
// }
