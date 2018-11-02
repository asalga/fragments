precision mediump float;

uniform vec2 u_res;
uniform float u_time;

const int CNT = 30;

float random (vec2 st) {
  return fract(sin(dot(st.xy,vec2(22.9898,78.233)))*43758.5453123);
}
float rand(float i, float seed){
  return fract(sin(i)*seed);//*2.-1.;
}

void main(){
  vec2 uv = gl_FragCoord.xy / u_res;// * 2. - 1.;
  float t = u_time * 1.;
  vec2 pos[CNT];
  vec2 vel[CNT];
  vec2 acc[CNT];

  float c = 0.;;

  float ft = fract(t);

  for(int it = 0; it < CNT; ++it){
    float fi = float(it);

    // pos[it] = vec2(rand(fi, 64134.), rand(fi, 39598.));
    pos[it] = vec2(rand(fi, 34134.), rand(fi, 64134.));

    // vel[it] = vec2(0., -(rand(fi, 39598.)+1.)/2. );
    vel[it] = vec2(0., 1.) * fract(pos[it]);

    pos[it].y = vel[it].y * t * pow(vel[it].y, 1.);
    pos[it].y = mod(pos[it].y, 2.0) - 0.5;
    c += (0.01 * vel[it].y) / pow(distance(uv, pos[it]), 1.9);
  }

  float fog = .1/pow(uv.y, 1.);
  c *= fog;
  c = smoothstep(0.7, 0.72, c);


  gl_FragColor = vec4(vec3(c), 1);
}


  // float m = 0.85 / distance(uv, vec2(-x, 0.));
  // float f = 0.5  / distance(uv, vec2(x, .3));
  // float c = 0.2 * (pow(m, 1.) + pow(f, 1.));
