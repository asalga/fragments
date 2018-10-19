
precision mediump float;

uniform vec2 u_res;
uniform float u_time;

void main(){
  vec2 uv = gl_FragCoord.xy / u_res * 2. - 1.;

  float x = sin(u_time) / 1.;
  float m = 0.2 / distance(uv, vec2(-x, 0.));
  float f = 0.2 / distance(uv, vec2(x, .3));
  float c = .5 * (pow(m, .1) + pow(f, .5));

  gl_FragColor = vec4(vec3(smoothstep(0.7, 0.72,c)),1.);
}
// precision mediump float;

// uniform vec2 u_res;
// uniform float u_time;

// const float speed = 1.;
// const int Count = 10;

// // hash functions from https://www.shadertoy.com/view/4sjGD1
// float hash(float x)
// {
//     return fract(sin(x) * 43758.5453) * 2.0 - 1.0;
// }
// vec2 hashPosition(float x)
// {
//   return vec2(
//     floor(hash(x) * 3.0) * 32.0 + 16.0,
//     floor(hash(x * 1.1) * 2.0) * 32.0 + 16.0
//   );
// }

// void main()// out vec4 fragColor, in vec2 fragCoord )
// {
//   float t = u_time * speed;
//   vec2 p = (gl_FragCoord.xy/u_res) * 2. -1.;
//   // float falloff =  4.3;
//   // vec3 metaBalls[Count];
//   // float d = 0.;
//   // vec2 screen;
//   // float c = 0.;

//   vec2 b1 = vec2(-.5) + sin(t)/2.;
//   vec2 b2 = vec2(.5) - sin(t)/2.;

//   float i = 0.;
//   float c = 0.;
//   float f = 5.;

//   i = .5/distance(b1, p);
//   // i += 11./distance(b2, p);
//   if(i > .84) i = 1.0;
//   c += pow(i, f);

//   i = .5/distance(b2, p);
//   // if(i < 0.85) i = 0.;
//   c += pow(i, f);




//   // for(int i = 0; i < Count; i++){
//   //   float f_it = float(i);
//   //   vec2 pos = hashPosition(f_it) + vec2(time * (1. + f_it));

//   //   screen = floor(mod(pos/u_res, 2.));

//   //   // remap 0..1 to -1 to 1 to avoid branching in next line.
//   //   vec2 dir = screen * 2. - 1.;
//   //   vec2 finalPos = vec2(u_res * (vec2(1.) - screen) + dir * mod(pos, u_res));

//   //   metaBalls[i] = vec3(finalPos, abs(f_it));

//   //   d = 1. / distance(metaBalls[i].xy , gl_FragCoord.xy);
//   //   c += pow(d, falloff);
//   // }

//   gl_FragColor = vec4(vec3(c), 1);
// }